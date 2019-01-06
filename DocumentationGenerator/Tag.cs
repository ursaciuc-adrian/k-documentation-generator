using System.Collections.Generic;

namespace DocumentationGenerator
{
    public class Tag
    {
        public string Name { get; set; }
        public Dictionary<string, string> Pairs { get; set; } = new Dictionary<string, string>();

        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}