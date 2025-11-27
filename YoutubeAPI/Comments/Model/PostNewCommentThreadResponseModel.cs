using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI.Comments
{
    public class PostNewCommentThreadResponseModel
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public Snippet snippet { get; set; }

        public class Snippet
        {
            public string channelId { get; set; }
            public string videoId { get; set; }
            public Toplevelcomment topLevelComment { get; set; }
            public bool canReply { get; set; }
            public int totalReplyCount { get; set; }
            public bool isPublic { get; set; }
        }

        public class Toplevelcomment
        {
            public string kind { get; set; }
            public string etag { get; set; }
            public string id { get; set; }
            public Snippet1 snippet { get; set; }
        }

        public class Snippet1
        {
            public string channelId { get; set; }
            public string videoId { get; set; }
            public string textDisplay { get; set; }
            public string textOriginal { get; set; }
            public string authorDisplayName { get; set; }
            public string authorProfileImageUrl { get; set; }
            public string authorChannelUrl { get; set; }
            public Authorchannelid authorChannelId { get; set; }
            public bool canRate { get; set; }
            public string viewerRating { get; set; }
            public int likeCount { get; set; }
            public DateTime publishedAt { get; set; }
            public DateTime updatedAt { get; set; }
        }

        public class Authorchannelid
        {
            public string value { get; set; }
        }

    }
}
