using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DocumentationGenerator
{
	public class Section
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Content { get; set; }

		public void RemoveAttributes()
		{
			var regex = new Regex(@"\/\/\/.*\:.*");

            Content = Content.Replace("\r", "");

			var result = Content.Split('\n', StringSplitOptions.RemoveEmptyEntries).Where(x => !regex.IsMatch(x));

			Content = string.Join("\n", result);
		}
	}
}