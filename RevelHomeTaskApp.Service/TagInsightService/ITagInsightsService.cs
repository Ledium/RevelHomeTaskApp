using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace RevelHomeTaskApp.Service
{
    public interface ITagInsightsService
    {
        void FindLongestPath(
            IEnumerable<IGrouping<string, HtmlNode>> tags,
            KeyValuePair<string, int> maxTag);

        KeyValuePair<string, int> FindMostPopularTag(
            IEnumerable<IGrouping<string,
            HtmlNode>> tags);

        IEnumerable<Path> FindPaths(
            IEnumerable<IGrouping<string, HtmlNode>> tags,
            KeyValuePair<string, int> maxTag);

        IEnumerable<IGrouping<string, HtmlNode>> GetTags(string url);
    }
}