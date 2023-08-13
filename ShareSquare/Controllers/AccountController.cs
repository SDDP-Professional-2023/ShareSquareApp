using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Encodings.Web;
using ShareSquare.Data.Models;
using ShareSquare.Data.Models.Domain;
using ShareSquareApp.Services.IServices;
using ShareSquareApp.Services;

namespace ShareSquare.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IEmailService _emailService;
        private readonly UrlEncoder _urlEncoder;
        private readonly IErrorService _errorService;

        // making use of dependency injection 
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender, UrlEncoder urlEncoder, IEmailService emailService, IErrorService errorService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _urlEncoder = urlEncoder;
            _emailService = emailService;
            _errorService = errorService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register(string? returnurl = "")
        {
            try { 
                ViewData["ReturnUrl"] = returnurl;
                RegisterViewModel registerViewModel = new RegisterViewModel();
                return View(registerViewModel);
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnurl = "")
        {
            try
            {
                ViewData["ReturnUrl"] = returnurl;

                // checks the data submitted in the form complies with all the validation rules
                // deined in the model
                if (ModelState.IsValid)
                {
                    // create the object for the application user
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name, PhoneNumber = model.PhoneNumber };
                    // create the user at the database level 
                    var result = await _userManager.CreateAsync(user, model.Password);

                    // checks if the user creation was successful
                    if (result.Succeeded)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        // Generate the callback URL for resetting the password
                        var callbackurl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                        //// Send the password reset link via email to the user
                        //_emailService.SendEmail(model.Email, "Confirm your account - Identity Manager",
                        //    "Please confirm your account by clicking here: <a href=\"" + callbackurl + "\">link</a>");

                        // signs the user into the application 
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        // Check if the returnUrl is local. If not, redirect to the default page.
                        if (Url.IsLocalUrl(returnurl))
                        {
                            return LocalRedirect(returnurl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Item");
                        }
                    }

                    // add errors once user is not created successfully 
                    AddErrors(result);
                }

                return View(model);
            }            
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            try { 
                // Check if the user ID or confirmation code are null
                // If any of them is null, it indicates an error in the request, hence, display the error view
                if (userId == null || code == null)
                {
                    return View("Error");
                }

                // Find the user with the provided user ID
                var user = await _userManager.FindByIdAsync(userId);

                // If no user is found, it means the provided user ID is not valid or the user does not exist,
                // hence, display the error view
                if (user == null)
                {
                    return View("Error");
                }

                // Try to confirm the user's email address using the provided confirmation code
                var result = await _userManager.ConfirmEmailAsync(user, code);

                // If the email confirmation is successful, display the "ConfirmEmail" view
                // Otherwise, display the error view
                return View(result.Succeeded ? "ConfirmEmail" : "Error");
            }
            catch (Exception ex)
            {
                await _errorService.LogErrorAsync(ex);
                return RedirectToAction("Error", "Account");
            }
        }

        [HttpGet]
        public IActionResult Login(string? returnurl = "")
        {
            // when a user tries to access a protected link there would 
            // be a returnurl params added to the link, and the user would
            // be directed to our login page. so after login we want the user 
            // to be directed back the the page the user tried to access.

            // this link would be stored to the returnurl.

            ViewData["ReturnUrl"] = returnurl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnurl = "")
        {
            ViewData["ReturnUrl"] = returnurl;

            // checks the data submitted in the form complies with all the validation rules
            // deined in the model
            if (ModelState.IsValid)
            {
                // signs the user into the application 
                // we are going to have a remeberMe bool flag in our applicaition
                // if true and cookie has not expired we would log the user in the next time he want to user our 
                // application without entering his credentials 
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    // Check if the returnUrl is local. If not, redirect to the default page.
                    if (Url.IsLocalUrl(returnurl))
                    {
                        return LocalRedirect(returnurl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Item");
                    }
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(VerifyAuthenticatorCode), new { returnurl, model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    return View("Locked");
                }
                else
                {
                    // displan an errror message when login is not succesfull
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user details by the email in the model 
                var user = await _userManager.FindByEmailAsync(model.Email);

                // If no user is found with the given email, redirect to the confirmation page
                if (user == null)
                {
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                // Generate a password reset token for the user
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Generate the callback URL for resetting the password
                var callbackurl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                // Send the password reset link via email to the user
                _emailService.SendEmail(model.Email, "Reset Password - Identity Manager",
                    "Please reset your password by clicking here: <a href=\"" + callbackurl + "\">link</a>");

                // Redirect to the confirmation page
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string? code = null)
        {
            // when reseting password a code is expected 
            // so if code is null, we return an error page 
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user details by the email in the model 
                var user = await _userManager.FindByEmailAsync(model.Email);

                // If no user is found with the given email, redirect to the confirmation page
                if (user == null)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                // Invoke the ResetPasswordAsync method of the UserManager service
                // This method attempts to reset the password for the specified user
                // It uses the provided password reset token and the new password
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

                // If the password reset operation is successful
                if (result.Succeeded)
                {
                    // redirect the user to the "ResetPasswordConfirmation" action
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                // add errors once the reset is not successful 
                AddErrors(result);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string? returnurl = null)
        {
            // generates the URL that the external login provider will redirect the user to after authentication 
            var redirecturl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnurl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirecturl);

            // triggers the external authentication process
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnurl = null, string? remoteError = null)
        {
            // If there was an error during the external authentication process, add it to the ModelState and return the Login view
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }

            // Retrieve information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();

            // If no information could be retrieved, redirect to the login page
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Try to sign in the user with the external login info. If the user has logged in with this provider before, this will succeed
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                // Update authentication tokens if login succeeded 
                await _signInManager.UpdateExternalAuthenticationTokensAsync(info);

                // Check if the returnUrl is local. If not, redirect to the default page.
                if (Url.IsLocalUrl(returnurl))
                {
                    return LocalRedirect(returnurl);
                }
                else
                {
                    return RedirectToAction("Index", "Item");
                }
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction("VerifyAuthenticatorCode", new { returnurl = returnurl });
            }
            else
            {
                // If the user has not logged in with this provider before, get the user's email and name from the login info 
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);

                // Store the return URL and provider display name in ViewData to be used in the view
                ViewData["ReturnUrl"] = returnurl;
                ViewData["ProviderDisplayName"] = info.ProviderDisplayName;


                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email, Name = name });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string? returnurl = "")
        {

            if (ModelState.IsValid)
            {
                // get the info about the user from external login provider 
                var info = await _signInManager.GetExternalLoginInfoAsync();

                if (info == null)
                {
                    return View("Error");
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        // Update authentication tokens if login succeeded 
                        await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                        // Check if the returnUrl is local. If not, redirect to the default page.
                        if (Url.IsLocalUrl(returnurl))
                        {
                            return LocalRedirect(returnurl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Item");
                        }
                    }
                }
                AddErrors(result);
            }
            ViewData["ReturnUrl"] = returnurl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Item");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RemoveAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            return RedirectToAction(nameof(Index), "Item");
        }

        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            // {0} - issuer identifier, {1} - email address, {2} - Secret key 
            string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

            var user = await _userManager.GetUserAsync(User);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            var token = await _userManager.GetAuthenticatorKeyAsync(user);
            string AuthenticatorUri = string.Format(AuthenticatorUriFormat, _urlEncoder.Encode("IdentityManager"),
                _urlEncoder.Encode(user.Email), token);
            var model = new TwoFactorAuthenticationViewModel() { Token = token, QRCodeUrl = AuthenticatorUri };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EnableAuthenticator(TwoFactorAuthenticationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var succeeded = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, model.Code);
                if (succeeded)
                {
                    await _userManager.SetTwoFactorEnabledAsync(user, true);
                }
                else
                {
                    ModelState.AddModelError("Verify", "Your two factor auth code could not be avalidated.");
                    return View(model);
                }

            }
            return RedirectToAction(nameof(AuthenticatorConfirmation));
        }

        public IActionResult AuthenticatorConfirmation()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> VerifyAuthenticatorCode(bool rememberMe, string? returnUrl = null)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new VerifyAuthenticatorViewModel { ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyAuthenticatorCode(VerifyAuthenticatorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(model.Code, model.RememberMe, rememberClient: false);

            if (result.Succeeded)
            {
                // Check if the returnUrl is local. If not, redirect to the default page.
                if (Url.IsLocalUrl(model.ReturnUrl))
                {
                    return LocalRedirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Item");
                }
            }
            if (result.IsLockedOut)
            {
                return View("Locked");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Code.");
                return View(model);
            }

        }

        public async Task<IActionResult> EditProfile()
        {
            var user = (ApplicationUser)await _userManager.GetUserAsync(User);
            var model = new EditProfileViewModel
            {
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
                // set other properties as needed
            };

            if (user == null)
            {
                ViewData["TwoFactorEnabled"] = false;
            }
            else
            {
                ViewData["TwoFactorEnabled"] = user.TwoFactorEnabled;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = (ApplicationUser)await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    user.Name = model.Name;
                    user.PhoneNumber = model.PhoneNumber;

                    // set other properties as needed
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Item"); // or redirect to another page
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }


        // helper functions that add the errors returned
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
