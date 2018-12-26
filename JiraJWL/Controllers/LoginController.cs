using JiraJWL.Models;
using Microsoft.AspNet.Identity.Owin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JiraJWL.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true

            bool resultSign = SignJIRA(model.Email, model.Password);

            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            if (resultSign)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
               
        private bool SignJIRA(string email, string password)
        {
            bool resultSign = false;
            var client = new RestClient("https://meng-mod-04.atlassian.net/rest/auth/1/session");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"username\": \"" + email + "\",\r\n    \"password\": \"" + password + "\"\r\n}", ParameterType.RequestBody);
            var response = client.Execute(request);

            if (response.StatusCode.ToString() == "OK")
            {
                Session["name"] = response.Cookies[1].Name;
                Session["value"] = response.Cookies[1].Value;
                resultSign = true;
            }
            else
            {
                resultSign = false;
            }
            return resultSign;
        }
    }
}