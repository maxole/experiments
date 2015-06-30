using System;
using System.Collections.Generic;
using System.Linq;

namespace Lambda.GenH30
{
    public class WriteResponse : EventArgs
    {
        private readonly List<Exception> _errors;

        public WriteResponse()
        {
            _errors = new List<Exception>();
        }

        public string Response { get; set; }

        public Exception Error
        {
            get { return _errors.FirstOrDefault(); }
        }
        public bool Expired { get; set; }

        public void AddError(Exception exception)
        {
            _errors.Add(exception);
        }
    }
}