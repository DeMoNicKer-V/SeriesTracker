using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using SeriesTracker.Classes;
using SeriesTracker.Classes.Shikimori;

namespace SeriesTracker.Services.ShikimoriBase
{
    internal class ShikimoriBase
    {
        private static readonly string apiUrl = "https://shikimori.one/api/graphql";
        private GraphQLHttpClient graphQLClient;

        public ShikimoriBase()
        {
            graphQLClient = new GraphQLHttpClient(apiUrl, new NewtonsoftJsonSerializer());
        }

        public async Task<GraphQLResponse<AnimeList<ShikimoriAnime>>> GetAnimes(int page)
        {
            return await graphQLClient.SendQueryAsync<AnimeList<ShikimoriAnime>>(GetRequest(page));
        }

        public async Task<GraphQLResponse<AnimeList<ShikimoriAnime>>> GetAnimesByName(int page, string name)
        {
            return await graphQLClient.SendQueryAsync<AnimeList<ShikimoriAnime>>(GetRequest(page, name));
        }

        private GraphQLRequest GetRequest(int page)
        {
            return new GraphQLRequest
            {
                Query = @"query GetAll($page: Int) {
                                animes(page: $page, limit: 5) {
                                    russian
                                    name
                                    description
                                    kind
                                    rating
                                    duration
                                    genres{ id name russian }
                                    episodes
                                    episodesAired
                                    status
                                    score
                                    airedOn {
                                        date
                                    }
                                    poster {
                                        originalUrl
                                    }
                                }
                            }",
                OperationName = "GetAll",
                Variables = new
                {
                    page = page
                }
            };
        }

        private GraphQLRequest GetRequest(int page, string name)
        {
            return new GraphQLRequest
            {
                Query = @"query GetByName($name: String, $page: Int) {
                                animes(search: $name, page: $page, limit: 5) {
                                    russian
                                    name
                                    description
                                    kind
                                    rating
                                    duration
                                    episodes
                                    genres{ id name russian }
                                    episodesAired
                                    status
                                    score
                                    airedOn {
                                        date
                                    }
                                    poster {
                                        originalUrl
                                    }
                                }
                            }",
                OperationName = "GetByName",
                Variables = new
                {
                    name = name,
                    page = page
                }
            };
        }
    }
}