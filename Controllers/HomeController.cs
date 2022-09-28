using CoombsBank.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CoombsBank.Constants;
using CoombsBank.Providers;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using User = CoombsBank.Models.User;

namespace CoombsBank.Controllers
{
    public class HomeController : Controller
    {
        private static string userTokenKey = "_userToken";
        
        FirebaseAuthProvider auth;
        private readonly FirestoreProvider _firestoreProvider;
        public HomeController(FirestoreProvider firestoreProvider)
        {
            _firestoreProvider = firestoreProvider;
            auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseWebApiKey._firebaseApiKey));
        }

        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString(userTokenKey);

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

                var token = authLink.FirebaseToken;

                if (token != null)
                {
                    HttpContext.Session.SetString(userTokenKey, token);

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

                var token = authLink.FirebaseToken;

                if (token != null)
                {
                    HttpContext.Session.SetString(userTokenKey, token);

                    var docId = login.FirstName.ToLower()[0] + login.LastName.ToLower();
                    var newUser = new Dictionary<string, object>
                    {
                        { "FirstName", login.FirstName },
                        { "LastName", login.LastName },
                        { "Email", login.Email },
                        { "IbanNo", GetIbanNoFromGenerator() }
                    };

                    await _firestoreProvider.AddOrUpdate<User>(newUser, new CancellationToken(), docId);

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

        private static string GetIbanNoFromGenerator()
        {
            var driver = new ChromeDriver("C:/Users/USER/Desktop/CoombsBank/CoombsBank/Drivers");
            driver.Navigate().GoToUrl("http://randomiban.com/?country=Netherlands");
            driver.Manage().Window.Minimize();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            var element = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='demo']")));

            return element.Text;
        }
    }
}