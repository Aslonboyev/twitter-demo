using System.Text.Json.Serialization;

namespace BlogApp.WebApi.Utills
{
    public class PaginationMetaData
    {
        [JsonPropertyName("current_page")]
        public uint CurrentPage { get; private set; }

        [JsonPropertyName("total_page")]
        public uint TotalPages { get; private set; }

        [JsonPropertyName("page_size")]
        public uint PageSize { get; private set; }

        [JsonPropertyName("total_count")]
        public uint TotalCount { get; private set; }

        [JsonPropertyName("is_first_page")]
        public bool IsFirstPage { get; private set; }

        [JsonPropertyName("is_last_page")]
        public bool IsLastPage { get; private set; }

        [JsonPropertyName("has_previous")]
        public bool HasPrevious { get; private set; }

        [JsonPropertyName("has_next")]
        public bool HasNext { get; private set; }

        public PaginationMetaData(int totalCount, PaginationParams @params)
        {
            TotalCount = (uint)totalCount;
            CurrentPage = (uint)@params.PageNumber;
            PageSize = (uint)@params.PageSize;
            TotalPages = (uint)Math.Ceiling((double)totalCount / @params.PageSize);
            IsFirstPage = @params.PageNumber == 1;
            IsLastPage = @params.PageNumber == TotalPages;
            HasPrevious = @params.PageNumber > 1;
            HasNext = @params.PageNumber < TotalPages;
        }

        public PaginationMetaData(int totalCount, int pageIndex, int pageSize)
        {
            TotalCount = (uint)totalCount;
            CurrentPage = (uint)pageIndex;
            PageSize = (uint)pageSize;
            TotalPages = (uint)Math.Ceiling((double)totalCount / pageSize);
            IsFirstPage = pageIndex == 1;
            IsLastPage = pageIndex == TotalPages;
            HasPrevious = pageIndex > 1;
            HasNext = pageIndex < TotalPages;
        }
    }
}
