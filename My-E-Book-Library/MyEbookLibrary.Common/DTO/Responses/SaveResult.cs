using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Responses
{
    public class SaveResult<T> : SaveResult where T : class
    {
        private readonly T? _savedValue;
        public T? SavedValue
        {
            get
            {
                if (!IsSuccess)
                {
                    throw new InvalidOperationException("Record not saved.");
                }
                return _savedValue;
            }

            private init => _savedValue = value;
        }

        private SaveResult(T? savedValue)
        {
            SavedValue = savedValue;
            IsSuccess = true;
            Error = Error.None;
        }

        private SaveResult(Error error) : base(error) 
        {
        }

        public static SaveResult<T> Success(T? savedRecord) => new SaveResult<T>(savedRecord);
        public static new SaveResult<T> Failure(Error error) => new SaveResult<T>(error);
    }

    public class SaveResult
    {
        public bool IsSuccess { get; protected set; }
        public Error? Error { get; protected set; }
        protected SaveResult()
        {
            IsSuccess = true;
            Error = Error.None;
        }
        protected SaveResult(Error error)
        {
            if (error == Error.None)
            {
                throw new ArgumentException("Invalid error");
            }
            IsSuccess = false;
            Error = error;
        }

        public static SaveResult Success() => new SaveResult();
        public static SaveResult Failure(Error error) => new SaveResult(error);
    }
}
