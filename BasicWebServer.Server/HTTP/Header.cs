using BasicWebServer.Server.Common;

namespace BasicWebServer.Server.HTTP
{
    public class Header
    {
        public const string ContentType = "Content-Type";
        public const string ContentLength = "Content-Length";
        public const string Date = "Date";
        public const string Location = "Location";
        public const string Server = "Server";
        public const string Novo = "Novo";

        public Header(string name, string value)
        {
            Guard.AgainstNull(name, nameof(name));
            Guard.AgainstNull(value, nameof(value));
        }

        public string Name { get; init; }
        public string Value { get; set; }

        public override string ToString()
        {
            // Content-Type: text/plain
            return $"{this.Name}: {this.Value}";
        }
    }
}
