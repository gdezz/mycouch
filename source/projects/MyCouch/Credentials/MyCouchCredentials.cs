using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyCouch
{
    public abstract class MyCouchCredentials
    {
        public static implicit operator MyCouchCredentials(System.Net.NetworkCredential credential)
        {
            return new BasicAuthCredentials(credential);
        }

        public abstract void PrepareHttpClient(HttpClient client);
    }
}
