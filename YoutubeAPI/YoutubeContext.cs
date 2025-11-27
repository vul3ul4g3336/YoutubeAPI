
using HttpUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeAPI.Channel;
using YoutubeAPI.Videos;

namespace YoutubeAPI
{
    public class YoutubeContext
    {
        public string _token { get; set; }
        public string _baseUrl = "https://www.googleapis.com/youtube/v3";
        public YoutubeContext(string token )
        {            
            _token = token;
            //CreateRequest();
        }

     
        public void CreateRequest()
        {
            IHttpRequest httpRequest = new HttpRequest(false, TokenRefresh.RotateGoogleToken());
            httpRequest.BaseUrl = _baseUrl;
            httpRequest.Token = _token;
            this.Channel = new Channel.Channel(httpRequest);
            this.Comments = new Comments.Comments(httpRequest);
            this.PlayLists = new PlayLists.PlayLists(httpRequest);
            this.Videos = new Videos.Videos(httpRequest);
            //this.Auth = new Auth.Auth(httpRequest);
        }
        public Channel.Channel Channel { get; set; }
        public Comments.Comments Comments { get; set; }
        public PlayLists.PlayLists PlayLists { get; set; }
        public Videos.Videos Videos { get; set; }   
        //public Auth.Auth Auth { get; set; }


    }
}
