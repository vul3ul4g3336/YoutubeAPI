using HttpUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YoutubeAPI.Channel.Model;
using YoutubeAPI.Videos.Model;

namespace YoutubeAPI.Channel
{
    public class Channel
    {
        private IHttpRequest _request;
        public Channel(IHttpRequest request)
        {
            _request = request;
        }

        public async Task<HttpResponse<GetYouTubeSearchModel>> SearchByKeyword(string keyword , string searchTypeEnum)
        {
            //_request.BaseUrl = "https://www.googleapis.com/youtube/v3/search";
            var parms = new Dictionary<string, string>()
            {
                {"part","snippet" },
                {"type", searchTypeEnum},
                {"q",keyword },
                {"maxResults", "20" }
            };
            HttpResponse<GetYouTubeSearchModel> response = await _request.GetAsync<GetYouTubeSearchModel>("https://www.googleapis.com/youtube/v3/search", parms);
            if (!response.IsSuccess)
            {
                throw new Exception(response.Message);
            }
            
            return response;
        }
        public async Task<HttpResponse<CheckSubscriptionResponse>> IsSubscribed(string channelID)
        {
            var parms = new Dictionary<string, string>()
            {
                {"part","snippet" },
                {"forChannelId", channelID },
                {"mine","true" }
            };
            HttpResponse<CheckSubscriptionResponse> response = await _request.GetAsync<CheckSubscriptionResponse>("https://www.googleapis.com/youtube/v3/subscriptions", parms);
            return response;
        }
        public async Task<HttpResponse<SubscribeChannelResponseModel>> SubscibeChannel(string channelId)
        {
            var CheckSubscribed = await IsSubscribed(channelId);
            if (CheckSubscribed.IsSuccess)
            {
                HttpResponse<string> result = new HttpResponse<string>(CheckSubscribed.Code, true);
            }
            var parms = new Dictionary<string, string>()
            {
                {"part","snippet" },
            };
            string url = "https://www.googleapis.com/youtube/v3/subscriptions";
            SubscribeChannelRequest model = new SubscribeChannelRequest(channelId);
            var response = await _request.PostAsync<SubscribeChannelResponseModel>(url, model, parms);
            return response;
        }

        public async Task<HttpResponse<string>> CancelUserSubscription(string channelId)
        {
            var checkSubscribe = await IsSubscribed(channelId);
            if (!checkSubscribe.IsSuccess)
            {
                return new HttpResponse<string>(checkSubscribe.Code , true);
            }
            string url = "https://www.googleapis.com/youtube/v3/subscriptions";
            var parms = new Dictionary<string, string>()
            {
                {"part", checkSubscribe.Data.item.id },
            };
            var response = await _request.DeleteAsync(url,parms);
            return response;
        }


        
    }
}
