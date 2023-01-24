using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Otto.Feed.FeedAzureFunc.Models.DTOs
{
	public class FeedDTO
	{
        public string feed_id { get; set; }
        public string user_id { get; set; }
        public string is_viewed { get; set; }
        public string[] post_collection { get; set; }
        public DateTime create_date { get; set; }
        public DateTime last_update_date { get; set; }
    }
}

