using AutoMapper;
using Otto.Feed.FeedAzureFunc.Core.Interfaces;
using Otto.Feed.FeedAzureFunc.Models.DTOs;
using Otto.Feed.FeedAzureFunc.Models.Models;
using Otto.Feed.FeedAzureFunc.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otto.Feed.FeedAzureFunc.Core.Services
{
    public class FeedCoreService : IFeedService
    {
        IRepositoryWrapper _repoWrapper;
        IMapper _mapper;
        public FeedCoreService(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }
        public async Task<FeedDTO> addFeedAsync(FeedDTO feed)
        {
            var data = await _repoWrapper.Feed.addFeedAsync(_mapper.Map<feed>(feed));
            return _mapper.Map<FeedDTO>(data);
        }

        public async Task<IEnumerable<FeedDTO>> GetFeedAsync(long user_id)
        {
            var data = await _repoWrapper.Feed.GetFeedAsync(user_id);
            return _mapper.Map<List<FeedDTO>>(data);
        }
    }
}
