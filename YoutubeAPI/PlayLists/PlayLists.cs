using HttpUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeAPI.PlayLists.Model;

namespace YoutubeAPI.PlayLists
{
    public class PlayLists
    {
        private IHttpRequest _request;
        public PlayLists(IHttpRequest request)
        {
            _request = request;
        }
        public async Task<HttpResponse<VideoListResponseModel>> GetAllPlaylists(string maxResults)
        {
            string url = "playlists";
            var parms = new Dictionary<string, string>()
            {
                {"part","snippet,contentDetails" },
                {"mine", "true" },
                {"maxResults", maxResults }

            };

            var response = await _request.GetAsync<VideoListResponseModel>(url, parms);
            return response;
        }

        public async Task<HttpResponse<VideoListResponseModel>> GetMyPlaylistVideos(string playListID, string videoID = null, string maxResults = "50")
        {
            string url = "playlistItems";
            var parms = new Dictionary<string, string>()
            {
                {"part","snippet" },
                {"playlistId", playListID },
                {"maxResults", maxResults }
            };
            if (videoID != null)
                parms.Add("videoID", videoID);
            var response = await _request.GetAsync<VideoListResponseModel>(url, parms);
            return response;
        }

        public async Task<HttpResponse<AddVideoToPlaylistResponseModel>> AddVideoToPlaylist(string videoID, string playListID)
        {
            string url = "https://www.googleapis.com/youtube/v3/playlistItems?part=snippet";
            var parms = new Dictionary<string, string>()
            {
                {"part","snippet"}

            };
            AddVideoToPlaylistRequestModel model = new AddVideoToPlaylistRequestModel(videoID, playListID);
            var response = await _request.PostAsync<AddVideoToPlaylistResponseModel>(url, model, parms);
            return response;
        }
        public async Task<HttpResponse<string>> PlaylistItemDeleteRequest(string playListID)
        {
            string url = "https://www.googleapis.com/youtube/v3/playlistItems";
            var parms = new Dictionary<string, string>()
            {
                {"part" , playListID },
            };
            var response = await _request.DeleteAsync(url, parms);
            return response;
        }
        public async Task<HttpResponse<GetMyUploadAndLikePlaylistIdsRequestModel>> GetMyUploadAndLikePlaylistIds()
        {
            string url = "https://www.googleapis.com/youtube/v3/channels";
            var parms = new Dictionary<string, string>()
            {
                {"part" , "contentDetails" },
                {"mine","true" }
            };
            var response = await _request.GetAsync<GetMyUploadAndLikePlaylistIdsRequestModel>(url, parms);
            return response;
        }
    }
}
