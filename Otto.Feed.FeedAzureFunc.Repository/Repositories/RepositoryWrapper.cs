using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otto.Feed.FeedAzureFunc.Repository.Context;
using Otto.Feed.FeedAzureFunc.Repository.Interfaces;

namespace Otto.Feed.FeedAzureFunc.Repository.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly DapperContext _dpContext;
        private IFeedRepository _FeedRepository;

        public RepositoryWrapper(DapperContext dpContext)
        {
            _dpContext = dpContext;
        }
        public IFeedRepository Feed
        {
            get
            {
                if (_FeedRepository == null)
                {
                    _FeedRepository = new FeedRepository(_dpContext);
                }
                return _FeedRepository;
            }
        }
    }
}