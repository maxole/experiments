using System;

namespace EfawateerGateway
{
    public abstract class AuthenticateTokenProvider
    {
        public static AuthenticateTokenProvider Current { get; set; }
        public abstract DateTime ExpiryDate { get; }
        public abstract string TokenKey { get; }
    }

    public class SuccessToken : AuthenticateTokenProvider
    {
        private readonly DateTime _expiryDate;
        private readonly string _tokenKey;

        public SuccessToken(string expiryDate, string tokenKey) : this(DateTime.Parse(expiryDate), tokenKey)
        {
        }

        private SuccessToken(DateTime expiryDate, string tokenKey)
        {
            _expiryDate = expiryDate;
            _tokenKey = tokenKey;
        }

        public override DateTime ExpiryDate
        {
            get { return _expiryDate; }
        }

        public override string TokenKey
        {
            get { return _tokenKey; }
        }
    }

    public class ErrorToken : AuthenticateTokenProvider
    {
        public override DateTime ExpiryDate
        {
            get { throw new Exception("Не выполнена аутентификация"); }
        }

        public override string TokenKey
        {
            get { throw new Exception("Не выполнена аутентификация"); }
        }
    }
}