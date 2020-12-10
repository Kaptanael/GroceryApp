using GroceryApp.Data;
using GroceryApp.Models;
using GroceryApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GroceryApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUnitOfWork uow, ILogger<AccountController> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserForRegisterViewModel userForRegisterViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    byte[] passwordHash, passwordSalt;

                    CreatePasswordHash(userForRegisterViewModel.Password, out passwordHash, out passwordSalt);

                    var userToCreate = new User
                    {
                        UserName = userForRegisterViewModel.UserName.ToLower(),
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        Email = userForRegisterViewModel.Email.ToLower()                        
                    };

                    await _uow.Users.AddAsync(userToCreate);
                    var result = await _uow.SaveAsync();

                    if (result > 0)
                    {
                        TempData["Message"] = "Registered Successfully";
                        TempData["Status"] = "success";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                TempData["Message"] = "Failed to register";
                TempData["Status"] = "danger";
            }

            return RedirectToAction(nameof(Register));
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserForLoginViewModel userForLoginViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userFromRepo = await _uow.Users.GetUserByUserNameAsync(userForLoginViewModel.UserName.ToLower());

                    if (userFromRepo == null)
                    {
                        ModelState.AddModelError(string.Empty, "Username or password is incorrect.");
                        return View(nameof(Login));
                    }

                    if (!VerifyPasswordHash(userForLoginViewModel.Password, userFromRepo.PasswordHash, userFromRepo.PasswordSalt))
                    {
                        ModelState.AddModelError(string.Empty, "Username or password is incorrect.");
                        return View(nameof(Login));
                    }

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, userFromRepo.UserName),
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = userForLoginViewModel.RememberMe == true ? true : false
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
                }
            }            
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UserForChangePasswordViewModel userForChangePasswordViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(User.Identity.Name))
                    {
                        var userFromRepo = await _uow.Users.GetUserByUserNameAsync(User.Identity.Name.ToLower());

                        if (userFromRepo == null)
                        {
                            ModelState.AddModelError(string.Empty, "User does not exist.");
                            return View(nameof(ChangePassword));
                        }

                        if (!VerifyPasswordHash(userForChangePasswordViewModel.OldPassword, userFromRepo.PasswordHash, userFromRepo.PasswordSalt))
                        {
                            ModelState.AddModelError(string.Empty, "Old password is incorrect.");
                            return View(nameof(ChangePassword));
                        }

                        byte[] passwordHash, passwordSalt;

                        CreatePasswordHash(userForChangePasswordViewModel.NewPassword, out passwordHash, out passwordSalt);

                        var userToUpdate = new User
                        {
                            UserId = userFromRepo.UserId,
                            UserName = userFromRepo.UserName.ToLower(),
                            PasswordHash = passwordHash,
                            PasswordSalt = passwordSalt,
                            Email = userFromRepo.Email.ToLower()                            
                        };

                        _uow.Users.Update(userToUpdate);
                        var result = _uow.Save();

                        if (result > 0)
                        {
                            TempData["Message"] = "Password changed Successfully";
                            TempData["Status"] = "success";
                        }
                    }
                }
            }            
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                TempData["Message"] = "Failed to change password";
                TempData["Status"] = "danger";
            }

            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsUsernameInUse(string username)
        {
            var userFromRepo = await _uow.Users.GetUserByUserNameAsync(username);

            if (userFromRepo == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Username {username} is already in use.");
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var userFromRepo = await _uow.Users.GetUserByEmailAsync(email);

            if (userFromRepo == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
