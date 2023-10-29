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
        public RequestParameter(int offset, int count)
        {
            this.Offset = offset < 1 ? 1 : offset;
            this.Count = count < 1 ? 0 : count;
        }

        public RequestParameter(int offset, int count, string filter, string sort)
        {
            this.Offset = offset < 1 ? 1 : offset;
            this.Count = count < 1 ? 0 : count;
            this.Filter = filter;
            this.Sort = sort;
        }
    }
}
