using Blackbird.Applications.Sdk.Utils.RestSharp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.MicrosoftTeamsBot
{
    public class MSTeamsBotClient : BlackBirdRestClient
    {
        public MSTeamsBotClient(string botServiceEndpoint) : base(new RestClientOptions() { BaseUrl = new Uri(botServiceEndpoint) })
        {
        }

        protected override Exception ConfigureErrorException(RestResponse response)
        {
            return new ArgumentException(response.ErrorMessage);
        }
    }
}
