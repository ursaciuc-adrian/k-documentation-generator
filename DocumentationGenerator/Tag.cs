using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DocumentationGenerator
{
    public class Tag
    {
        public string Name { get; set; }
        public bool ProcessValue { get; set; }

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

            var ruleType2 = new Regex(@"(?<from>[a-zA-Z]*)(\ )*\:(\ )*([a-zA-Z])*(\ )*\=\>(\ )*(?<to>[a-zA-Z]*)");
            var matchRuleType2 = ruleType2.Match(value);

            if (matchRuleType1.Success)
            {
                return matchRuleType1.Groups["from"] + " -> " + matchRuleType1.Groups["to"];
            }

            if (matchRuleType2.Success)
            {
                return matchRuleType2.Groups["from"] + " -> " + matchRuleType2.Groups["to"];
            }

            return "Invalid";
        }
    }
}