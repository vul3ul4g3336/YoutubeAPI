using HttpUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI.Auth
{
    public class Auth
    {
        public bool IsLogin
        {
            get
            {
                string filePath = @"C:\Users\TUF\Documents\.youtube\credential.json";
                string jsonContent = File.ReadAllText(filePath);
                ClientModel model = JsonConvert.DeserializeObject<ClientModel>(jsonContent);

                return  !string.IsNullOrEmpty(model.AccessToken);
            }

        }
        private IHttpRequest _request;
        //public Auth(IHttpRequest request)
        //{
        //    _request = request;
        //}
        public async Task Login()
        {
            await AuthUtil.RunOAuth2Flow();


            //YoutubeContext context = new YoutubeContext(model.AccessToken);
            //context.CreateRequest();

            //var response = await context.Channel.GetUserInfo();
            //var profileModel = response.Data;
            //model.ID = profileModel.items[0].id;
            //model.Title = profileModel.items[0].snippet.title;
            //model.CustomUrl = profileModel.items[0].snippet.customUrl;
            //model.ThumbNails = profileModel.items[0].snippet.thumbnails.@default.url;
            //CredentialManager.UpSert("MyYoutubeApp_Token", model);

            return;
        }

    }
}
