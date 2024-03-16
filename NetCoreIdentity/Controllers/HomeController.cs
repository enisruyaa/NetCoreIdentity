using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentity.Models;
using NetCoreIdentity.Models.ContextClasses;
using NetCoreIdentity.Models.Entities;
using NetCoreIdentity.Models.ViewModels.AppUsers.PureVms;
using System.Diagnostics;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace NetCoreIdentity.Controllers
{
    [AutoValidateAntiforgeryToken] // Get ile gelen sayfada verilen özel bir token sayesinde Post'un bu tokensiz yapýlmamsýný saðlar.
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;
        readonly SignInManager<AppUser> _signInManager;

        // Identity kütüphanesi crud ve servis iþlemleri için bir takým class'lara sahiptirç Bu manager class'larý sizin ilgili Identity yapýlarýnýzýn Crud iþlemleribe ve baþka bussiness logic iþlemlerine girmesini saðlar kesinlikle ctora yazýlmasý gerekir.

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestModel model)
        {
            if (ModelState.IsValid)
            {
                // Maping iþlemi (UserRequestModel tipindeki model'deki bilgileri AppUser class'ýndan instance alarak oraya aktarýyoruz.)

                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(appUser,model.Password); //Þifreyi burada vermemizin sebebi þifrenin Identity tarafýndan otomatik þifrelenmesi için burayý kullanmamýz gerkiyor.

                if (result.Succeeded)
                {
                    #region AdminEklemekIcinTekKullanimlikKodlar
                    /*
                    AppRole role = await _roleManager.FindByNameAsync("Admin"); // Admin ismindeki rolü bulabilirse Role nesnesini appRole'e atacak bulamazasa null olacak

                    if (role == null) await _roleManager.CreateAsync(new() { Name = "Admin"} ); // Admin isminde bir rol yarattýk

                    await _userManager.AddToRoleAsync(appUser, "Admin"); // appUser deðiþkeninin tuttuðu kullanýcý nesnesini Admin isimli Role'e ekledik.
                    */
                    #endregion

                    #region MmemberEklemekIcinKodlar

                    AppRole appRole = await _roleManager.FindByNameAsync("Member");
                    if (appRole == null) await _roleManager.CreateAsync(new() { Name = "Member" });

                    await _userManager.AddToRoleAsync(appUser, "Member");//Register olan kullanýcý artýk bu kod sayesinde direkt Member rolüne sahip olacaktýr... 

                    #endregion

                    return RedirectToAction("Register");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }

            return View(model);
        }

        public IActionResult SignIn(string returnUrl)
        {
            UserSignInRequestModel usModel = new()
            {
                ReturnUrl = returnUrl
            };

            return View(usModel);
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInRequestModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByNameAsync(model.UserName);

                SignInResult result = await _signInManager.PasswordSignInAsync(appUser, model.Password, model.RememberMe, true);

                if (result.Succeeded) 
                {
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    IList<string> roles = await _userManager.GetRolesAsync(appUser);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("AdminPanel");  //Eger gitmek istediginiz sayfa bir baska Area'da ise routeData parametresine Anonymus type olarak argüman vererek gönderirsiniz return RedirectToAction("AdminPanel",new {Area = "Admin"})
                    }

                    else if (roles.Contains("Member"))
                    {
                        return RedirectToAction("MemberPanel");
                    }

                    return RedirectToAction("Panel");
                }

                else if (result.IsLockedOut)
                {
                    DateTimeOffset? lockOutEndDate = await _userManager.GetLockoutEndDateAsync(appUser);
                    ModelState.AddModelError("",$"Hesabýnýz {(lockOutEndDate.Value.UtcDateTime - DateTime.UtcNow).Minutes } Dakikka Süre Askýya Alýnmýþtýr");
                }

                else
                {
                    string message = "";
                    if (appUser != null)
                    {
                        int maxFailedAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts; //middleware'deki maksimum hata sayýnýz...

                        message = $"Eðer {maxFailedAttempts - await _userManager.GetAccessFailedCountAsync(appUser)} Kez Daha Yanlýþ Giriþ Yaparsanýz Hesabýnýz Geçici Olarak Askýya Alýnacaktýr";

                    }

                    else
                    {
                        message = "Kullanýcý Bulunamadý";
                    }

                    ModelState.AddModelError("", message);
                }
            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles = "Member")]
        public IActionResult MemberPanel()
        {
            return View();
        }

        public IActionResult Panel()
        {
            return View();
        }
    }
}
