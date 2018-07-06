using System;
using System.Linq;
using EnsureThat;
using MyCouch.Net;

namespace MyCouch
{
    public class DbConnectionInfo : ConnectionInfo
    {
        public string DbName { get; }

        public DbConnectionInfo(string serverAddress, string dbName) : this(new Uri(serverAddress), dbName) { }
        public DbConnectionInfo(Uri serverAddress, string dbName) : base(UriMagic.Abracadabra(serverAddress.OriginalString, dbName))
        {
            Ensure.String.IsNotNullOrWhiteSpace(dbName, nameof(dbName));

            DbName = dbName.Trim(' ', '/');
        }
    }

    public class ServerConnectionInfo : ConnectionInfo
    {
        public ServerConnectionInfo(string serverAddress) : this(new Uri(serverAddress)) { }
        public ServerConnectionInfo(Uri serverAddress) : base(serverAddress) { }
    }

    public abstract class ConnectionInfo
    {
        public Uri Address { get; }
        public TimeSpan? Timeout { get; set; }

        private MyCouchCredentials credentials;
        public MyCouchCredentials Credentials
        {
            get => credentials;
            set
            {
                credentials = value ?? AnonymousCredentials.Instance;
            }
        }

        [Obsolete("Use Credentials")]
        public BasicAuthString BasicAuth
        {
            get
            {
                BasicAuthCredentials basicAuthCredentials = Credentials as BasicAuthCredentials;
                if ((basicAuthCredentials == null) || (basicAuthCredentials.Credential == null)) return null;
                return new BasicAuthString(basicAuthCredentials.Credential);
            }
            set
            {
                if(value == null)
                {
                    Credentials = AnonymousCredentials.Instance;
                }
                else
                {
                    Credentials = new BasicAuthCredentials(value);
                }
            }
        }

        public bool AllowAutoRedirect { get; set; } = false;
        public bool ExpectContinue { get; set; } = false;
        public bool UseProxy { get; set; } = false;

        protected ConnectionInfo(Uri address)
        {
            Ensure.Any.IsNotNull(address, nameof(address));

            Address = RemoveUserInfoFrom(address);
            Credentials = null;
            if (!string.IsNullOrWhiteSpace(address.UserInfo))
            {
                var userInfoParts = ExtractUserInfoPartsFrom(address);
                Credentials = new BasicAuthCredentials(userInfoParts[0], userInfoParts[1]);
            }
        }

        private Uri RemoveUserInfoFrom(Uri address)
        {
            return new Uri(address.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.UserInfo, UriFormat.UriEscaped));
        }

        private string[] ExtractUserInfoPartsFrom(Uri address)
        {
            return address.UserInfo
                .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(Uri.UnescapeDataString)
                .ToArray();
        }
    }
}