namespace Backend.Common.Response
{
    public sealed class Error
    {
        public Error() { }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public Error(string code, string message, IEnumerable<string> errors) : this(code, message)
        {
            Code = code;
            Message = message;
            Errors = errors;
        }

        public string Code { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public static readonly Error None = new(string.Empty, null);
    }

}
