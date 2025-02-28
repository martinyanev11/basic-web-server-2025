namespace BasicWebServer.Server.HTTP
{
    using BasicWebServer.Server.Common;

    public class Session
    {
        public const string SessionCookieName = "MyWebServerSID";
        public const string SessionCurrentDateKey = "CurrentDate";

        private Dictionary<string, string> _data;

        public Session(string id)
        {
            Guard.AgainstNull(id, nameof(id));

            this.Id = id;
            this._data = new Dictionary<string, string>();
        }

        public string Id { get; init; } // TODO: should be Guid?

        public string this[string key]
        {
            get => this._data[key];
            set => this._data[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return this._data.ContainsKey(key);
        }
    }
}
