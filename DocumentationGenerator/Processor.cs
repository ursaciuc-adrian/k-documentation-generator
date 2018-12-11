using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DocumentationGenerator
{
	public class Processor
	{
		public string Title { get; }
		public string Description { get; }
		public IEnumerable<Section> Sections { get; }

		public Processor(string content)
		{
			Sections = GetSections(content);
			Title = GetAttribute(content, "title");
			Description = GetAttribute(content, "description");
		}

		private string GetAttribute(string content, string attribute)
		{
			attribute = $"//{attribute}:";

			var line = content.Split('\n', StringSplitOptions.RemoveEmptyEntries)
				.FirstOrDefault(x => x.Contains(attribute));

			if (line == null)
			{
				return null;
			}
			
			var index = line.IndexOf(attribute, StringComparison.Ordinal) + attribute.Length;
			return line.Substring(index, line.Length - index);
		}
		
		private string GetContentBetweenTags(string value, string tag)
		{
			var startTag = $"//<{tag}>";
			var endTag = $"//</{tag}>";

			if (!value.Contains(startTag) || !value.Contains(endTag))
			{
				return null;
			}
			
			var index = value.IndexOf(startTag, StringComparison.Ordinal) + startTag.Length;
			
			return value.Substring(index, value.IndexOf(endTag, StringComparison.Ordinal) - index);
		}
		
		private IEnumerable<Section> GetSections(string content)
		{
			var lines = content.Split('\n').ToList();

			var startSectionRegex = new Regex(@"\/\/<[^\/]+>");

			foreach (var line in lines)
			{
				if (startSectionRegex.IsMatch(line))
				{
					string tag = line.Substring(3, line.Length - 4);
					
					if (tag != null)
					{
						var section = new Section
						{
							Name = tag,
							Content = GetContentBetweenTags(content, tag)
						};
						
						section.Description = GetAttribute(section.Content, "description");
						section.RemoveAttributes();
						
						yield return section;
					}
				}
			}
		}
	}
}