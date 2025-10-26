using HttpUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeAPI.Comments.Model;
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

    }
}
