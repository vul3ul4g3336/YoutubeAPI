using HttpUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YoutubeAPI.Model;

namespace YoutubeAPI
{
    public class TokenRefresh
    {

        private static bool NeedRefresh(GoogleCredentialModel c)
        {
          return  !DateTime.TryParse(c.ExpireTime, out var exp)
                 || DateTime.UtcNow >= exp.ToUniversalTime().AddSeconds(-30);
        }
                 

        public static InterceptorAsync RotateGoogleToken()
        {
            return async (req, ct, sendDownstream) =>
            {


                var cred = CredentialManager.Load<GoogleCredentialModel>("MyYoutubeApp_Token");
                

                if (NeedRefresh(cred))
                {
                    // 呼叫 Google Refresh API
                    Model.RefreshTokenModel m = new Model.RefreshTokenModel();
                    var form = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("client_id", m.client_id),
                        new KeyValuePair<string,string>("client_secret",m.client_secret),
                        new KeyValuePair<string,string>("refresh_token", cred.RefreshToken),
                        new KeyValuePair<string,string>("grant_type", "refresh_token"),
                    });

                    var refreshReq = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
                    {
                        Content = form
                    };

                    var refreshResp = await sendDownstream(refreshReq, ct);

                    var json = await refreshResp.Content.ReadAsStringAsync();
                    var dto = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(json);

                    cred.AccessToken = dto.access_token;
                    cred.ExpireTime = DateTime.Now.AddSeconds(dto.expires_in)
                                            .ToString("yyyy-MM-dd HH:mm:ss");
                    CredentialManager.UpSert("MyYoutubeApp_Token", cred);
                }

                // 把最新的 token 掛上去
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", cred.AccessToken);

                // 告訴 BaseHandler：繼續送
                return Interceptor.ReplaceReuqest(req);
            };
        }
        //public async Task refreshTokenAsync()
        //{
        //    HttpRequest service = new HttpRequest();
        //    GoogleCredentialModel cred = CredentialManager.Load<GoogleCredentialModel>("MyYoutubeApp_Token");
        //    HttpResponse<TokenResponse> response = await service.PostAsync<TokenResponse>("https://oauth2.googleapis.com/token", new Model.RefreshTokenModel());
        //    cred.AccessToken = response.Data.access_token;
        //    cred.ExpireTime = DateTime.Now.AddSeconds(3600).ToString("yyyy-MM-dd HH:mm:ss");
            
        //    CredentialManager.UpSert("MyYoutubeApp_Token", cred);


        //}
    }
}
