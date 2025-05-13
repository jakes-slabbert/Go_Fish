// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable



using GoFish.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GoFishAreas.Identity.Pages.Account
{
	public class ForgotPasswordModel : PageModel
	{
		private readonly UserManager<AppUser> _userManager;
		

		public ForgotPasswordModel(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
			
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
		public class InputModel
		{
			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Required]
			[EmailAddress]
			public string Email { get; set; }
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(Input.Email);
				// Don't reveal that the user does not exist
				if (user == null)
					return RedirectToPage("./ForgotPasswordConfirmation");

				var isConfirmed = await _userManager.IsEmailConfirmedAsync(user);
				if (isConfirmed == false)
				{
					return RedirectToPage("./ForgotPasswordConfirmation");
				}

				return RedirectToPage("./ForgotPasswordConfirmation");
			}

			return Page();
		}
	}
}
