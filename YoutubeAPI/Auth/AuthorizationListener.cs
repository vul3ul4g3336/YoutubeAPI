using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
namespace YoutubeAPI.Auth
{
    public class AuthorizationListener
    {
        public static string FindAvailableLoopbackRedirectUri()
        {
            int port = 8080;
            // 嘗試從 8080 開始找一個可用的端口
            while (port < 9000)
            {
                try
                {
                    // 嘗試建立 HttpListener，如果成功，表示端口可用
                    var listener = new HttpListener();
                    var uri = $"http://127.0.0.1:{port}/";
                    listener.Prefixes.Add(uri);
                    listener.Start();
                    listener.Stop();
                    return uri; // 返回帶有尾隨斜線的 URI (HttpListener 需要)
                }
                catch (HttpListenerException)
                {
                    port++;
                }
            }

            throw new InvalidOperationException("無法找到可用的本地端口來啟動 HttpListener。");
        }

        public static string BuildAuthorizationUrl(string redirectUri, string codeChallenge)
        {
            var uriBuilder = new UriBuilder("https://accounts.google.com/o/oauth2/v2/auth");
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            string filePath = @"C:\Users\TUF\Documents\.youtube\credential.json";
            string jsonContent = File.ReadAllText(filePath);
            ClientModel model = JsonConvert.DeserializeObject<ClientModel>(jsonContent);
            query["client_id"] = model.ClientID;
            query["redirect_uri"] = redirectUri.TrimEnd('/'); // Google 要求的 redirect_uri 不帶尾隨斜線
            query["response_type"] = "code";
            query["scope"] = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/youtube.upload https://www.googleapis.com/auth/youtube https://www.googleapis.com/auth/youtubepartner https://www.googleapis.com/auth/youtube.force-ssl";
            query["code_challenge"] = codeChallenge;
            query["code_challenge_method"] = "S256";
            query["state"] = Guid.NewGuid().ToString(); // 建議使用 state 參數

            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }

        public static void OpenBrowser(string url)
        {
            try
            {
                // 使用 Process.Start 開啟瀏覽器 (兼容 .NET Core / .NET Framework / 跨平台)
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"無法自動開啟瀏覽器。請手動複製以下連結到您的瀏覽器：\n{url}");
                Console.WriteLine($"錯誤資訊：{ex.Message}");
            }
        }

        public static async Task<string> ListenForAuthorizationCode(string redirectUri)
        {
            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add(redirectUri);
                listener.Start();
                Console.WriteLine($"監聽 {redirectUri} 中...等待授權碼。");

                var context = await listener.GetContextAsync();
                var request = context.Request;
                var response = context.Response;

                var query = request.Url.Query;
                var queryParams = System.Web.HttpUtility.ParseQueryString(query);
                var code = queryParams["code"];

                // 回覆一個簡單的 HTML 頁面讓用戶知道流程已完成
                string responseString = !string.IsNullOrEmpty(code)
                    ? "<html><body><h1>授權成功！</h1><p>您可以關閉此視窗並返回控制台應用程式。</p></body></html>"
                    : "<html><body><h1>授權失敗</h1><p>請返回控制台查看錯誤訊息。</p></body></html>";

                var buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                response.OutputStream.Close();

                listener.Stop();
                return code;
            }
        }

        // --- Token 交換函式 (Step 4) ---

        public static async Task ExchangeCodeForToken(string code, string redirectUri, string codeVerifier)
        {
            Console.WriteLine("正在執行 Token 交換...");
            string filePath = @"C:\Users\TUF\Documents\.youtube\credential.json";
            string jsonContent = File.ReadAllText(filePath);
            ClientModel model = JsonConvert.DeserializeObject<ClientModel>(jsonContent);
            
            using (var client = new HttpClient())
            {
                // 建立 POST 請求的內容 (application/x-www-form-urlencoded)
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "code", code },
                    {"client_secret",model.ClientSecret},
                    { "client_id", model.ClientID },
                    // 注意：這裡的 redirect_uri 必須與授權請求和 Cloud Console 中註冊的完全一致
                    { "redirect_uri", redirectUri.TrimEnd('/') },
                    { "code_verifier", codeVerifier },
                    { "grant_type", "authorization_code" }
                });

                try
                {
                    var response = await client.PostAsync("https://oauth2.googleapis.com/token", content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Token 交換成功！");
                        Console.ResetColor();

                        // 解析 JSON 響應
                        using (JsonDocument document = JsonDocument.Parse(responseContent))
                        {
                            var root = document.RootElement;
                            model.AccessToken = root.GetProperty("access_token").GetString();
                            model.RefreshToken = root.TryGetProperty("refresh_token", out var rt) ? rt.GetString() : "(未獲取，可能需要添加 'offline_access' scope)";
                            model.ExpireTime = DateTime.Now.AddSeconds(root.GetProperty("expires_in").GetInt32()).ToString("yyyy-MM-dd HH:mm:ss");
                            Console.WriteLine("========================================");
                            Console.WriteLine($"Access Token: {model.AccessToken.Substring(0, 20)}... (省略部分)");
                            Console.WriteLine($"Token Type: {root.GetProperty("token_type").GetString()}");
                            Console.WriteLine($"Expires Time (秒): {model.ExpireTime}");
                            Console.WriteLine($"Refresh Token: {model.RefreshToken}");
                            Console.WriteLine("========================================");
                            Console.WriteLine("您現在可以使用 Access Token 呼叫 Google API。");
                        }
                        string outputJson = JsonConvert.SerializeObject(model, Formatting.Indented);
                        File.WriteAllText(filePath, outputJson);
                        return;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Token 交換失敗。HTTP 狀態碼: {response.StatusCode}");
                        Console.WriteLine($"錯誤訊息: {responseContent}");
                        Console.ResetColor();
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Token 交換時發生例外狀況: {ex.Message}");
                    
                }
            }
        }
    }
}
