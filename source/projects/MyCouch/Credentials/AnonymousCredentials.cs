using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyCouch
{
    public sealed class AnonymousCredentials : MyCouchCredentials
    {
        static AnonymousCredentials singleton;
        static AnonymousCredentials()
        {
            singleton = new AnonymousCredentials();
        }
        public static MyCouchCredentials Instance => singleton;

        private AnonymousCredentials()
        { }
        public override void PrepareHttpClient(HttpClient client)
        {
        }
    }
}
