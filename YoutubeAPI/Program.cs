using CefSharp.DevTools.WebAuthn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeAPI.Videos.Model;
using CredentialManagement;
using YoutubeAPI.Comments;

using Credential = CredentialManagement.Credential;
using CefSharp.DevTools.CSS;
using CefSharp.DevTools.IndexedDB;
using YoutubeAPI.Auth;
namespace YoutubeAPI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            YoutubeContext youtubeContext = new YoutubeContext("");
            youtubeContext.CreateRequest();
            UploadVideoModel model = new UploadVideoModel("測試", "", "private");
            // 原始含有多餘引號的字串
            string rawPath = "\"C:\\Users\\TUF\\Desktop\\Downloads\\file_example_MP4_480_1_5MG.mp4\"";

            // 移除首尾的引號，得到正確的系統路徑
            string filePath = rawPath.Trim('\"');
            var response = await youtubeContext.Videos.UploadVideo(model, filePath);


           // await AuthUtil.RunOAuth2Flow();

            //2.嘗試從本地儲存中載入該記錄
            //   (Load() 方法會尋找 targetName 對應的記錄)
            //if (cred.Load())
            //{
            //    3.如果找到記錄，呼叫 Delete() 方法將其從 Windows 憑證管理員中移除。
            //        這行程式碼會刪除本地端叫這個名字的記錄。
            //     cred.Delete();
            //    Console.WriteLine("成功");
            //}


            Console.ReadKey();
        }
    }
}
