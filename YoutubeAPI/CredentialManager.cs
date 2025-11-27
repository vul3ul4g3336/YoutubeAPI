using HttpUtility.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CredentialManagement;
namespace YoutubeAPI
{
    public class CredentialManager
    {
        public static bool IsAuthenticated(string targetName)
        {
            var cred = new Credential { Target = targetName };
            return cred.Load();
        }

        public static T Load<T>(string targetName) where T : class, new()
        {
            T t = new T();
            var cred = new Credential { Target = targetName };
            if (!cred.Load())
            {
                throw new Exception("Credential 不存在");
            }
            cred.Load();
            string[] credntials = cred.Password.Split('|');
            PropertyInfo[] infos = t.GetType().GetProperties();
            for (int i = 0; i < infos.Length; i++)
            {
                infos[i].SetValue(t, credntials[i]);
            }

            return t;
        }
        public static bool UpSert(string targetName, object data)
        {
            var cred = new Credential { Target = targetName,Type = CredentialType.Generic };
            string[] dataProps = data.GetType().GetProperties().Select(x => x.GetValue(data)?.ToString() ?? "").ToArray();
            string password = string.Join("|", dataProps);
            //if (!cred.Load())
            //{
            //    cred.Username = targetName;
            //    cred.Type = CredentialType.Generic;
            //}
            cred.Password = password;
            return cred.Save();
        }
    }
}
