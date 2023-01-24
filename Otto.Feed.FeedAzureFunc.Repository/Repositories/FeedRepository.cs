using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
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

        public async Task<IEnumerable<feed>> addFeedAsync(feed feed)
        {
            // We can change the number of post Ids stored in each feed.
            int feed_size = 5;

            long[] post_buffer;
            List<feed> feeds = new List<feed>();

            for (int i=0; i<feed.post_collection.Length; i += feed_size)
            {
                if(feed.post_collection.Length - i < feed_size)
                {
                    post_buffer = new long[feed.post_collection.Length - i];
                    Array.Copy(feed.post_collection, i, post_buffer, 0, feed.post_collection.Length - i);
                }
                else
                {
                    post_buffer = new long[feed_size];
                    Array.Copy(feed.post_collection, i, post_buffer, 0, feed_size);
                }

                var query = "INSERT INTO feed(user_id,post_collection) " +
                "VALUES (@user_id,@post_buffer) " +
                "returning feed_id,user_id,is_viewed,post_collection,create_date,last_update_date;";

                var parameters = new DynamicParameters();
                parameters.Add("user_id", feed.user_id, DbType.Int64);
                parameters.Add("post_buffer", post_buffer, DbType.Object);

                using(var connection = _dpContext.CreateConnection())
                {
                    var row = await connection.QuerySingleOrDefaultAsync<dynamic>(query, parameters);

                    feed query_feed = new feed
                    {
                        feed_id = row.feed_id,
                        user_id = row.user_id,
                        is_viewed = row.is_viewed,
                        post_collection = row.post_collection,
                        create_date = row.create_date,
                        last_update_date = row.last_update_date
                    };

                    feeds.Add(query_feed);
                }
            }

            return feeds;
        }

        public async Task<IEnumerable<feed>> GetFeedAsync(reqfeed input)
        {
            var query = "SELECT feed_id,user_id,is_viewed,post_collection,create_date,last_update_date FROM feed " +
                "WHERE user_id = @user_id AND is_viewed = FALSE AND create_date >= @start_date AND create_date <= @end_date " +
                $"ORDER BY feed_id {input.feed_order} limit @feed_limit";

            var fallback_query = "SELECT feed_id,user_id,is_viewed,post_collection,create_date,last_update_date FROM feed " +
                "WHERE user_id = @user_id AND is_viewed = TRUE AND create_date >= @start_date AND create_date <= @end_date " +
                $"ORDER BY feed_id {input.feed_order} limit @feed_limit";

            var query2 = "UPDATE feed SET is_viewed = TRUE WHERE feed_id = @feed_id;";

            var parameters = new DynamicParameters();
            parameters.Add("user_id", input.user_id, DbType.Int64);
            parameters.Add("feed_limit", input.feed_limit, DbType.Int64);
            parameters.Add("start_date", input.start_date, DbType.DateTime2);
            parameters.Add("end_date", input.end_date, DbType.DateTime2);

            using (var connection = _dpContext.CreateConnection())
            {
                var feedlist = await connection.QueryAsync<dynamic>(query, parameters);

                if(feedlist == null)
                {
                    return null;
                }

                //Fallback feed when all is_viewed = TRUE;

                //if (feedlist == null)
                //{
                //    var fallback_feedlist = await connection.QueryAsync<dynamic>(fallback_query, parameters);

                //    if(fallback_feedlist == null)
                //    {
                //        return null;
                //    }

                //    List<feed> fallback_feeds = new();
                //    foreach(var fallback_feed in fallback_feedlist)
                //    {
                //        feed getfallback_feed = new()
                //        {
                //            feed_id = fallback_feed.feed_id,
                //            user_id = input.user_id,
                //            is_viewed = fallback_feed.is_viewed,
                //            post_collection = fallback_feed.post_collection,
                //            create_date = fallback_feed.create_date,
                //            last_update_date = fallback_feed.last_update_date
                //        };

                //        fallback_feeds.Add(getfallback_feed);
                //    }
                //    return fallback_feeds;
                //}

                List<feed> feeds = new ();
                foreach (var feed in feedlist)
                {
                    feed getfeed = new()
                    {
                        feed_id = feed.feed_id,
                        user_id = input.user_id,
                        is_viewed = feed.is_viewed,
                        post_collection = feed.post_collection,
                        create_date = feed.create_date,
                        last_update_date = feed.last_update_date
                    };

                    //method to change is_viewed status to TRUE after fetching the feed.

                    var parameters2 = new DynamicParameters();
                    parameters2.Add("feed_id", feed.feed_id, DbType.Int64);
                    await connection.QuerySingleOrDefaultAsync<dynamic>(query2, parameters2);

                    feeds.Add(getfeed);
                }
                return feeds;
            }
        }
    }
}
