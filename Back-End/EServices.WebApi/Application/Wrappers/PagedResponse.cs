using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int Offset { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public int ResultCount {get;set;}

        public PagedResponse(T data, int offset, int count)
        {
            this.Offset = offset;
            this.Count = count;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }

        public PagedResponse(T data, int offset, int count,int resultCount, int totalCount)
        {
            this.Offset = offset;
            this.Count = count;
            this.ResultCount = resultCount;
            this.TotalCount = totalCount;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }
}
