using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DocumentationGenerator.Pages
{
    public class IndexModel : PageModel
    {
	    [Required]
	    [Display(Name = "File")]
	    [BindProperty]
	    public IFormFile UploadFile { get; set; }
	    

	    public async Task<IActionResult> OnPostAsync()
	    {
		    string result;
		    using (var reader = new StreamReader(UploadFile.OpenReadStream()))
		    {
			    result = await reader.ReadToEndAsync();  
		    }

		    return RedirectToPage("Documentation", new { fileContent = result });
	    }
    }
}