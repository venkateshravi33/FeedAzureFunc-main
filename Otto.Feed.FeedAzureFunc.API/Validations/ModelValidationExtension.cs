﻿using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.Feed.FeedAzureFunc.API.Validations
{
    public static class ModelValidationExtension
    {
        private static ValidationWrapper<T> BuildValidationWrapper<T>(string res)
        {
            ValidationWrapper<T> body = new ValidationWrapper<T>();

            body.Value = JsonConvert.DeserializeObject<T>(res);

            var results = new List<ValidationResult>();
            body.IsValid = Validator.TryValidateObject(body.Value, new ValidationContext(body.Value, null, null), results, true);
            body.ValidationResults = results;

            return body;
        }
        public static async Task<ValidationWrapper<T>> GetBodyAsync<T>(this HttpRequest request)
        {
            var bodyString = await request.ReadAsStringAsync();
            return BuildValidationWrapper<T>(bodyString);
        }
    }
}