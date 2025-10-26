using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI.PlayLists.Model
{
    public class AddVideoToPlaylistRequestModel
    {


        public Snippet snippet { get; set; }

        public class Snippet
        {
            public string playlistId { get; set; }
            public Resourceid resourceId { get; set; }
        }

        public class Resourceid
        {
            public string kind { get; set; }
            public string videoId { get; set; }
        }
        public AddVideoToPlaylistRequestModel(string videoID,string playlistID)
        {
            this.snippet.resourceId.videoId = videoID;
            this.snippet.playlistId = playlistID;
        }
    }
}
