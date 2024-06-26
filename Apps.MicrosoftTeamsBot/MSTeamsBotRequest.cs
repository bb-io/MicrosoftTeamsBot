﻿using Apps.MicrosoftTeamsBot.Dtos;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.MicrosoftTeamsBot
{
    public class MSTeamsBotRequest : BlackBirdRestRequest
    {
        public MSTeamsBotRequest(string resource, Method method, IEnumerable<AuthenticationCredentialsProvider> creds) : base(resource, method, creds)
        {
        }

        protected override void AddAuth(IEnumerable<AuthenticationCredentialsProvider> creds)
        {
            var client = new RestClient("https://login.microsoftonline.com");
            var request = new RestRequest("/botframework.com/oauth2/v2.0/token", Method.Post);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", ApplicationConstants.BotClientId);
            request.AddParameter("client_secret", ApplicationConstants.BotClientSecret);
            request.AddParameter("scope", ApplicationConstants.BotScope);
            var response = JsonConvert.DeserializeObject<NotAuthResponse>(client.ExecuteAsync(request).Result.Content);
            this.AddHeader("Authorization", $"Bearer {response.AccessToken}");
        }
    }
}
