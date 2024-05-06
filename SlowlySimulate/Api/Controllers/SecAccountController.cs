using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SlowlySimulate.Api.Hubs;
using SlowlySimulate.Api.ViewModels.Account;
using SlowlySimulate.Application.Users.Services;
using SlowlySimulate.CrossCuttingConcerns.Models;
using SlowlySimulate.Domain.Identity;
using SlowlySimulate.Infrastructure;
using SlowlySimulate.Persistence.Permissions;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace SlowlySimulate.Api.Controllers;


public class SecAccountController : Controller
{
    private readonly IAccountManager _accountManager;
    private readonly ApiResponse _invalidData;
    private readonly IHubContext<HubSample, IHubSample> _hubContext;
    private readonly IUserService _userService;
    private readonly ICurrentUser _currentUser;
    public SecAccountController(IAccountManager accountManager, IHubContext<HubSample, IHubSample> hubContext
        , ICurrentUser userIdProvider,
        IUserService userService)
    {
        _accountManager = accountManager;
        _hubContext = hubContext;
        _userService = userService;
        _currentUser = userIdProvider;
        _invalidData = new ApiResponse(Status400BadRequest, "InvalidData");
    }
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInputModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {

            var result = await _accountManager.Login(model);

            if (result.IsSuccessStatusCode)
            {

                // Redirect back to the original URL (returnUrl) after successful login
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home"); // Default redirect if returnUrl is not local
                }
            }

            switch (result.Message)
            {
                case "Two factor authentication required":
                //return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

                case "LockedUser":
                    return View("Lockout");

                case "EmailNotConfirmed":
                    ModelState.AddModelError(string.Empty, "User not allowed to log in because email is not confirmed");
                    break;

                case "LoginFailed":
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    break;

                default:
                    ModelState.AddModelError(string.Empty, "Unexpected login result.");
                    break;
            }
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(SlowlySimulate.Api.ViewModels.Account.RegisterViewModel parameters, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            var result = await _accountManager.Register(parameters);
            if (result.IsSuccessStatusCode)
            {
                //var sameCountryGroupUser = await _userService.GetSameCountryUsers(_currentUser.UserId);
                return RedirectToLocal(returnUrl);
            }
            AddErrors(result);
        }
        // If we got this far, something failed, redisplay form
        return View(parameters);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOff()
    {
        await _accountManager.Logout(User);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [AllowAnonymous]
    [HttpPost("ConfirmEmail")]
    public async Task<ActionResult<ApiResponse>> ConfirmEmail(ConfirmEmailViewModel parameters)
    {
        if (ModelState.IsValid)
        {
            var response = await _accountManager.ConfirmEmail(parameters);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ConfirmEmail", "SecAccount");
            }
            return View("Error");
        }
        return View("Error");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel parameters)
    {
        if (ModelState.IsValid)
        {
            var response = await _accountManager.ForgotPassword(parameters);
            if (response.IsSuccessStatusCode)
            {// Don't reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }
            return View("ForgotPasswordConfirmation");
        }

        // If we got this far, something failed, redisplay form
        return View(parameters);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string code = null)
    {
        return code == null ? View("Error") : View();
    }

    [HttpPost("ResetPassword")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var response = await _accountManager.ResetPassword(model);

        if (response.StatusCode == Status404NotFound)
        {
            // Don't reveal that the user does not exist
            return RedirectToAction(nameof(SecAccountController.ResetPasswordConfirmation), "SecAccount");
        }

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(SecAccountController.ResetPasswordConfirmation), "SecAccount");
        }

        AddErrors(response);
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }











    [HttpPost("EnableAuthenticator")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status401Unauthorized)]
    public async Task<ApiResponse> EnableAuthenticator(AuthenticatorVerificationCodeViewModel parameters)
    => ModelState.IsValid ? await _accountManager.EnableAuthenticator(User, parameters) : _invalidData;

    [HttpPost("DisableAuthenticator")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status401Unauthorized)]
    public async Task<ApiResponse> DisableAuthenticator()
    => await _accountManager.DisableAuthenticator(User);

    [HttpPost("ForgetTwoFactorClient")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status401Unauthorized)]
    public async Task<ApiResponse> ForgetTwoFactorClient()
    => await _accountManager.ForgetTwoFactorClient(User);

    [HttpPost("Enable2fa")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status401Unauthorized)]
    public async Task<ApiResponse> Enable2fa()
    => await _accountManager.Enable2fa(User);

    [HttpPost("Disable2fa")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status401Unauthorized)]
    public async Task<ApiResponse> Disable2fa()
    => await _accountManager.Disable2fa(User);














    [HttpPost("UpdatePassword")]
    public async Task<ApiResponse> UpdatePassword(UpdatePasswordViewModel parameters)
        => ModelState.IsValid ? await _accountManager.UpdatePassword(User, parameters) : _invalidData;

    [HttpGet("UserViewModel")]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status401Unauthorized)]
    public async Task<ApiResponse> UserViewModel()
    => await _accountManager.UserViewModel(User);

    [HttpPost("UpdateUser")]
    [Authorize]
    public async Task<ApiResponse> UpdateUser(UserViewModel userViewModel)
    => ModelState.IsValid ? await _accountManager.UpdateUser(userViewModel) : _invalidData;

    ///----------Admin User Management Interface Methods
    // POST: api/Account/Create
    [HttpPost("Create")]
    [Authorize(Permissions.User.Create)]
    public async Task<ApiResponse> Create(RegisterViewModel parameters)
    => ModelState.IsValid ? await _accountManager.Create(parameters) : _invalidData;

    // DELETE: api/Account/5
    [HttpDelete("{id}")]
    [Authorize(Permissions.User.Delete)]
    public async Task<ApiResponse> Delete(string id)
    => await _accountManager.Delete(id);

    [HttpGet("GetUser")]
    public ApiResponse GetUser()
    => _accountManager.GetUser(User);

    [HttpPost("AdminUpdateUser")]
    [Authorize(Permissions.User.Update)]
    public async Task<ApiResponse> AdminUpdateUser([FromBody] UserViewModel userViewModel)
    => ModelState.IsValid ? await _accountManager.AdminUpdateUser(userViewModel) : _invalidData;

    [HttpPost("AdminUserPasswordReset")]
    [Authorize(Permissions.User.Update)]
    [ProducesResponseType(Status204NoContent)]
    public async Task<ApiResponse> AdminResetUserPasswordAsync(ChangePasswordViewModel changePasswordViewModel)
    => ModelState.IsValid ? await _accountManager.AdminResetUserPasswordAsync(changePasswordViewModel, User) : _invalidData;




    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
    private void AddErrors(ApiResponse result)
    {
        ModelState.AddModelError(string.Empty, result.Message);
    }
}

