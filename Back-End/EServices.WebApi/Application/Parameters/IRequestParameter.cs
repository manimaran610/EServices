

namespace Application.Parameters
{
    public interface IRequestParameter
    {
        public int Offset { get; set; }
        public int Count { get; set; }

        /// <summary>
        /// single string for filter operations
        /// </summary> <summary>
        /// Eg: FieldName:operator:value,...
        /// Operator - eq,neq,
        /// con,ncon,
        /// sw,nsw
        /// ew,new,
        /// </summary>
        /// <value></value>
        public string Filter { get; set; }

        /// <summary>
        /// Sort value should be asc or desc
        /// </summary>
        /// FieldName:asc,FieldName:desc
        /// <value></value>
        public string Sort { get; set; }
    }
}