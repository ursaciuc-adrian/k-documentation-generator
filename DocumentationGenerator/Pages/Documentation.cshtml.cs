using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DocumentationGenerator.Pages
{
	public class DocumentationModel : PageModel
	{
		public Processor Processor { get; set; }
		
		public void OnGet(string fileContent)
		{
			Processor = new Processor(fileContent);
		}
	}
}