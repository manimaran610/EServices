using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Filters
{
    public class RequestParameter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        /// <summary>
        /// single string for filter operations
        /// </summary> <summary>
        /// Eg: FieldName:operator:value,...
        /// Operator - eq,ieq,neq,ineq,
        /// con,ncon,icon,incon,
        /// sw,isw,nsw,insw
        /// ew,iew,new,inew,
        /// in,nin,iin,inin
        /// </summary>
        /// <value></value>
        public string Filter { get; set; }

        /// <summary>
        /// Sort value should be asc or desc
        /// </summary>
        /// FieldName:asc,FieldName:desc
        /// <value></value>
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
