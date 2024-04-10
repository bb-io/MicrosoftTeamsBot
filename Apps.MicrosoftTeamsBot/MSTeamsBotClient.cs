using Blackbird.Applications.Sdk.Utils.RestSharp;
using RestSharp;

namespace Apps.MicrosoftTeamsBot
{
    public class MSTeamsBotClient : BlackBirdRestClient
    {
        public MSTeamsBotClient(string botServiceEndpoint) : base(new RestClientOptions() { BaseUrl = new Uri(botServiceEndpoint) })
        {
        }

        protected override Exception ConfigureErrorException(RestResponse response)
        {
            return new ArgumentException(response.Content);
        }
    }
}
