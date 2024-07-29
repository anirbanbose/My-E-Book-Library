using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Responses
{
    public class DropdownResult<T> where T : class
    {
        private readonly List<T>? _records;
        public bool IsSuccess { get; }

        public List<T>? Records
        {
            get
            {
                if (!IsSuccess)
                {
                    throw new InvalidOperationException("No records found.");
                }
                return _records;
            }

            private init => _records = value;
        }

        public Error? Error { get; }

        private DropdownResult(List<T>? records)
        {
            Records = records;
            IsSuccess = true;
            Error = Error.None;
        }

        private DropdownResult(Error error)
        {
            if (error == Error.None)
            {
                throw new ArgumentException("Invalid error");
            }
            IsSuccess = false;
            Error = error;
        }

        public static DropdownResult<T> Success(List<T>? records) => new DropdownResult<T>(records);
        public static DropdownResult<T> Failure(Error error) => new DropdownResult<T>(error);
    }
}
