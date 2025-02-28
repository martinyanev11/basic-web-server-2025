namespace BasicWebServer.Server.Responses
{
    using BasicWebServer.Server.HTTP;

    public class TextFileResponse : Response
    {
        public TextFileResponse(string fileName) : base(StatusCode.OK)
        {
            this.FileName = fileName;

            this.Headers.Add(Header.ContentType, ContentType.PlainText);
        }

        public string FileName { get; set; }

        public override string ToString()
        {
            if (File.Exists(FileName))
            {
                this.Body = File.ReadAllTextAsync(this.FileName).Result;

                var fileBytesCount = new FileInfo(this.FileName).Length;

                this.Headers.Add(Header.ContentLength, fileBytesCount.ToString());

                this.Headers.Add(Header.ContentDisposition, $"attachment; filename=\"{this.FileName}\"");
            }

            return base.ToString();
        }
    }
}
