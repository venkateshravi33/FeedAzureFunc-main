using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Otto.Feed.FeedAzureFunc.Models.Models
{
	public class feed
	{
        [Key]
        public long feed_id { get; set; }
        [Required]
        public long user_id { get; set; }
        [Required]
        public bool is_viewed { get; set; }
        [Required]
        //public List<long> post_collection { get; set; }
        public long[] post_collection { get; set; }
        public DateTime create_date { get; set; }
        public DateTime last_update_date { get; set; }
    }
}

