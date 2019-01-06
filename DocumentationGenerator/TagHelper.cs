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
                regex = new Regex($@"(?<=\/\/\<{tag}.*\>)((.|\n)*)(?=\/\/\<\/{tag}\>)");
            }
            else
            {
                regex = new Regex($@"(?<=\<{tag}.*\>)((.|\n)*)(?=\<\/{tag}\>)");
            }

            var match = regex.Match(content);

            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            return null;
        }

        public static IEnumerable<string> GetTags(string content)
        {
            content = content.Trim();
            content = content.Replace("\r", "");

            if (string.IsNullOrEmpty(content))
            {
                yield break;
            }

            var tagsRegex = new Regex(@"<([^\/]+)(\ .*)?>((.|\n)*)<\/\1>");

            foreach (var match in tagsRegex.Matches(content).ToList())
            {
                yield return match.Groups[1].Value;
            }
        }

        public static Tag GetTagsInheritance(string content)
        {
            var tagInheritance = new Tag();

            var tags = GetTags(content).ToList();

            foreach (var tag in tags)
            {
                var tagContent = GetContentBetweenTags(content, tag, false);
                if (GetTags(tagContent).Count() != 0)
                {
                    var newTag = GetTagsInheritance(tagContent);
                    newTag.Name = tag;

                    tagInheritance.Tags.Add(newTag);
                }
                else
                {
                    tagInheritance.Pairs.Add(tag, tagContent);
                }

                content = RemoveTag(content, tag);
            }

            return tagInheritance;
        }

        private static string RemoveTag(string content, string tag)
        {
            return Regex.Replace(content, $@"(?<=\<{tag}.*\>)((.|\n)*)(?=\<\/{tag}\>)", "");
        }
    }
}
