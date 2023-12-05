namespace FNG
{
    public class UnknownFunctionException : Exception {
        public UnknownFunctionException() : base("No functions are present for this port on the current node.") {}
    }
    public class InvalidConnectionException : Exception
    {
        public InvalidConnectionException() : base("Port types do not match/exist.") { }
    }
}