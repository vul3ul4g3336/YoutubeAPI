using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI.Videos.Model
{
    public class GetVideoRatingModel
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public Item[] items { get; set; }


        public class Item
        {
            public string videoId { get; set; }
            public string rating { get; set; }
        }

    }
}
