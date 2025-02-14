using System.Collections;

namespace BasicWebServer.Server.HTTP
{
    public class HeaderCollection : IEnumerable<Header>
    {
        private readonly Dictionary<string, Header> _headers;

        public HeaderCollection()
            => this._headers = new Dictionary<string, Header>();

        public int Count => this._headers.Count;

        public void Add(string name, string value)
        {
            Header header = new Header(name, value);

            this._headers.Add(name, header);
        }

        public IEnumerator<Header> GetEnumerator() => this._headers.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
