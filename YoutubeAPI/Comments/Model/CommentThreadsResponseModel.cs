using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI.Comments.Model
{
    public class CommentThreadsResponseModel
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        public PageInfo pageInfo { get; set; }
        public List<CommentThreadItem> items { get; set; }

        public class PageInfo
        {
            public int totalResults { get; set; }
            public int resultsPerPage { get; set; }
        }

        public class CommentThreadItem
        {
            public string kind { get; set; }
            public string etag { get; set; }
            public string id { get; set; }
            public Snippet snippet { get; set; }
            public Replies replies { get; set; }
        }

        public class Snippet
        {
            public string channelId { get; set; }
            public string videoId { get; set; }
            public TopLevelComment topLevelComment { get; set; }
            public bool canReply { get; set; }
            public int totalReplyCount { get; set; }
            public bool isPublic { get; set; }
        }

        public class TopLevelComment
        {
            public string kind { get; set; }
            public string etag { get; set; }
            public string id { get; set; }
            public CommentSnippet snippet { get; set; }
        }

        public class CommentSnippet
        {
            public string channelId { get; set; }
            public string videoId { get; set; }
            public string textDisplay { get; set; }
            public string textOriginal { get; set; }
            public string authorDisplayName { get; set; }
            public string authorProfileImageUrl { get; set; }
            public string authorChannelUrl { get; set; }
            public AuthorChannelId authorChannelId { get; set; }
            public bool canRate { get; set; }
            public string viewerRating { get; set; }
            public int likeCount { get; set; }
            public DateTime publishedAt { get; set; }
            public DateTime updatedAt { get; set; }
            public string parentId { get; set; } // 只有回覆才會有
        }

        public class AuthorChannelId
        {
            public string value { get; set; }
        }

        public class Replies
        {
            public List<ReplyComment> comments { get; set; }
        }

        public class ReplyComment
        {
            public string kind { get; set; }
            public string etag { get; set; }
            public string id { get; set; }
            public CommentSnippet snippet { get; set; }
        }
    }

}
