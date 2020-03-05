using HtmlAgilityPack;
using RevelHomeTaskApp.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RevelHomeTaskApp.Service
{
    public class TagInsightsService : ITagInsightsService
    {
        public IEnumerable<HtmlNode> tags { get; set; }
        public HtmlDocument content { get; set; }
        public ICollection<Path> tagPaths { get; set; }

        public TagInsightsService()
        {
            tags = new List<HtmlNode>();
            content = new HtmlDocument();
            tagPaths = new List<Path>();
        }

        public IEnumerable<IGrouping<string, HtmlNode>> GetTags(string url)
        {
            content = WebPageHelper.LoadPage(url);

            tags = content.DocumentNode.Descendants().ToList();

            var groupedTags = tags
                .Where(x => x.NodeType == HtmlNodeType.Element)
                .GroupBy(x => x.Name);

            if (groupedTags.Count() > 0)
            {
                Console.WriteLine(" Unique tags found:");

                foreach (var tag in groupedTags)
                {
                    Console.WriteLine($" Name: {tag.Key} with {tag.Count()} occurrences.");
                }

                Console.WriteLine("  ");
            }

            return groupedTags;
        }

        public KeyValuePair<string, int> FindMostPopularTag(
            IEnumerable<IGrouping<string, HtmlNode>> tags)
        {
            var tagCounts = new Dictionary<string, int>();
            foreach (var node in tags)
            {
                tagCounts.Add(node.Key, node.Count());
            }


            var result = tagCounts.OrderByDescending(x => x.Value).FirstOrDefault();

            Console.WriteLine($" Most popular tag: {result.Key}. Tag was found {result.Value} times");
            return result;
        }

        public IEnumerable<Path> FindPaths(
            IEnumerable<IGrouping<string, HtmlNode>> tags,
            KeyValuePair<string, int> maxTag)
        {
            var paths = tags
                .Where(x => x.Key == maxTag.Key)
                .FirstOrDefault()
                .ToList();

            foreach (var item in paths)
            {
                var extract = Regex.Matches(item.XPath.ToLowerInvariant(), "(" + maxTag.Key + "\\[[0-9]+)\\]");
                int count = 0;
                var extractString = string.Empty;

                foreach (var i in extract.ToList())
                {
                    extractString += i.Value.ToString();
                }

                int levels = item.XPath.Count(x => x == '/');
                var resultString = Regex.Matches(extractString, @"\d+");

                foreach (var s in resultString.ToList())
                {
                    count += int.Parse(s.Value);
                }

                tagPaths.Add(new Path
                {
                    LevelCount = levels,
                    TagOccurrences = count,
                    XPath = item.XPath
                });
            }

            return tagPaths;
        }

        public void FindLongestPath(
            IEnumerable<IGrouping<string, HtmlNode>> tags,
            KeyValuePair<string, int> maxTag)
        {
            var paths = FindPaths(tags, maxTag);

            var result = paths
                .OrderByDescending(x => x.TagOccurrences)
                .ThenByDescending(x => x.LevelCount)
                .FirstOrDefault();

            Console.WriteLine(" ");

            Console.WriteLine($" The longest path is { result.LevelCount} levels deep. \n " +
                $" Path has {result.TagOccurrences} occurences of {maxTag.Key} \n " +
                $" XPath - {result.XPath}");
        }
    }
}