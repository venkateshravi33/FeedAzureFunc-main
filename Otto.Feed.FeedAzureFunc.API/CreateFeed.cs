using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using Otto.Feed.FeedAzureFunc.Core.Services;
using Otto.Feed.FeedAzureFunc.Core.Interfaces;
using Otto.Feed.FeedAzureFunc.Models.DTOs;
using Otto.Feed.FeedAzureFunc.API.Validations;
using Otto.Feed.FeedAzureFunc.Models.Models;

namespace Otto.Feed.FeedAzureFunc.API
{
    public class CreateFeed
    {
        private readonly IFeedService _feedService;
        private object responseMessage;

        public CreateFeed(IFeedService feedService)
        {
            _feedService = feedService;
        }

        [FunctionName("CreateFeed")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/feeds/CreateFeed")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            ValidationWrapper<FeedDTO> httpResponseBody = await req.GetBodyAsync<FeedDTO>();
            if (!httpResponseBody.IsValid)
            {
                return new BadRequestObjectResult($"Invalid input: {string.Join(", ", httpResponseBody.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            }

            string reqBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<FeedDTO>(reqBody);
            try
            {
                responseMessage = await _feedService.addFeedAsync(data);
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
            //Console.WriteLine(responseMessage);

            //var responseMessage = "testAPI";

            return new OkObjectResult(responseMessage);
        }
    }
}

