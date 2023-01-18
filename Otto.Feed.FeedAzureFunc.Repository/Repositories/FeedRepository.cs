using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Otto.Feed.FeedAzureFunc.Models.Models;
using Otto.Feed.FeedAzureFunc.Repository.Context;
using Otto.Feed.FeedAzureFunc.Repository.Interfaces;

namespace Otto.Feed.FeedAzureFunc.Repository.Repositories
{
    public class FeedRepository : IFeedRepository
    {
        private readonly DapperContext _dpContext;
        public FeedRepository(DapperContext dpContext)
        {
            _dpContext = dpContext;
        }

        public async Task<feed> addFeedAsync(feed feed)
        {
            //var query = "INSERT INTO feed(user_id,is_viewed,post_collection,create_date,last_update_date) " +
            //    "VALUES (@user_id,@is_viewed,@post_collection,@create_date,@last_update_date) returning feed_id;";

            var query = "INSERT INTO feed(user_id,is_viewed,post_collection) " +
                "VALUES (@user_id,@is_viewed,@post_collection) returning feed_id;";

            var parameters = new DynamicParameters();
            parameters.Add("user_id", feed.user_id, DbType.Int64);
            parameters.Add("is_viewed", feed.is_viewed, DbType.Boolean);
            parameters.Add("post_collection", feed.post_collection, DbType.Object);
            //parameters.Add("create_date", feed.create_date, DbType.DateTime);
            //parameters.Add("last_update_date", feed.last_update_date, DbType.DateTime);

            using (var connection = _dpContext.CreateConnection())
            {
                var row = await connection.QuerySingleOrDefaultAsync<dynamic>(query, parameters);
                feed.feed_id = row.feed_id;
                return feed;
            }
        }

        public async Task<IEnumerable<feed>> GetFeedAsync(long user_id)
        {
            var query = "SELECT feed_id,user_id,is_viewed,post_collection,create_date,last_update_date FROM feed WHERE user_id = @user_id AND is_viewed = FALSE ORDER BY feed_id DESC limit 1";

            var parameters = new DynamicParameters();
            parameters.Add("user_id", user_id, DbType.Int64);

            using (var connection = _dpContext.CreateConnection())
            {
                var feedlist = await connection.QueryAsync<dynamic>(query, parameters);

                if (feedlist == null)
                {
                    return null;
                }

                List<feed> feeds = new List<feed>();
                foreach (var feed in feedlist)
                {
                    feed getfeed = new feed
                    {
                        feed_id = feed.feed_id,
                        user_id = user_id,
                        is_viewed = feed.is_viewed,
                        post_collection = feed.post_collection,
                        create_date = feed.create_date,
                        last_update_date = feed.last_update_date
                    };

                    feeds.Add(getfeed);
                }
                return feeds;
            }
        }
    }
}
