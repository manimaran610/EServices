public class TemplateRowConfig
    {
        public TemplateRowConfig(int orderNo, int rowIndex, int rowInsertAfter, int nestedRowCount)
        {
            OrderNo = orderNo;
            RowIndex = rowIndex;
            RowInsertAfter = rowInsertAfter;
            NestedRowCount = nestedRowCount;
            NestedRowIndex = rowInsertAfter;
            NestedRowInsertAfter = rowIndex;
        }
        public TemplateRowConfig() { }

        public int OrderNo { get; set; }
        public int NestedRowCount { get; set; }
        public int RowInsertAfter { get; set; }
        public int RowIndex { get; set; }
        public int NestedRowIndex { get; set; }
        public int NestedRowInsertAfter { get; set; }

    }