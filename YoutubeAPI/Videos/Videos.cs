using HttpUtility;
using HttpUtility.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YoutubeAPI.PlayLists.Model;
using YoutubeAPI.Videos.Model;

namespace YoutubeAPI.Videos
{
    public class Videos
    {
        private IHttpRequest _request;
        public Videos(IHttpRequest request)
        {
            _request = request;
        }

        public async Task<HttpResponse<string>> SetVideoRating(string videoID, string rating)
        {

            string url = "videos/rate";
            var parms = new Dictionary<string, string>()
            {
                {"rating",rating },
                {"id",  videoID }
            };
            var response = await _request.PostAsync<string>(url, input: null, urlParam: parms);
            return response;
        }
        public async Task<HttpResponse<UploadVideoResposeModel>> UploadVideo(UploadVideoModel model, string filePath)
        {
            string url = "https://www.googleapis.com/upload/youtube/v3/videos";
            var parms = new Dictionary<string, string>()
            {
                {"uploadType","multipart" },
                {"part",  "snippet,status" }
            };

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var videoContent = new StreamContent(fileStream);
            videoContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");
            MultipartContent content = MultipartRelateDataContent.Content(model, videoContent);

            var response = await _request.PostAsync<UploadVideoResposeModel>(url, content, parms);
            return response;
        }


        public async Task<HttpResponse<VideoListResponseModel>> ListUploadedVideos(UploadVideoModel model)
        {
            PlayLists.PlayLists playLists = new PlayLists.PlayLists(_request);
            var videoListModel = await playLists.GetMyUploadAndLikePlaylistIds();
            var uploadsListID = videoListModel.Data.items[0].contentDetails.relatedPlaylists.uploads;

            var response = await playLists.GetMyPlaylistVideos(uploadsListID);
            return response;
        }

        public async Task<HttpResponse<GetVideosResponseModel>> GetVideos(string regionCode, string chart = null, string id = null, string videoCategoryId = null)
        {
            var parms = new Dictionary<string, string>()
            {
                {"part","snippet,contentDetails,statistics" },
                {"regionCode",  regionCode },
                {"maxResults", "50" }
            };
            if (videoCategoryId != null) parms.Add("videoCategoryId", videoCategoryId);
            if (chart != null) parms.Add("chart", chart);
            if (id != null) parms.Add("id", id);
            var response = await _request.GetAsync<GetVideosResponseModel>("videos", parms);
            return response;
        }

        public async Task<HttpResponse<VideoDetailModel>> GetVideoDetail(string videoID)
        {
            var parms = new Dictionary<string, string>()
            {
                {"part","snippet,contentDetails,statistics" },
                {"id", videoID },
                {"maxResults", "50" }
            };
            string url = "videos";

            var response = await _request.GetAsync<VideoDetailModel>(url, parms);
            return response;
        }
        public async Task<HttpResponse<GetVideoRatingModel>> GetVideoRating(string videoID)
        {
            var parms = new Dictionary<string, string>()
            {
                {"id" , videoID }
            };
            string url = "videos/getRating";
            var response = await _request.GetAsync<GetVideoRatingModel>(url, parms);
            return response;
        }
        //public async Task<HttpResponse<GetCommentModel>> GetComment(string videoID)
        //{
        //    var parms = new Dictionary<string, string>()
        //    {
        //        {"videoId" , videoID },
        //        {"part","snippet,replies"  },
        //        {"maxResults", "50" }
        //    };
        //    string url = "commentThreads";
        //    var response = await _request.GetAsync<GetCommentModel>(url, parms);
        //    return response;
        //}
    }
}
