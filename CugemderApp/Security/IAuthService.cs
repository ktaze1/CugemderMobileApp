using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.Security
{
    public interface IAuthService
    {
        Task Login(LoginRequest loginRequest);
        Task Register(RegisterRequest registerRequest);
        Task Logout();
        Task ResetPassword(PasswordChangeModel model);
        Task ForgotPassword(ForgotPasswordModel model);
        Task<CurrentUser> CurrentUserInfo();
    }
}
