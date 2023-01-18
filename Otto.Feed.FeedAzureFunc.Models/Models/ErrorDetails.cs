using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Otto.Feed.FeedAzureFunc.Models.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }

        public String ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}