using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.Feed.FeedAzureFunc.Repository.Interfaces
{
    public interface IRepositoryWrapper
    {
        IFeedRepository Feed { get; }
    }
}
