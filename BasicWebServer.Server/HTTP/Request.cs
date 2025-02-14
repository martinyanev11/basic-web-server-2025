﻿namespace BasicWebServer.Server.HTTP
{
    public class Request
    {
        public Method Method { get; private set; }
        public string Url { get; private set; }
        public HeaderCollection Headers { get; private set; }
        public string Body { get; private set; }

        public static Request Parse(string request) 
        { 
            var lines = request.Split(Environment.NewLine);

            var startLine = lines.First().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var method = ParseMethod(startLine[0]);
            var url = startLine[1];

            HeaderCollection headers = ParseHeaders(lines.Skip(1));

            var bodyLines = lines.Skip(headers.Count + 2).ToArray();

            var body = string.Join(Environment.NewLine, bodyLines);

            return new Request()
            {
                Method = method,
                Url = url,
                Headers = headers,
                Body = body
            };
        }

        private static HeaderCollection ParseHeaders(IEnumerable<string> headerLines)
        {
            var headereCollection = new HeaderCollection();

            foreach (var headerLine in headerLines)
            {
                if (headerLine == string.Empty) break;

                var headerParts = headerLine.Split(":", 2);

                if (headerParts.Length != 2)
                {
                    throw new InvalidOperationException("Request is not valid");
                }

                var headerName = headerParts[0];
                var headerValue = headerParts[1].Trim();

                headereCollection.Add(headerName, headerValue);
            }

            return headereCollection;
        }

        private static Method ParseMethod(string method)
        {
            try
            {
                return (Method)Enum.Parse(typeof(Method), method, true);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Method '{method}' is not supported");
            }
        }
    }
}
