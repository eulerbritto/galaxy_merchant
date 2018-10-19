namespace galaxy_merchant_back.Controllers
{
    internal sealed class UnrecognizeWordException : System.Exception
    {        
        public UnrecognizeWordException(string message) : base(message) {}
    }
}