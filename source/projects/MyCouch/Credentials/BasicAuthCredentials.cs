using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyCouch
{
    using MyCouch.Net;
    using EnsureThat;
    public class BasicAuthCredentials : MyCouchCredentials
    {
        public BasicAuthCredentials()
        { }

        public BasicAuthCredentials(BasicAuthString authString)
        {
            EnsureArg.IsNotNull(authString, nameof(authString));
            var bytes = Convert.FromBase64String(authString.Value);
            string decoded = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            string[] userInfoParts = decoded.Split(new char[] { ':' }, 2);
            Credential = new System.Net.NetworkCredential(userInfoParts[0], userInfoParts[1]);
        }

        public BasicAuthCredentials(System.Net.NetworkCredential credential)
        {
            EnsureArg.IsNotNull(credential, nameof(credential));
            Credential = credential;
        }

        public BasicAuthCredentials(string login, string password)
        {
            EnsureArg.IsNotNullOrWhiteSpace(login, nameof(login));
            EnsureArg.IsNotNullOrWhiteSpace(password, nameof(password));
            Credential = new System.Net.NetworkCredential(login, password);
        }

        public System.Net.NetworkCredential Credential { get; set; }
        
        public override void PrepareHttpClient(HttpClient client)
        {
            var authString = new BasicAuthString(Credential.UserName, Credential.Password);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString.Value);
        }
    }
}
