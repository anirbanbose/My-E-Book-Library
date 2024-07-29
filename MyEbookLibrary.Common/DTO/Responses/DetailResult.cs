using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Responses
{
    public class DetailResult<T> where T : class
    {
        private readonly T? _record;
        public bool IsSuccess { get; }

        public T? Record
        {
            get
            {
                if (!IsSuccess)
                {
                    throw new InvalidOperationException("No records found.");
                }
                return _record;
            }

            private init => _record = value;
        }

        public Error? Error { get; }

        private DetailResult(T? record)
        {
            Record = record;
            IsSuccess = true;
            Error = Error.None;
        }

        private DetailResult(Error error)
        {
            if (error == Error.None)
            {
                throw new ArgumentException("Invalid error");
            }
            IsSuccess = false;
            Error = error;
        }

        public static DetailResult<T> Success(T? record) => new DetailResult<T>(record);
        public static DetailResult<T> Failure(Error error) => new DetailResult<T>(error);
    }
}
