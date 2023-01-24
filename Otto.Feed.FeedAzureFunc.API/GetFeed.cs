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
using System.Xml.Linq;

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
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/feeds/GetFeed")] HttpRequest req,
            ILogger log
            )
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            String user_id = req.Query["user_id"];
            String feed_limit = req.Query["feed_limit"];
            String feed_order = req.Query["feed_order"];
            String start_date = req.Query["start_date"];
            String end_date = req.Query["end_date"];

            ReqFeedDTO data = new ();
            {
                data.user_id = user_id;

                //Using default feed limit 1.
                data.feed_limit = string.IsNullOrEmpty(feed_limit) ? "1" : feed_limit;

                //Using default feed order as Descending.
                data.feed_order = string.IsNullOrEmpty(feed_order) ? "DESC" : feed_order;

                //Using 1st January,2010 as default start date.
                data.start_date = string.IsNullOrEmpty(start_date) ? "2010-01-01" : start_date;

                //Using current date as default end date.
                data.end_date = string.IsNullOrEmpty(end_date) ? $"{DateTime.Now}" : end_date;
            }

            try
            {
                var responseMessage = await _feedService.GetFeedAsync(data);
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
