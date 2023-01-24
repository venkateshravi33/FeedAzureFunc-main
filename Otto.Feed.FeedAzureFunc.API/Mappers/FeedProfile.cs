using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Otto.Feed.FeedAzureFunc.Models.DTOs;
using Otto.Feed.FeedAzureFunc.Models.Models;

namespace Otto.Feed.FeedAzureFunc.API.Mappers
{
    public class FeedProfile : Profile
    {
        public FeedProfile()
        {
            CreateMap<feed, FeedDTO>().ReverseMap();
            CreateMap<reqfeed, ReqFeedDTO>().ReverseMap();
        }

    }
}
