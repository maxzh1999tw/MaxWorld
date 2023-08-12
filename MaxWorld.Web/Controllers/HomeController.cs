using MaxWorld.Web.Filters;
using MaxWorld.Web.Models;
using MaxWorld.Web.Services;
using MaxWorld.Web.Utilities.MailSenders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MaxWorld.Web.Controllers
{
    [CustomAuthorize]
    public class HomeController : BaseController
    {
        private readonly AuthService _authService;
        private readonly MailHelper _mailHelper;

        public HomeController(
            AuthService authService,
            MailHelper mailHelper,
            BaseControllerArgument baseControllerArgument) : base(baseControllerArgument)
        {
            _authService = authService;
            _mailHelper = mailHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region === Login ===

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (SessionUserInfo != null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ApiExceptionFilter]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (SessionUserInfo != null)
            {
                return BadRequest();
            }

            var user = await _authService.LoginAsync(email, password);
            if (user == null)
            {
                return ApiFailed("Invalid");
            }

            SessionUserInfo = new SessionUserInfo(user.UserId, user.Name);
            return ApiSuccess();
        }

        #endregion

        #region === Register ===
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (SessionUserInfo != null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ApiExceptionFilter]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string email, string password, string name)
        {
            if (SessionUserInfo != null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return ApiFailed(InvalidModelState);
            }

            if (await _authService.IsAccountRegisteredAsync(email))
            {
                return ApiFailed("Registered");
            }

            using var trans = Repository.BeginTransaction();
            Guid userId = await _authService.RegisterAsync(email, password, name);
            trans.Commit();

            SessionUserInfo = new SessionUserInfo(userId, name);
            return ApiSuccess();
        }

        #endregion

        #region === ForgotPassword ===

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            if (SessionUserInfo != null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ApiExceptionFilter]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (SessionUserInfo != null)
            {
                return BadRequest();
            }

            if (!await _authService.IsAccountRegisteredAsync(email))
            {
                return ApiFailed("Invalid");
            }

            var resetToken = await _authService.GeneratePasswordResetTokenAsync(email);
            string scheme = Url.ActionContext.HttpContext.Request.Scheme;
            var resetUrl = $"{Url.Action("ResetPassword", "Home", null, scheme)}?token={resetToken}";
            await _mailHelper.SendAsync(email, "重設密碼信", $"請<a href='{resetUrl}'>點擊此處</a>重設密碼。");

            return ApiSuccess();
        }

        [AllowAnonymous]
        public IActionResult ResetPassword()
        {
            if (SessionUserInfo != null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ApiExceptionFilter]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string token, string password)
        {
            if (SessionUserInfo != null)
            {
                return BadRequest();
            }

            var userId = await _authService.GetUserIdByPasswordResetTokenAsync(token);
            if (userId == null)
            {
                return ApiFailed("Invalid");
            }

            var user = await _authService.ResetPasswordAsync(userId.Value, password);
            SessionUserInfo = new SessionUserInfo(userId.Value, user.Name);
            return ApiSuccess();
        }

        #endregion

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}