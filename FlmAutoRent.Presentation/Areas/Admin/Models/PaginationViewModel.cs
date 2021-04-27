namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class PaginationViewModel
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public double pageTotal { get; set; }
        public bool displayPagination{ get; set; }
        public int totalRecords { get; set; }
        public int displayRecord { get; set; }
    }
}