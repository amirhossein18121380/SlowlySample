﻿using CrossCuttingConcerns.Models;
using Domain.Models;
using SlowlySimulate.Api.ViewModels.Account;
using System.Security.Claims;

namespace SlowlySimulate.Manager;

public interface IAccountManager
{
    Task<ApiResponse> BuildLoginViewModel(string returnUrl);
    Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(ClaimsPrincipal authenticatedUser, HttpContext httpContext, string logoutId);

    Task<ApiResponse> Login(LoginInputModel parameters);
    Task<ApiResponse> LoginWith2fa(LoginWith2faInputModel parameters);
    Task<ApiResponse> LoginWithRecoveryCode(LoginWithRecoveryCodeInputModel parameters);
    Task<ApiResponse> Logout(ClaimsPrincipal authenticatedUser);

    Task<ApiResponse> Register(RegisterViewModel parameters);

    Task<ApiResponse> ConfirmEmail(ConfirmEmailViewModel parameters);

    Task<ApiResponse> ForgotPassword(ForgotPasswordViewModel parameters);
    Task<ApiResponse> ResetPassword(ResetPasswordViewModel parameters);
    Task<ApiResponse> UpdatePassword(ClaimsPrincipal authenticatedUser, UpdatePasswordViewModel parameters);

    Task<ApiResponse> EnableAuthenticator(ClaimsPrincipal authenticatedUser, AuthenticatorVerificationCodeViewModel parameters);
    Task<ApiResponse> DisableAuthenticator(ClaimsPrincipal authenticatedUser);
    Task<ApiResponse> ForgetTwoFactorClient(ClaimsPrincipal authenticatedUser);
    Task<ApiResponse> Enable2fa(ClaimsPrincipal authenticatedUser);
    Task<ApiResponse> Disable2fa(ClaimsPrincipal authenticatedUser);

    Task<ApiResponse> UserViewModel(ClaimsPrincipal authenticatedUser);

    Task<ApiResponse> UpdateUser(UserViewModel userViewModel);

    Task<ApiResponse> Create(RegisterViewModel parameters);

    Task<ApiResponse> Delete(string id);

    ApiResponse GetUser(ClaimsPrincipal authenticatedUser);

    Task<ApiResponse> AdminUpdateUser(UserViewModel userViewModel);

    Task<ApiResponse> AdminResetUserPasswordAsync(ChangePasswordViewModel changePasswordViewModel, ClaimsPrincipal authenticatedUser);

    Task<User> RegisterNewUserAsync(string userName, string email, string password, bool requireConfirmEmail);
}