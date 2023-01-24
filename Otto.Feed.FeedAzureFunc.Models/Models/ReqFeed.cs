using System;
using System.ComponentModel.DataAnnotations;

namespace Otto.Feed.FeedAzureFunc.Models.Models
{
	public class reqfeed
	{
        [Key]
        public long user_id { get; set; }
        public long feed_limit { get; set; }
        public string feed_order { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
    }
}

