using ScryfallApi.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static ScryfallApi.Client.Models.SearchOptions;

namespace ScryfallApi.Client.Apis
{
    ///<inheritdoc cref="ICards"/>
    public class Cards : ICards
    {
        private readonly BaseRestService _restService;

        internal Cards(BaseRestService restService)
        {
            _restService = restService;
        }

        public Task<ResultList<Card>> Get(int page) => _restService.GetAsync<ResultList<Card>>($"/cards?page={page}");

        public Task<Card> GetRandom() => _restService.GetAsync<Card>($"/cards/random", false);

        public Task<Card> GetById(string id) => _restService.GetAsync<Card>($"/cards/{id}");

        public Task<ResultList<Card>> Collection(IEnumerable<string> ids) {
            var request = new {
                identifiers = ids.Select(x => new { id = x }).ToArray()
            };

            return _restService.PostAsync<ResultList<Card>>($"/cards/collection", request);
        }

        public Task<ResultList<Card>> Search(string query, int page, CardSort sort) =>
            Search(query, page, new SearchOptions { Sort = sort });

        public Task<ResultList<Card>> Search(string query, int page, SearchOptions options = default)
        {
            if (page < 1) page = 1;

            query = WebUtility.UrlEncode(query);
            return _restService.GetAsync<ResultList<Card>>($"/cards/search?q={query}&page={page}&{options.BuildQueryString()}");
        }
    }
}