using System;
using System.Collections.Generic;
using System.Text;

namespace WPFBase.Shared
{
    public class ApiResponse
    {
        public string Message { get; set; }

        public bool Status { get; set; }

        public object Result { get; set; }
    }

    public class ApiResponse<T>
    {
        public string Message { get; set; }

        public bool Status { get; set; }

        public T Result { get; set; }
    }
}
