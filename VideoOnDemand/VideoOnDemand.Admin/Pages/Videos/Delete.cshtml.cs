﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using VideoOnDemand.Data.Data.Entities;
using VideoOnDemand.Data.Services;

namespace VideoOnDemand.Admin.Pages.Videos
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private IDbReadService _dbReadService;
        private IDbWriteService _dbWriteService;

        [BindProperty]
        public Video Input { get; set; } = new Video();

        [TempData]
        public string StatusMessage { get; set; }

        public DeleteModel(IDbReadService dbReadService, IDbWriteService dbWriteService)
        {
            _dbReadService = dbReadService;
            _dbWriteService = dbWriteService;
        }

        public void OnGet(int id)
        {
            Input = _dbReadService.Get<Video>(id, true);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var success = await _dbWriteService.Delete(Input);
                if (success)
                {
                    StatusMessage = $"Deleted Video: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }

            // Something failed, redisplay the form.
            return Page();
        }
    }
}