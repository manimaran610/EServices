using System;
using System.Collections.Generic;
using System.Text;
using Application.Parameters;

namespace Application.Filters
{
    public class RequestParameter : IRequestParameter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }
        public RequestParameter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public RequestParameter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize < 1 ? 0 : pageSize;
        }

        public RequestParameter(int pageNumber, int pageSize, string filter, string sort)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize < 1 ? 0 : pageSize;
            this.Filter = filter;
            this.Sort = sort;
        }
    }
}
