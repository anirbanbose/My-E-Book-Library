using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEbookLibrary.Common.DTO.Responses
{
    public sealed record Error(int ErrorCode, string ErrorMessage)
    {
        private static readonly int _recordNotFoundCode = 404;
        private static readonly int _validationErrorCode = -1;
        private static readonly int _saveFailureErrorCode = -2;

        public static readonly Error None = new(0, string.Empty);
        public static Error RecordNotFound(string message) => new Error(_recordNotFoundCode, message);
        public static Error RecordNotFound() => new Error(_recordNotFoundCode, "No record found.");
        public static Error ValidationError(string message) => new Error(_validationErrorCode, message);
        public static Error SaveFailure(string message) => new Error(_saveFailureErrorCode, message);
    }
}
