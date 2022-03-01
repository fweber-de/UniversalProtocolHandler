namespace ProtocolHandler
{
    class ProtocolResult
    {
        public string Protocol { get; set; }

        public string Path { get; set; }

        public ProtocolResult(string protocol, string path)
        {
            this.Protocol = protocol;
            this.Path = path;
        }
    }
}
