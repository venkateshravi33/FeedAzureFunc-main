using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using System.Web.Http;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Otto.Feed.FeedAzureFunc.Core.Services;
using Otto.Feed.FeedAzureFunc.Core.Interfaces;
using Otto.Feed.FeedAzureFunc.Models.DTOs;
using Otto.Feed.FeedAzureFunc.API.Validations;
using Otto.Feed.FeedAzureFunc.Models.Models;

namespace Otto.Feed.FeedAzureFunc.API
{
    public class GetFeed
    {
        private readonly IFeedService _feedService;
        public GetFeed(IFeedService feedService)
        {
            _feedService = feedService;
        }

        [FunctionName("GetFeed")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/feeds/GetFeed/{user_id}")] HttpRequest req,
            long user_id,
            ILogger log
            )
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var responseMessage = await _feedService.GetFeedAsync(user_id);
                if (responseMessage.ToList().Count == 0)
                {
                    return new NotFoundObjectResult(new ErrorDetails()
                    {
                        StatusCode = 404,
                        ErrorMessage = "Record not found"
                    });
                }

                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ErrorDetails()
                {
                    StatusCode = 500,
                    ErrorMessage = ex.Message
                })
                {
                    StatusCode = 500
                };
            }
        }
    }
}
