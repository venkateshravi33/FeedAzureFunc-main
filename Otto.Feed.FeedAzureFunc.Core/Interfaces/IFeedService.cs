using Otto.Feed.FeedAzureFunc.Models.DTOs;
using Otto.Feed.FeedAzureFunc.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.Feed.FeedAzureFunc.Core.Interfaces
{
	public interface IFeedService
	{
        public Task<FeedDTO> addFeedAsync(FeedDTO feed);

        public Task<IEnumerable<FeedDTO>> GetFeedAsync(long user_id);
    }
}

