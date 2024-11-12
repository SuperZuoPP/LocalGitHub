using System;
using System.Collections.Generic;
using System.Text;

namespace WPFBase.Shared.Parameters
{
    public class QueryParameter
    {
        private const int maxPageSize = 10000;

        private int _pageSize;
        public int PageIndex { get; set; }
        public int PageSize 
        {
            get { return _pageSize; }
            set { _pageSize = value> maxPageSize? maxPageSize:value; }
        }
        public string Search { get; set; }
    }
}
