using Application.DTOs.Account;
using Application.Wrappers;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<Response<string>> CreateUserAsync(CreateUserRequest request, string origin);
        Task<Response<string>> CreateManagementUserAsync(CreateManagementUserRequest request, string origin);
        Task<Response<string>> UpdateUserRestrictionAsync(string email, bool isRestricted);

        Task CheckMailServer();



    }
}
