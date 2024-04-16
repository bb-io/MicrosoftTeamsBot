﻿using Apps.MicrosoftTeamsBot.Authorization.OAuth2;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.MicrosoftTeamsBot
{
    public class MSTeamsApplication : BaseInvocable, IApplication, ICategoryProvider
    {
        private readonly Dictionary<Type, object> _typesInstances;

        public IEnumerable<ApplicationCategory> Categories
        {
            get => [ApplicationCategory.Microsoft365Apps];
            set { }
        }
        
        public MSTeamsApplication(InvocationContext invocationContext) : base(invocationContext)
        {
            _typesInstances = CreateTypesInstances();
        }

        public string Name
        {
            get => "Microsoft Teams Bot";
            set { }
        }

        public T GetInstance<T>()
        {
            if (!_typesInstances.TryGetValue(typeof(T), out var value))
            {
                throw new InvalidOperationException($"Instance of type '{typeof(T)}' not found");
            }
            return (T)value;
        }

        private Dictionary<Type, object> CreateTypesInstances()
        {
            return new Dictionary<Type, object>
            {
                { typeof(IOAuth2AuthorizeService), new OAuth2AuthorizeService(InvocationContext) },
                { typeof(IOAuth2TokenService), new OAuth2TokenService(InvocationContext) }
            };
        }
    }
}
