using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace BlogApp.WebApi.Utills
{
    public class PaginationParams
    {
        private const int maxPageSize = 50;
        private int pageSize = 25;

        [FromQuery(Name = "page_number")]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "page_size")]
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }

        public PaginationParams(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public PaginationParams()
        {
        }
    }
}