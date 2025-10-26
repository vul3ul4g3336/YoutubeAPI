using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI
{

    public class GoogleCredentialModel
    {
        public GoogleCredentialModel() { }
        public String AccessToken { get; set; }
        public String ExpireTime { get; set; }
        public String RefreshToken { get; set; }

        public GoogleCredentialModel(string accessToken, string expireTime, string refreshToken)
        {
            AccessToken = accessToken;
            ExpireTime = expireTime;
            RefreshToken = refreshToken;
        }
    }
}
