using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI.Model
{
    public class RefreshTokenModel
    {
        public string client_id { get; set; }
        public string client_secret { get; set; } 
        public string grant_type { get; set; } = "refresh_token";
        public string redirect_uri { get; set; } = "http://localhost:5500/redirect.html";
        public string refresh_token { get; set; } 
    }
}
