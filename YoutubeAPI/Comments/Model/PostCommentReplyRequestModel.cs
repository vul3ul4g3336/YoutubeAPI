using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI.Comments.Model
{
    public class PostCommentReplyRequestModel
    {

        public PostCommentReplyRequestModel(string parentID,string text)
        {
            snippet = new Snippet()
            {
                parentId = parentID,
                textOriginal = text
            };
        }
        public Snippet snippet { get; set; }


        public class Snippet
        {
            public string parentId { get; set; }
            public string textOriginal { get; set; }
        }

    }
}
