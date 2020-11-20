using CugemderApp.Server.Model;
using CugemderApp.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;


namespace CugemderApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly CugemderMobileAppDbContext _dbContext;

        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            CugemderMobileAppDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return BadRequest("Kullanıcı bulunamadı");
            var singInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!singInResult.Succeeded) return BadRequest("Hatalı şifre ya da onaylanmamış kullanıcı");

            await _signInManager.SignInAsync(user, request.RememberMe);
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> SendEmailForConfirmation(ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.email);
            if (user == null) return BadRequest("Kayıtlı e-mail bulunamadı veya yanlış");

            var newID = string.Format(@"{0}", Guid.NewGuid());

            _dbContext.ForgotPasswordRequests.Add(new ForgotPasswordRequests()
            {
                UserEmail = model.email,
                ExpireDate = DateTime.Now.AddDays(1),
                Id = newID
            });

            await _dbContext.SaveChangesAsync();

            var link = $"http://api.cugemder.com/api/auth/ResetPassword?id={newID}";

            //TODO Send email

            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress($"{user.Email}", $"{user.FirstName} { user.LastName}"));
                message.From = new MailAddress("beeportsifre@gmail.com", "BeePort");
                message.Subject = "Sifre Değişikliği";
                message.Body = $"Yeni bir şifre almak için linke tıklayın: <a href=\"{link}\">{link}</a> ";
                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("beeportsifre@gmail.com", "184589Be-"); // TODO ÇÜGEMDER Mail Gir
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword([FromQuery] string id)
        {
            var resetRequest = await _dbContext.ForgotPasswordRequests.FindAsync(id);
            var newPassword = Password.Generate(6, 1);


            while (!Regex.IsMatch(newPassword, @"^(?=.{6,})(?=.*[1-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[(!@#$%^&*()_+\- =\`{}[\]:”;'<>?,.\/, )])(?!.*(.)\1{2,}).+$"))
            {
                newPassword = Password.Generate(6, 1);
            }

            if (resetRequest != null && resetRequest.ExpireDate > DateTime.Now)
            {
                var user = await _userManager.FindByEmailAsync(resetRequest.UserEmail);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                await _userManager.ResetPasswordAsync(user, token, newPassword);

                using (var message = new MailMessage())
                {
                    message.To.Add(new MailAddress($"{user.Email}", $"{user.FirstName} { user.LastName}"));
                    message.From = new MailAddress("beeportsifre@gmail.com", "BeePort");
                    message.Subject = "Yeni Şifreniz";
                    message.Body = $"Yeni şifreniz: {newPassword} ";
                    message.IsBodyHtml = true;

                    using (var client = new SmtpClient("smtp.gmail.com", 587))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("beeportsifre@gmail.com", "184589Be-"); // TODO : ÇÜGEMDER Mail gir
                        client.EnableSsl = true;
                        client.Send(message);
                    }
                }
            }

            //send new password

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordChangeModel passwordChange)
        {
            var user = await _userManager.FindByIdAsync(passwordChange.id);
            if (user == null) return BadRequest("Kullanıcı bulunamadı");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, passwordChange.newPassword);
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest parameters)
        {
            var user = new ApplicationUser()
            {
                UserName = parameters.Email,
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
                Email = parameters.Email,
                PhotoUrl = parameters.photoUrl,
                DateOfBirth = parameters.DateofBirth,
                PhoneNumber = parameters.PhoneNo
            };
            var result = await _userManager.CreateAsync(user, parameters.Password);
            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()?.Description);

            _dbContext.Points.Add(new Points { AddedBy = "Yeni Kullanici", TotalPoints = 0, UserId = user.Id, UpdatedAt = DateTime.Now });
            await _dbContext.SaveChangesAsync();


            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet]
        public CurrentUser CurrentUserInfo()
        {
            return new CurrentUser
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                Claims = User.Claims
                .ToDictionary(c => c.Type, c => c.Value)
            };
        }
    }

    public static class Password
    {
        private static readonly char[] Punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();

        public static string Generate(int length, int numberOfNonAlphanumericCharacters)
        {
            if (length < 1 || length > 128)
            {
                throw new ArgumentException(nameof(length));
            }

            if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
            {
                throw new ArgumentException(nameof(numberOfNonAlphanumericCharacters));
            }

            using (var rng = RandomNumberGenerator.Create())
            {
                var byteBuffer = new byte[length];

                rng.GetBytes(byteBuffer);

                var count = 0;
                var characterBuffer = new char[length];

                for (var iter = 0; iter < length; iter++)
                {
                    var i = byteBuffer[iter] % 87;

                    if (i < 10)
                    {
                        characterBuffer[iter] = (char)('0' + i);
                    }
                    else if (i < 36)
                    {
                        characterBuffer[iter] = (char)('A' + i - 10);
                    }
                    else if (i < 62)
                    {
                        characterBuffer[iter] = (char)('a' + i - 36);
                    }
                    else
                    {
                        characterBuffer[iter] = Punctuations[i - 62];
                        count++;
                    }
                }

                if (count >= numberOfNonAlphanumericCharacters)
                {
                    return new string(characterBuffer);
                }

                int j;
                var rand = new Random();

                for (j = 0; j < numberOfNonAlphanumericCharacters - count; j++)
                {
                    int k;
                    do
                    {
                        k = rand.Next(0, length);
                    }
                    while (!char.IsLetterOrDigit(characterBuffer[k]));

                    characterBuffer[k] = Punctuations[rand.Next(0, Punctuations.Length)];
                }

                return new string(characterBuffer);
            }
        }
    }
}
