using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI.Channel.Model
{
    public class SubscribeChannelRequest
    {
        public Snippet snippet { get; set; } = new Snippet();


        public class Snippet
        {
            public Resourceid resourceId { get; set; } = new Resourceid();
        }

        public class Resourceid
        {
            public string kind { get; set; } = "youtube#channel";
            public string channelId { get; set; }
        }


        //youtubeContext.Channel.Subscribe("channelID")
        //youtubeContext.Channel.UnSubscribe("channelID")
        public SubscribeChannelRequest(string chaanelID)
        {

            this.snippet.resourceId.channelId = chaanelID;
        }
    }
}
