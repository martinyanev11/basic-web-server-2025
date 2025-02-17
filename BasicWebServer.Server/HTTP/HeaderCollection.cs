﻿using System.Collections;

namespace BasicWebServer.Server.HTTP
{
    public class HeaderCollection : IEnumerable<Header>
    {
        private readonly Dictionary<string, Header> _headers;

        public HeaderCollection()
            => this._headers = new Dictionary<string, Header>();

        public string this[string name] => this._headers[name].Value;

        public int Count => this._headers.Count;

        public bool Contains(string name) => this._headers.ContainsKey(name);

        public void Add(string name, string value) => this._headers[name] = new Header(name, value);

        public IEnumerator<Header> GetEnumerator() => this._headers.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
