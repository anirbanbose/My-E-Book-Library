using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Responses
{
    public class ListResult<T> where T : class
    {
        private readonly List<T>? _records;
        public int TotalRecordCount { get;}
        public bool IsSuccess { get; }

        public List<T>? Records
        {
            get
            {
                return _records;
            }

            private init => _records = value;
        }

        public Error? Error { get; }

        private ListResult(List<T>? records, int totalRecordCount)
        {
            Records = records;
            IsSuccess = true;
            Error = Error.None;
            TotalRecordCount = totalRecordCount;
        }

        private ListResult(Error error)
        {
            if (error == Error.None)
            {
                throw new ArgumentException("Invalid error");
            }
            IsSuccess = false;
            Error = error;
        }

        public static ListResult<T> Success(List<T>? records, int totalRecordCount) => new ListResult<T>(records, totalRecordCount);
        public static ListResult<T> Failure(Error error) => new ListResult<T>(error);
    }
}
