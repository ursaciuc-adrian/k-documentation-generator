using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using DocumentationGenerator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DocumentationGenerator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProcessorService _processorService;

        [Required]
	    [Display(Name = "File")]
	    [BindProperty]
	    public IFormFile UploadFile { get; set; }

        public IndexModel(IProcessorService processorService)
        {
            _processorService = processorService;
        }

	    public async Task<IActionResult> OnPostAsync()
	    {
		    using (var reader = new StreamReader(UploadFile.OpenReadStream()))
		    {
                _processorService.SetData(await reader.ReadToEndAsync());  
		    }

		    return RedirectToPage("Documentation");
	    }
    }
}