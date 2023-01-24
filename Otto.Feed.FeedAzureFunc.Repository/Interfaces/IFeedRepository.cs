using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Otto.Feed.FeedAzureFunc.Models.DTOs;
using Otto.Feed.FeedAzureFunc.Models.Models;

namespace Otto.Feed.FeedAzureFunc.Repository.Interfaces
{
    public interface IFeedRepository
    {
        public Task<IEnumerable<feed>> addFeedAsync(feed feed);
        public Task<IEnumerable<feed>> GetFeedAsync(reqfeed input);
    }
}
