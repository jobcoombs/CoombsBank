using CoombsBank.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CoombsBank.Constants;
using Newtonsoft.Json;

namespace CoombsBank.Controllers
{
    public class HomeController : Controller
    {
        FirebaseAuthProvider auth;
        public HomeController()
        {
            auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseWebApiKey._firebaseApiKey));
        }

        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("_UserToken");

            if (token != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("_userToken");
            return RedirectToAction("SignIn");
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModel login)
        {
            try
            {
                // log in 
                var authLink = await auth.SignInWithEmailAndPasswordAsync(login.Email, login.Password);

                string token = authLink.FirebaseToken;

                if (token != null)
                {
                    HttpContext.Session.SetString("_userToken", token);

                    return RedirectToAction("Index");
                }
            }
            catch (FirebaseAuthException ex)
            {
                var exception = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, exception.error.message);
                return View(login);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(LoginModel login)
        {
            try
            {
                // create user
                await auth.CreateUserWithEmailAndPasswordAsync(login.Email, login.Password);

                var authLink = await auth.SignInWithEmailAndPasswordAsync(login.Email, login.Password);

                string token = authLink.FirebaseToken;

                if (token != null)
                {
                    HttpContext.Session.SetString("_userToken", token);

                    return RedirectToAction("Index");
                }
            }
            catch (FirebaseAuthException ex)
            {
                var exception = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, exception.error.message);
                return View(login);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}