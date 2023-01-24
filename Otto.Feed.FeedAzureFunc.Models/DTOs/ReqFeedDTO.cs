using System;
using System.ComponentModel.DataAnnotations;

namespace Otto.Feed.FeedAzureFunc.Models.DTOs
{
	public class ReqFeedDTO
	{
        public string user_id { get; set; }
        public string feed_limit { get; set; }
        public string feed_order { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }
}

