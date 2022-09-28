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
        private static string _userTokenKey = "_userToken";
        private static string _docId = "_docId";
        private static decimal _depositFee = 0.001m;

        FirebaseAuthProvider auth;
        private readonly FirestoreProvider _firestoreProvider;
        public HomeController(FirestoreProvider firestoreProvider)
        {
            _firestoreProvider = firestoreProvider;
            auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseWebApiKey._firebaseApiKey));
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString(_userTokenKey);
            var user = await GetCurrentUser();
            
            if (token != null && user != null)
            {
                return View(user);
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
            HttpContext.Session.Remove(_userTokenKey);
            HttpContext.Session.Remove(_docId);
            return RedirectToAction("SignIn");
        }

        public async Task<IActionResult> AccountOverview()
        {
            var user = await GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("SignIn");
            }

            user.ActualBalance = decimal.Parse(user.Balance);
            user.Balance = $"{user.ActualBalance:n2}";
            
            return View(user);
        }

        public async Task<IActionResult> Deposit()
        {
            var user = await GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("SignIn");
            }
            
            user.ActualBalance = decimal.Parse(user.Balance);
            user.Balance = $"{user.ActualBalance:n2}";

            return View(user);
        }
        
        public async Task<IActionResult> Transfer()
        {
            var user = await GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("SignIn");
            }
            
            user.ActualBalance = decimal.Parse(user.Balance);
            user.Balance = $"{user.ActualBalance:n2}";

            return View(user);
        }

        public async Task<RedirectToActionResult> DepositMoneyToAccount(decimal amount)
        {
            var currentUser = await GetCurrentUser();
            var docId = HttpContext.Session.GetString(_docId);

            if (currentUser == null || string.IsNullOrEmpty(docId))
            {
                return RedirectToAction("SignIn");
            }

            var amountToDeposit = decimal.Round(amount - (amount * _depositFee), 2) + decimal.Parse(currentUser.Balance);

            currentUser.Balance = amountToDeposit.ToString("#.##");

            await _firestoreProvider.AddOrUpdate<User>(currentUser, new CancellationToken(), docId);

            return RedirectToAction("AccountOverview");
        }
        
        public async Task<RedirectToActionResult> TransferAmountToOtherAccount(decimal amount, string ibanNo)
        {
            var currentUser = await GetCurrentUser();
            var docId = HttpContext.Session.GetString(_docId);

            if (currentUser == null || string.IsNullOrEmpty(docId))
            {
                return RedirectToAction("SignIn");
            }

            var allUsers = await _firestoreProvider.GetAll<User>(CancellationToken.None);

            var recipient = allUsers.ToArray().FirstOrDefault(x => x.IbanNo == ibanNo);

            if (recipient == null)
            {
                return RedirectToAction("Error", new { errorMsg = "Unable to find recipient's account, please ensure you have entered the correct IBAN" });
            }

            var usersRemainingBalance = decimal.Round(decimal.Parse(currentUser.Balance) - amount, 2);
            currentUser.Balance = usersRemainingBalance.ToString("#.##");

            await _firestoreProvider.AddOrUpdate<User>(currentUser, CancellationToken.None, docId);

            var recipientNewBalance = decimal.Round(amount, 2) + decimal.Parse(recipient.Balance);
            recipient.Balance = recipientNewBalance.ToString("#.##");

            var recipientDocId = recipient.FirstName.ToLower()[0] + recipient.LastName.ToLower();
            await _firestoreProvider.AddOrUpdate<User>(recipient, CancellationToken.None, recipientDocId);

            return RedirectToAction("AccountOverview");
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModel login)
        {
            try
            {
                // log in 
                var authLink = await auth.SignInWithEmailAndPasswordAsync(login.Email, login.Password);

                var token = authLink.FirebaseToken;

                if (token != null && !string.IsNullOrEmpty(authLink.User.DisplayName))
                {
                    HttpContext.Session.SetString(_userTokenKey, token);
                    HttpContext.Session.SetString(_docId, authLink.User.DisplayName);
                    return RedirectToAction("Index");
                }
            }
            catch (FirebaseAuthException ex)
            {
                var exception = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(string.Empty, exception.error.message);
                return View(login);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(LoginModel login)
        {
            try
            {
                var docId = login.FirstName.ToLower()[0] + login.LastName.ToLower();
                
                // create user
                await auth.CreateUserWithEmailAndPasswordAsync(login.Email, login.Password, docId);

                var authLink = await auth.SignInWithEmailAndPasswordAsync(login.Email, login.Password);

                var token = authLink.FirebaseToken;

                if (token != null)
                {
                    HttpContext.Session.SetString(_userTokenKey, token);
                    var newUser = new Dictionary<string, object>
                    {
                        { "FirstName", login.FirstName },
                        { "LastName", login.LastName },
                        { "Email", login.Email },
                        { "IbanNo", GetIbanNoFromGenerator() },
                        { "Balance", "0" } // Firestore lacks support of storing decimal/float for monetary values, workaround for the sake of project
                    };

                    await _firestoreProvider.AddOrUpdate<User>(newUser, new CancellationToken(), docId);
                    
                    HttpContext.Session.SetString(_docId, docId);
                    
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
        public IActionResult Error(string? errorMsg = null)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorMessage = errorMsg });
        }

        private async Task<User?> GetCurrentUser()
        {
            var docId = HttpContext.Session.GetString(_docId);
            return !string.IsNullOrEmpty(docId) ? await _firestoreProvider.Get<User>(docId, new CancellationToken()) : null;
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