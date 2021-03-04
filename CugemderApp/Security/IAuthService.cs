using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.Security
{
    public interface IAuthService
    {
        Task<HttpResponseMessage> Login(LoginRequest loginRequest);
        Task Register(RegisterRequest registerRequest);
        Task Logout();
        Task SendConfirmationEmail(string id);
        Task ResetPassword(PasswordChangeModel model);
        Task ForgotPassword(ForgotPasswordModel model);
        Task<CurrentUser> CurrentUserInfo();
        Task SendNewUserEmail(string user);
    }
}
