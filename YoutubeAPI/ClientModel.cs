using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI
{
    public class ClientModel
    {

        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string ExpireTime { get; set; }
        public string UserID { get; set; }
        public string NickName { get; set; }
        public string CustomUrl { get; set; }
        public string ThumbNails { get; set; }


    }
}
