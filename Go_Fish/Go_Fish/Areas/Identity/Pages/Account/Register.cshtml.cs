// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Web;
using Newtonsoft.Json;
using Serilog;
using System.Text.RegularExpressions;
using GoFishData.Entities;

namespace GoFishAreas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IUserEmailStore<AppUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // Don't allow people to register by default 
            return Page();

            var reCaptchaToken = Request.Form["YourTokenFieldName"];

            // Validate the ReCaptcha token
            var reCaptchaValid = await ValidateReCaptchaAsync(reCaptchaToken);

            if (reCaptchaValid.Success == false)
            {
#if DEBUG
                // Write all the errors to the validation errors
                reCaptchaValid.ErrorCodes.ForEach(c => ModelState.AddModelError("", c));
#endif
                ModelState.AddModelError("", "ReCaptcha validation failed.");
                return Page();
            }

            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var referredBy = string.Empty;
                // Keep track of who referred the business to the site
                if (Request.Cookies.ContainsKey("ReferredBy"))
                    referredBy = Request.Cookies["ReferredBy"].Trim();

                if (IsIpBlocked(HttpContext.Connection.RemoteIpAddress?.ToString()))
                {
                    await _emailSender.SendEmailAsync("support@fusiontree.co.za", "User Blocked", $"<p>Someone tried to register with an email address which we don't support. This is just a notification to check that we're not blocking someone who should be allowed.</p><p>Email used was: <b>{Input.Email}</b></p>");
                    ModelState.AddModelError("Email", $"This message serves as a notice regarding your violation of our terms and conditions.{Environment.NewLine}If you think this is an error, please support for help");
                    return Page();
                }

                // Validate the Email address some more
                if (await IsValidEmail(Input.Email) == false)
                {
                    await _emailSender.SendEmailAsync("support@fusiontree.co.za", "Email Blocked", $"<p>Someone tried to register with an email address which we don't support. This is just a notification to check that we're not blocking someone who should be allowed.</p><p>Email used was: <b>{Input.Email}</b></p>");
                    ModelState.AddModelError("Email", "Oops, it looks like the email address you entered is not supported. If you think this is an error, please contact us for help");
                    return Page();
                }


                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private AppUser CreateUser()
        {
            try
            {
                var user = Activator.CreateInstance<AppUser>();
                //user.CreatedOn = DateTime.Now;
                return user;
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<AppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AppUser>)_userStore;
        }

        private async Task<CaptchaResponse> ValidateReCaptchaAsync(string token)
        {
            // Perform ReCaptcha validation here
            // You'll need to make a POST request to Google's ReCaptcha verification endpoint
            // Example code for making HTTP request:

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"secret", "6Lfr18kpAAAAAA_gTqz80TXJBSiE_RpwqM9qYSNJ"},
                    {"response", token}
                }));

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(responseBody);
                    Log.Information(responseBody);
                    return captchaResponse;
                }

                return new CaptchaResponse() { Success = false };
            }
        }
        public class CaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            public string ChallengeTs { get; set; }

            public string Hostname { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }

        private bool IsIpBlocked(string ip)
        {
            var list = GetListOfBlockedIPs();

            if (list.Contains(ip))
                return true;

            return false;
        }

        private async Task<bool> IsValidEmail(string email)
        {

            var listOfSpamDomains = GetListOfEmailDomains();


            var result = Regex.IsMatch(email, @"[a-zA-Z0-9._%+-]{18}@[a-zA-Z0-9.-]{5}\.[a-zA-Z]{2,}");

            foreach (var item in listOfSpamDomains)
            {
                if (email.EndsWith(item, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            // Lets make a request to see if it's a spam email account 10Minute emails etc...
            if (result)
            {
                // Extract domain name from email address using regular expression
                string domain = Regex.Match(email, @"(?<=@)[^.]+\.[^.\s]+").Value;

                // Create HttpClient to send request
                HttpClient client = new HttpClient();
                HttpResponseMessage response = null;

                try
                {
                    response = await client.GetAsync("http://" + domain);
                    Console.WriteLine("Response message: " + response.StatusCode);
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        return false;

                    foreach (var item in listOfSpamDomains)
                    {
                        if (content.Contains(item))
                            return false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    response?.Dispose();
                    client?.Dispose();
                }

            }


            // Next if we did not exclude them from the file then try to use the mail validation api.
            if (CheckQuickEmailValidation(email) == false)
                return false;

            //Count the number of periods in the email address
            //Usually spam emails have 6 periods.

            var splitEmail = email.Split('@');

            int cnt = splitEmail[0].Count(c => c == '.');

            if (cnt >= 3)
                return false;

            // If we get here it was valid email address
            return true;
        }

        private bool CheckQuickEmailValidation(string emailaddress)
        {
            var result = QuickEmailVerification.ValidateEmailAddressAsync(emailaddress).Result;

            if (result.disposable.Equals("false", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private List<string> GetListOfEmailDomains()
        {
            var list = new List<string>() { "kbvehicle.com", "couldmail.com", "fatamail.com", "iemail.one", "10minutemail.com", "@gufum.com", "@omeie.com", "@aklqo.com", "@cdfaq.com", "@eanok.com", "@eveav.com", "@evonb.com", "@gosne.com", "@idxue.com", "@iencm.com", "@ixaks.com", "@jeoce.com", "@jiooq.com", "@lcdvd.com", "@mzico.com", "@nbzmr.com", "@nezid.com", "@oqiwq.com", "@qaioz.com", "@suoox.com", "@tuofs.com", "@xcoxc.com", "@zcrcd.com", "@zslsz.com", "@zzrgg.com" };

            if (System.IO.File.Exists("BlockedEmailDomains.txt"))
            {
                var content = System.IO.File.ReadAllText("BlockedEmailDomains.txt");
                list.AddRange(content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.TrimEntries));
            }
            else
            {
                Log.Information($"Could not find file: {System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "BlockedEmailDomains.txt")}");
            }

            return list;
        }

        private List<string> GetListOfBlockedIPs()
        {
            var list = new List<string>();

            if (System.IO.File.Exists("BlockedIPs.txt"))
            {
                var content = System.IO.File.ReadAllText("BlockedIPs.txt");
                list.AddRange(content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.TrimEntries));
            }
            else
            {
                using (System.IO.File.CreateText("BlockedIPs.txt"))
                    Log.Information($"Could not find file: {System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "BlockedIPs.txt")}");
            }

            return list;
        }
        class QuickEmailVerification
        {
            private const string QuickVerificationEmailApiKey = "Ae3a6a58b6f3fd051aebdf06a4e376b7703ac08180fda144320532a49bf10"; // Replace API_KEY with your API Key

            public static async Task<EmailVerificationModel> ValidateEmailAddressAsync(string emailAddress)
            {
                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string apiURL = $"https://api.quickemailverification.com/v1/verify?email={HttpUtility.UrlEncode(emailAddress)}&apikey={QuickVerificationEmailApiKey}";
                        return await httpClient.GetFromJsonAsync<EmailVerificationModel>(apiURL);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        [Serializable]
        public class EmailVerificationModel
        {
            public string result { get; set; }
            public string reason { get; set; }
            public string disposable { get; set; }
            public string accept_all { get; set; }
            public string role { get; set; }
            public string free { get; set; }
            public string email { get; set; }
            public string user { get; set; }
            public string domain { get; set; }
            public string mx_record { get; set; }
            public string mx_domain { get; set; }
            public string safe_to_send { get; set; }
            public string did_you_mean { get; set; }
            public string success { get; set; }
            public object message { get; set; }

            public EmailVerificationModel()
            {

            }
        }

    }
}
