using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI.Comments.Model
{
    public class EditCommentRequestModel
    {
        public string id {  get; set; }
        public EditCommentPayload snippet { get; set; }
        public EditCommentRequestModel(string videoID, string text)
        {
            id = videoID;
            snippet = new EditCommentPayload(text);
        }
    }
    public class EditCommentPayload
    {
        public string textOriginal { get; set; }
        public EditCommentPayload(string text)
        {
            this.textOriginal = text;
        }
    }
}
