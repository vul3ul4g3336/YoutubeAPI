using CefSharp.DevTools.Network;
using HttpUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeAPI.Comments.Model;
using YoutubeAPI.Videos;
using YoutubeAPI.Videos.Model;

namespace YoutubeAPI.Comments
{
    public class Comments
    {
        private IHttpRequest _request;
        public Comments(IHttpRequest request)
        {
            _request = request;
        }
        public async Task<HttpResponse<GetCommentModel>> GetComment(string videoID)
        {
            var parms = new Dictionary<string, string>()
            {
                {"videoId" , videoID },
                {"part","snippet,replies"  },
                {"maxResults", "50" }
            };
            string url = "commentThreads";
            var response = await _request.GetAsync<GetCommentModel>(url, parms);
            return response;
        }
        public async Task<HttpResponse<string>> DeleteComment(string commentID)
        {
            var parms = new Dictionary<string, string>()
            {
                {"id" , commentID },
            };
            string url = "comments";
            var response = await _request.DeleteAsync(url, parms);
            return response;
        }
        public async Task<HttpResponse<PostNewCommentThreadResponseModel>> PostNewCommentThread(string videoID, string comment)
        {
            var parms = new Dictionary<string, string>()
            {
                {"part","snippet" }
            };
            string url = "commentThreads";
            PostNewCommentThreadRequestModel model = new PostNewCommentThreadRequestModel(videoID, comment);
            var response = await _request.PostAsync<PostNewCommentThreadResponseModel>(url, model, parms);
            return response;
        }
        public async Task<HttpResponse<PostCommentReplyResponseModel>> PostCommentReply(string parentID, string text)
        {
            var parms = new Dictionary<string, string>()
            {
                {"part","snippet" }
            };
            string url = "comments";
            PostCommentReplyRequestModel model = new PostCommentReplyRequestModel(parentID, text);
            var response =await _request.PostAsync<PostCommentReplyResponseModel>(url, model, parms);
            return response;
        }
    }
}
