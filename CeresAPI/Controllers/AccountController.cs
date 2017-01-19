using CeresAPI.Models;
using CeresAPI.Models.AccountModel;
using CeresAPI.Models.AccountModel.login;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CeresAPI.Controllers
{
    public class AccountController : ApiController
    {
        private static string GetUserIPAddress()
        {
            var context = System.Web.HttpContext.Current;
            string ip = String.Empty;

            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (!String.IsNullOrWhiteSpace(context.Request.UserHostAddress))
                ip = context.Request.UserHostAddress;

            if (ip == "::1")
                ip = "127.0.0.1";

            return ip;
        }

        [HttpPost]
        [Route("api/Account/Register")]
        [AllowAnonymous]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> Register([FromBody] CreateAccount account)
        {
            if (ModelState.IsValid)
            {

                //mongo connection
                IMongoDatabase db = Connection.getMLabConnection();

                var accountInfo = db.GetCollection<Account>("account");
                // if username exist
                var condition = Builders<Account>.Filter.Eq("email", account.email);
                var results = accountInfo.Find(condition).ToList();
                if (results.Count == 1)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, account.email + " is created.");
                }
                else
                {
                    // if the username !exist
                    try
                    {
                        //create object to send to mongo db
                        CreateAccount createAccount = new CreateAccount();

                        //generate a salt
                        String salt = Helper.GeneratePassword(10);
                        createAccount.salt = salt;
                        //encrypt password with salt and sha1
                        //hash the pass with md5
                        String saltedPassword = Helper.EncodePassword(account.password, salt);
                        createAccount.password = saltedPassword;
                        createAccount.name = account.name;
                        createAccount.email = account.email;
                        createAccount.unit_id = account.unit_id;
                        //send to mongo
                        var createAccountInfo = db.GetCollection<CreateAccount>("account");
                        await createAccountInfo.InsertOneAsync(createAccount);
                        return Request.CreateResponse(HttpStatusCode.Created, createAccount);
                    }
                    catch (AggregateException aggEx)
                    {
                        aggEx.Handle(x =>
                        {
                            var mwx = x as MongoWriteException;
                            if (mwx != null && mwx.WriteError.Category == ServerErrorCategory.DuplicateKey)
                            {
                                // mwx.WriteError.Message contains the duplicate key error message
                                return true;
                            }
                            return false;
                        });
                    }
                }
                //if (result.Succeeded)
                //{
                //    //  Comment the following line to prevent log in until the user is confirmed.
                //    //  await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                //    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //    var callbackUrl = Url.Action("ConfirmEmail", "Account",
                //       new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //    await UserManager.SendEmailAsync(user.Id, "Confirm your account",
                //       "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                //    // Uncomment to debug locally 
                //    // TempData["ViewBagLink"] = callbackUrl;

                //    ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                //                    + "before you can log in.";

                //return View("Info");
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            //AddErrors(result);
            return Request.CreateResponse(HttpStatusCode.Created, "Created");

        }

        [HttpPost]
        [AllowAnonymous]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public List<CreateAccount> Login([FromBody] Login loginInfo)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            IMongoDatabase db = Connection.getMLabConnection();
            List<CreateAccount> accountList = new List<CreateAccount>();
            var accountInfo = db.GetCollection<CreateAccount>("account");
            var currentPlantData = db.GetCollection<CurrentPlantData>("currentPlantData");

            // if username exist
            var condition = Builders<CreateAccount>.Filter.Eq("email", loginInfo.email);
            var fields = Builders<CreateAccount>.Projection.Include(p => p.name).Include(p => p._id).Include(p => p.unit_id);
            var results = accountInfo.Find(condition).Project<CreateAccount>(fields).ToList();
            Console.WriteLine("result" + results.ToJson());

            return results;

        }
    }
}
