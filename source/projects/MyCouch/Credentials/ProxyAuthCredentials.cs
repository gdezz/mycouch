using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyCouch
{
    public class ProxyAuthCredentials : MyCouchCredentials
    {
        public const string DefaultUserNameRequestHeader = "X-Auth-CouchDB-UserName";
        public const string DefaultTokenRequestHeader = "X-Auth-CouchDB-Token";
        public const string DefaultRolesRequestHeader = "X-Auth-CouchDB-Roles";

        public ProxyAuthCredentials(string username, string token, string[] roles = null)
        {
            UserName = username;
            Token = token;
            Roles = roles ?? new string[] { };
        }

        public string UserNameRequestHeader { get; set; } = DefaultUserNameRequestHeader;

        public string TokenRequestHeader { get; set; } = DefaultTokenRequestHeader;

        public string RolesRequestHeader { get; set; } = DefaultRolesRequestHeader;

        public string UserName { get; set; }

        public string Token { get; set; }

        public string[] Roles { get; set; }

        public override void PrepareHttpClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Add(UserNameRequestHeader, UserName);
            if (!string.IsNullOrEmpty(Token))
            {
                client.DefaultRequestHeaders.Add(TokenRequestHeader, Token);
            }

            if (Roles != null)
            {
                foreach (var role in Roles)
                {
                    client.DefaultRequestHeaders.Add(RolesRequestHeader, role);
                }
            }
        }
    }
}
