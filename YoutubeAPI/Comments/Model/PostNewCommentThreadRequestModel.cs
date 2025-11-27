using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI.Comments.Model
{
    public class PostNewCommentThreadRequestModel
    {

        public PostNewCommentThreadRequestModel(string videoID, string comment)
        {
            snippet = new Snippet
            {
                videoId = videoID,
                topLevelComment = new Toplevelcomment
                {
                    snippet = new Snippet1
                    {
                        textOriginal = comment 
                    }
                }
            };
        }
        public Snippet snippet { get; set; }


        public class Snippet
        {
            public string videoId { get; set; }
            public Toplevelcomment topLevelComment { get; set; }
        }

        public class Toplevelcomment
        {
            public Snippet1 snippet { get; set; }
        }

        public class Snippet1
        {
            public string textOriginal { get; set; }
        }

    }
}
