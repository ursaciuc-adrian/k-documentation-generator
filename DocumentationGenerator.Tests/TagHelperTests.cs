using System.Collections.Generic;
using Xunit;
using Shouldly;

namespace DocumentationGenerator.Tests
{
    public class TagHelperTests
    {
        [Fact]
        public void GetTags_ShouldReturnEmptyArray_WhenThereAreNoTags()
        {
            var result = TagHelper.GetTags("");

            result.ShouldBe(new string[] { });
        }

        [Fact]
        public void GetTags_ShouldReturnArrayWithATag_WhenThereIsASingleTag()
        {
            var result = TagHelper.GetTags("  <tag> content  </tag> ");

            result.ShouldBe(new [] { "tag" });
        }

        [Fact]
        public void GetTags_ShouldReturnCorrectTag_WhenATagHasAttributes()
        {
            var result = TagHelper.GetTags("  <tag attr=\"data\"> content  </tag> ");

            result.ShouldBe(new[] { "tag" });
        }

        [Fact]
        public void GetTags_ShouldReturnArrayWithAllTags_WhenThereIsAMultipleValidTag()
        {
            var result = TagHelper.GetTags("  <tag> content  </tag> <tag2> content2 </tag2>");

            result.ShouldBe(new[] { "tag", "tag2" });
        }

        [Fact]
        public void GetTags_ShouldReturnOnlyBaseTags_WhenThereNestedTags()
        {
            var result = TagHelper.GetTags("  <tag>  <tag2> content2 </tag2> </tag>");

            result.ShouldBe(new[] { "tag" });
        }


        [Fact]
        public void GetContentBetweenTags_ShouldReturnNull_WhenContentIsEmpty()
        {
            var result = TagHelper.GetContentBetweenTags("", "tag", false);

            result.ShouldBeNull();
        }

        [Fact]
        public void GetContentBetweenTags_ShouldReturnNull_WhenStartTagIsMissing()
        {
            var result = TagHelper.GetContentBetweenTags("content </tag>", "tag", false);

            result.ShouldBeNull();
        }

        [Fact]
        public void GetContentBetweenTags_ShouldReturnNull_WhenEndTagIsMissing()
        {
            var result = TagHelper.GetContentBetweenTags("  <tag> content </eee>", "tag", false);

            result.ShouldBeNull();
        }

        [Fact]
        public void GetContentBetweenTags_ShouldReturnNull_WhenBothTagsAreMissing()
        {
            var result = TagHelper.GetContentBetweenTags("  <tag22> content </eee>", "tag", false);

            result.ShouldBeNull();
        }

        [Fact]
        public void GetContentBetweenTags_ShouldReturnCorrectContent_WhenTagsAreValid()
        {
            var result = TagHelper.GetContentBetweenTags("  <tag> content </tag>", "tag", false);

            result.ShouldBe("content");
        }

        [Fact]
        public void GetContentBetweenTags_ShouldReturnCorrectContent_WhenTagsAreValidAndCommented()
        {
            var result = TagHelper.GetContentBetweenTags("  //<tag> content //</tag>", "tag", true);

            result.ShouldBe("content");
        }

        [Fact]
        public void GetContentBetweenTags_ShouldReturnCorrectContent_WhenTagsHasAttributes()
        {
            var result = TagHelper.GetContentBetweenTags("  <tag attr=\"data\"> content </tag>", "tag", false);

            result.ShouldBe("content");
        }

        [Fact]
        public void GetContentBetweenTags_ShouldReturnCorrectContent_WhenContentHasBreaklines()
        {
            var result = TagHelper.GetContentBetweenTags("  <tag attr=\"data\"> content \nline </tag>", "tag", false);

            result.ShouldBe("content \nline");
        }

        [Fact]
        public void GetContentBetweenTags_ShouldReturnContentWithTags_WhenContentHasNestedTags()
        {
            var result = TagHelper.GetContentBetweenTags("  <tag attr=\"data\"> <tag2>content </tag2> </tag>", "tag", false);

            result.ShouldBe("<tag2>content </tag2>");
        }

        [Fact]
        public void GetTagsInheritance_ShouldReturnCorrectValue_WhenContentHasNoNestedTags()
        {
            var result = TagHelper.GetTagsInheritance("  <tag attr=\"data\"> content </tag>");

            var expected = new Tag()
            {
                Pairs = new Dictionary<string, string> { { "tag", "content" } }
            };

            result.Pairs.ShouldBe(expected.Pairs);
        }

        [Fact]
        public void GetTagsInheritance_ShouldReturnCorrectValue_WhenContentHasOneNestedTag()
        {
            var result = TagHelper.GetTagsInheritance("  <tag attr=\"data\"> <tag2>content </tag2> </tag>");

            var expected = new Tag
            {
                Tags = new List<Tag>
                {
                    new Tag
                    {
                        Pairs = new Dictionary<string, string> { { "tag2", "content" } },
                    }
                },
                Pairs = new Dictionary<string, string>()
            };

            result.Pairs.ShouldBe(expected.Pairs);
            result.Tags[0].Pairs.ShouldBe(expected.Tags[0].Pairs);
        }

        [Fact]
        public void GetTagsInheritance_ShouldReturnCorrectValue_WhenContentHasMoreNestedTag()
        {
            var result = TagHelper.GetTagsInheritance(@"
                <threads>
                    <thread>
                        <k> $PGM:Stmt </k>
                        <control>
                            <fstack> .List </fstack>
                        </control>
                        <env> .Map </env>
                        <env2> .Map </env2>

                        <store> .Map </store>
                        <holds> .Map </holds>
                    </thread>
                </threads>
                <env> .Map </env>
                <store> .Map </store>
                <stack> .List </stack>
                <in  stream=""stdin""> .List </in>
                <out  stream=""stdout""> .List </out>");

            var expected = new Tag
            {
                Tags = new List<Tag>
                {
                    new Tag
                    {
                        Name = "threads",
                        Tags = new List<Tag>()
                        {
                            new Tag
                            {
                                Name = "Thread"
                            }
                        },
                        Pairs = new Dictionary<string, string> ()
                    }
                },
                Pairs = new Dictionary<string, string>
                {
                    { "env", ".Map" },
                    { "store", ".Map" },
                    { "stack", ".List" },
                    { "in", ".List" },
                    { "out", ".List" },
                }
            };

            result.Pairs.ShouldBe(expected.Pairs);
        }
    }
}
