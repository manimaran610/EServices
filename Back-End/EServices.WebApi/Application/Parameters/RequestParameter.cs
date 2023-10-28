using System;
using System.Collections.Generic;
using System.Text;
using Application.Parameters;

namespace Application.Filters
{
    public class RequestParameter : IRequestParameter
    {
        public int Offset { get; set; }
        public int Count { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }
        public RequestParameter()
        {
            this.Offset = 1;
            this.Count = 10;
        }
        public RequestParameter(int pageNumber, int pageSize)
        {
            this.Offset = pageNumber < 1 ? 1 : pageNumber;
            this.Count = pageSize < 1 ? 0 : pageSize;
        }

        public RequestParameter(int pageNumber, int pageSize, string filter, string sort)
        {
            this.Offset = pageNumber < 1 ? 1 : pageNumber;
            this.Count = pageSize < 1 ? 0 : pageSize;
            this.Filter = filter;
            this.Sort = sort;
        }
    }
}
