using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI.PlayLists.Model
{
    public class GetMyUploadAndLikePlaylistIdsRequestModel
    {

        public string kind { get; set; }
        public string etag { get; set; }
        public Pageinfo pageInfo { get; set; }
        public Item[] items { get; set; }


        public class Pageinfo
        {
            public int totalResults { get; set; }
            public int resultsPerPage { get; set; }
        }

        public class Item
        {
            public string kind { get; set; }
            public string etag { get; set; }
            public string id { get; set; }
            public Contentdetails contentDetails { get; set; }
        }

        public class Contentdetails
        {
            public Relatedplaylists relatedPlaylists { get; set; }
        }

        public class Relatedplaylists
        {
            public string likes { get; set; }
            public string uploads { get; set; }
        }

    }
}
