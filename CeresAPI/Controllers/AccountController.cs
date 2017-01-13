using CeresAPI.Models;
using CeresAPI.Models.AccountModel;
using MongoDB.Bson;
using MongoDB.Driver;
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
        [HttpPost]
        [Route("api/Account/Create")]
        public async void CreateAccount([FromBody]Account account)
        {
            // connect to mLab
            MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
            IMongoDatabase db = client.GetDatabase("ceres_unit1");
            List<Plant> plant_list = new List<Plant>();
            var plantInfo = db.GetCollection<Plant>("account");

            var allPlants
                = await plantInfo.Find(new BsonDocument()).ToListAsync(); //get all documents in the collection
            //foreach (Plant plant in allPlants)
            //{
            //	plant_list.Add(plant);
            //}
            //return allPlants.ToJson();
            //return plantInfo.ToJson();

        }

        [HttpPost]
        [Route("api/Account/Register")]
        [AllowAnonymous]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> Register([FromBody] Account account)
        {
            if (ModelState.IsValid)
            {
                //var user = new Account { UserName = model.Email, Email = model.Email };
                //var result = await UserManager.CreateAsync(user, model.Password);

                //hash the pass with md5
                String salt = Helper.GeneratePassword(128);
                String saltedPassword = Helper.EncodePassword(account.password, salt);
                

                //mongo connection

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
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, ModelState);
            }
            //AddErrors(result);
            return Request.CreateResponse(HttpStatusCode.Created, account);

        }
        [HttpPost]
        [AllowAnonymous]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> Login([FromBody] Login loginInfo)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            // Require the user to have a confirmed email before they can log on.
            // var user = await UserManager.FindByNameAsync(model.Email);

            //connect to db
            var user = loginInfo.Find(loginInfo.email, loginInfo.password);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account-Resend");

                    // Uncomment to debug locally  
                    ViewBag.Link = callbackUrl;
                    ViewBag.errorMessage = "You must have a confirmed email to log on. "
                                         + "The confirmation token has been resent to your email account.";
                    return View("Error");
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
    }
}
