using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DocumentationGenerator
{
    public static class TagHelper
    {
        public static string GetContentBetweenTags(string content, string tag, bool commented)
        {
            Regex regex;

            if (commented)
            {
                regex = new Regex($@"\/\/(\<{tag}[ a-zA-Z0-9\=\""]*\>)(?<content>(.|\n)*?)(\/\/\<\/{tag}\>)");
            }
            else
            {
                regex = new Regex($@"(\<{tag}[ a-zA-Z0-9\=\""]*\>)(?<content>(.|\n)*?)(\<\/{tag}\>)");
            }

            var match = regex.Match(content);

            if (match.Success)
            {
                return match.Groups["content"].Value.Trim();
            }

            var sectionRegex = new Regex(@"\((.|\n)*\.(?<name>[a-zA-Z]*)(.|\n)*\=\>(?<content>(.|\n)*)\)");
            match = sectionRegex.Match(content);

            if (match.Success)
            {
                return match.Groups["content"].Value.Trim();
            }

            return null;
        }

        public static IEnumerable<string> GetTags(string content)
        {
            content = content.Trim();
            content = content.Replace("\r", "");

            if (string.IsNullOrEmpty(content))
            {
                return new HashSet<string>();
            }

            var tags = new HashSet<string>();

            var sectionRegex = new Regex(@"\((\ |\n)*\.(?<name>[a-zA-Z]*)(.|\n)*\=\>(?<content>(\ |\n)*\<([^\/]+)(\ .*)?\>((.|\n)*?)\<\/\6\>)\)");
            var sectionMatches = sectionRegex.Matches(content).ToList();

            foreach (var match in sectionMatches)
            {
                tags.Add(match.Groups["name"].Value);
            }
            content = Regex.Replace(content, sectionRegex.ToString(), "");


            var tagsRegex = new Regex(@"<([^\/]+)(\ .*)?>((.|\n)*?)<\/\1>");

            foreach (var match in tagsRegex.Matches(content).ToList())
            {
                tags.Add(match.Groups[1].Value);
            }

            return tags;
        }

        public static bool IsSection(string content, string tag)
        {
            return new Regex($@"\((.|\n)*\.{tag}(.|\n)*\=\>(?<content>(.|\n)*)\)").IsMatch(content);
        }

        public static Tag GetTagsInheritance(string content, bool isRule = false)
        {
            var tagInheritance = new Tag
            {
                Content = content,
                ProcessValue = isRule
            };

            var tags = GetTags(content).ToList();

            foreach (var tag in tags)
            {
                var tagContent = GetContentBetweenTags(content, tag, false);
                if (GetTags(tagContent).Count() != 0)
                {
                    var newTag = GetTagsInheritance(tagContent, isRule);
                    newTag.Name = tag;
                    newTag.IsSection = IsSection(content, tag);

                    tagInheritance.Tags.Add(newTag);
                }
                else
                {
                    tagInheritance.Pairs.Add(tag, tagContent);
                }
            }

            return tagInheritance;
        }
    }
}
