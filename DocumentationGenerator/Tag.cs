using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DocumentationGenerator
{
    public class Tag
    {
        public string Name { get; set; }
        public bool ProcessValue { get; set; }
        public string Content { get; set; }
        public bool IsSection { get; set; }

        public Dictionary<string, string> Pairs { get; set; } = new Dictionary<string, string>();

        public List<Tag> Tags { get; set; } = new List<Tag>();

        public string GetProcessedValue(string value)
        {
            if (!ProcessValue)
            {
                return value;
            }

            var ruleType1 = new Regex(@"(?<from>[a-zA-Z]*)(\ *)?\|\-\>(\ *)?(?<to>[a-zA-Z]*)(\ *)?\:(\ *)?([a-zA-Z])+");
            var matchRuleType1 = ruleType1.Match(value);

            var ruleType2 = new Regex(@"\((?<from>.*)\=\>(?<to>.*)\)");
            var matchRuleType2 = ruleType2.Match(value);

            if (matchRuleType1.Success)
            {
                var to = RemoveTypes(matchRuleType1.Groups["to"].Value.Trim());
                var from = RemoveTypes(matchRuleType1.Groups["from"].Value);

                if (to == ".")
                {
                    to = "T";
                }

                return from + " -> " + to;
            }

            if (matchRuleType2.Success)
            {
                var to = RemoveTypes(matchRuleType2.Groups["to"].Value.Trim());
                var from = RemoveTypes(matchRuleType2.Groups["from"].Value);
                
                if (to == ".")
                {
                    to = "T";
                }

                return  from + " | " + to;
            }

            return value.Replace("...", "");
        }

        public string RemoveTypes(string content)
        {
            return Regex.Replace(content, "\\:[a-zA-Z]*|\\;", "");
        }

        public bool HasEndDots(string value)
        {
            var rule = new Regex(@".*\.\.\.$");

            return rule.IsMatch(value.Trim());
        }

        public bool HasStartDots(string value)
        {
            var rule = new Regex(@"^\.\.\..*");

            return rule.IsMatch(value.Trim());
        }
    }
}