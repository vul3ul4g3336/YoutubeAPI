using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeAPI.Auth
{
    public static class AuthUtil
    {
        public static async Task RunOAuth2Flow()
        {
            // 步驟 1: 生成 PKCE 碼驗證器 (code_verifier) 和挑戰碼 (code_challenge)
            var (codeVerifier, codeChallenge) = GenerateCodeVerifierAndChallenge();

            // 尋找一個未被使用的本地端口並建立 Redirect URI
            var redirectUri = AuthorizationListener.FindAvailableLoopbackRedirectUri();
            Console.WriteLine($"應用程式將在 {redirectUri} 監聽授權碼。請確認此 URI 已在您的 Google Cloud Console 中註冊為授權的重定向 URI。");

            // 步驟 2.1: 建立授權 URL
            var authUrl = AuthorizationListener.BuildAuthorizationUrl(redirectUri, codeChallenge);

            Console.WriteLine("\n請在瀏覽器中完成授權。");

            // 步驟 2.2: 在系統預設瀏覽器中開啟授權 URL
            AuthorizationListener.OpenBrowser(authUrl);

            // 步驟 3: 啟動本地 HTTP 監聽器以接收授權碼
            string authorizationCode = await AuthorizationListener.ListenForAuthorizationCode(redirectUri);

            if (string.IsNullOrEmpty(authorizationCode))
            {
                Console.WriteLine("未收到授權碼或使用者拒絕授權。");
                return ;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n成功收到授權碼！");
            Console.ResetColor();

            // 步驟 4: 將授權碼交換為存取權杖 (Access Token)
             await AuthorizationListener.ExchangeCodeForToken(authorizationCode, redirectUri, codeVerifier);
            return;
        }

        // --- PKCE 相關函式 (Step 1) ---

        private static (string verifier, string challenge) GenerateCodeVerifierAndChallenge()
        {
            // 產生 code_verifier
            var codeVerifier = GenerateRandomString(64);

            // 使用 SHA256 雜湊並 Base64Url 編碼 code_challenge (S256 方法)
            using (var sha256 = SHA256.Create())
            {
                var codeVerifierBytes = Encoding.UTF8.GetBytes(codeVerifier);
                var codeChallengeBytes = sha256.ComputeHash(codeVerifierBytes);
                var codeChallenge = Base64UrlEncode(codeChallengeBytes);
                return (codeVerifier, codeChallenge);
            }
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }

        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Replace('+', '-'); // 替換 + 為 -
            output = output.Replace('/', '_'); // 替換 / 為 _
            output = output.Replace("=", ""); // 移除填充字符 =
            return output;
        }
    }
}
