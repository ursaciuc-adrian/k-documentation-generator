using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DocumentationGenerator
{
    public class Processor
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public Tag Configuration { get; set; }

        public void ProcessData(string content)
        {
            content = content.Replace("\r", "");
            Sections = GetSections(content);
            Title = GetAttribute(content, "title");
            Description = GetAttribute(content, "description");
            Configuration = ProcessConfiguration();
        }

        public Tag ProcessConfiguration()
        {
            var configSection = Sections.FirstOrDefault(x => x.Name.ToLower() == "configuration");
            if (configSection == null)
            {
                return null;
            }

            var tag = TagHelper.GetTagsInheritance(configSection.Content);
            tag.Name = "T";

            return tag;
        }

        private string GetAttribute(string content, string attribute)
        {
            attribute = $"//{attribute}:";

            var line = content.Split('\n')
                .FirstOrDefault(x => x.Contains(attribute));

            if (line == null)
            {
                return null;
            }

            var index = line.IndexOf(attribute, StringComparison.Ordinal) + attribute.Length;
            return line.Substring(index, line.Length - index);
        }

        private IEnumerable<Section> GetSections(string content)
        {
            var lines = content.Split('\n').ToList();

            var startSectionRegex = new Regex(@"\/\/<[^\/]+>");

            foreach (var line in lines)
            {
                if (startSectionRegex.IsMatch(line))
                {
                    string tag = line.Trim().Substring(3, line.Trim().Length - 4);

                    if (tag != null)
                    {
                        var section = new Section
                        {
                            Name = tag,
                            Content = TagHelper.GetContentBetweenTags(content, tag, true)
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