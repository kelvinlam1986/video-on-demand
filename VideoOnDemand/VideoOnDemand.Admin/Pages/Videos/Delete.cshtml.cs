﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using VideoOnDemand.Admin.Models;
using VideoOnDemand.Admin.Services;

namespace VideoOnDemand.Admin.Pages.Videos
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private IUserService _userService;

        [BindProperty]
        public UserPageModel Input { get; set; } = new UserPageModel();

        [TempData]
        public string StatusMessage { get; set; }

        public DeleteModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet(string userId)
        {
            StatusMessage = string.Empty;
            Input = _userService.GetUser(userId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.DeleteUser(Input.Id);
                if (result)
                {
                    StatusMessage = $"User {Input.Email} was deleted.";
                    return RedirectToPage("Index");
                }
            }

            // Something failed, redisplay the form.
            return Page();
        }
    }
}