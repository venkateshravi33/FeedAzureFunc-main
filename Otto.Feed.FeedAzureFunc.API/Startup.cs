using System;
using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Otto.Feed.FeedAzureFunc.Repository.Context;
using Otto.Feed.FeedAzureFunc.Repository.Interfaces;
using Otto.Feed.FeedAzureFunc.Repository.Repositories;
using Otto.Feed.FeedAzureFunc.API.Mappers;
using Otto.Feed.FeedAzureFunc.Core.Interfaces;
using Otto.Feed.FeedAzureFunc.Core.Services;

[assembly: FunctionsStartup(typeof(Otto.Todo.TodoAzureFunc.API.Startup))]

namespace Otto.Todo.TodoAzureFunc.API
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddHttpClient();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FeedProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddSingleton<DapperContext>();
            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            builder.Services.AddScoped<IFeedService, FeedCoreService>();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        }
    }
}