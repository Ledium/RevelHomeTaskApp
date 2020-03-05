using RevelHomeTaskApp.Service;

namespace RevelHomeTaskApp
{
    public class TagInsightApp
    {
        private readonly ITagInsightsService _insightsService;
        public TagInsightApp(ITagInsightsService insightsService)
        {
            _insightsService = insightsService;
        }

        public void Run(string url)
        {      
            var tagResult = _insightsService.GetTags(url);

            var mostPopulartag = _insightsService.FindMostPopularTag(tagResult);

           _insightsService.FindLongestPath(tagResult, mostPopulartag);
        }
    }
}
