namespace BasicWebServer.Server.HTTP
{
    using BasicWebServer.Server.Common;

    public class Cookie
    {
        // ok
        public Cookie(string name, string value)
        {
            Guard.AgainstNull(name, nameof(name));
            Guard.AgainstNull(value, nameof(value));

            this.Name = name;
            this.Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
            => $"{this.Name}={this.Value}";
    }
}
