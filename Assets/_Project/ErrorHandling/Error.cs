namespace Metaverse.ErrorHandling
{
    internal class Error
    {
        public ErrorType Type { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }

        public Error(ErrorType type, string message, string url = null)
        {
            Type = type;
            Message = message;
            Url = url;
        }
    }
}