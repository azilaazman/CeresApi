using CeresAPI.Models;
using CeresAPI.Models.AccountModel;
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
        public async Task<HttpResponseMessage> Register([FromBody] Account account)
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
                        createAccount.API_Key = Helper.GeneratePassword(35);
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

        private string GetToken(string url, string userName, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>( "grant_type", "password" ),
                        new KeyValuePair<string, string>( "username", userName ),
                        new KeyValuePair<string, string> ( "Password", password )
                    };
            var content = new FormUrlEncodedContent(pairs);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url + "Token", content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> Login([FromBody] Login loginInfo)
        {
            if (ModelState.IsValid)
            {
                String ipaddr = GetUserIPAddress();
                IMongoDatabase db = Connection.getMLabConnection();
                List<CreateAccount> accountList = new List<CreateAccount>();
                var accountInfo = db.GetCollection<CreateAccount>("account");
                // if username exist
                var condition = Builders<CreateAccount>.Filter.Eq("email", loginInfo.email);
                var results = accountInfo.Find(condition).ToList();
                Console.WriteLine("result" + results.ToJson());
                return Request.CreateResponse(HttpStatusCode.Continue, ipaddr);
            }

            // Require the user to have a confirmed email before they can log on.
            // var user = await UserManager.FindByNameAsync(model.Email);

            //connect to db
            //var user = loginInfo.Find(loginInfo.email, loginInfo.password);
            //if (user != null)
            //{
            //    if (!await UserManager.IsEmailConfirmedAsync(user.Id))
            //    {
            //        string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account-Resend");

            //        // Uncomment to debug locally  
            //        ViewBag.Link = callbackUrl;
            //        ViewBag.errorMessage = "You must have a confirmed email to log on. "
            //                             + "The confirmation token has been resent to your email account.";
            //        return View("Error");
            //    }
            //}

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            //    switch (result)
            //    {
            //        case SignInStatus.Success:
            //            return RedirectToLocal(returnUrl);
            //        case SignInStatus.LockedOut:
            //            return View("Lockout");
            //        case SignInStatus.RequiresVerification:
            //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //        case SignInStatus.Failure:
            //        default:
            //            ModelState.AddModelError("", "Invalid login attempt.");
            //            return View(model);
            //    }
            //}
            return Request.CreateResponse(HttpStatusCode.Accepted, "ok");
        }
    }
}
