using System.Text.Json.Serialization;

namespace BlogApp.WebApi.Utills
{
    public class PagedList<T>
    {
        [JsonPropertyName("meta_data")]
        public PaginationMetaData MetaData { get; set; }

        public List<T> Data { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new PaginationMetaData(count, pageNumber, pageSize);

            Data = new List<T>(items);
        }

        public static PagedList<T> ToPagedList(IQueryable<T> source,
            int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip(
                (pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static PagedList<T> ToPagedList(IQueryable<T> source, PaginationParams @params)
        {
            var count = source.Count();
            var items = source.Skip(
                (@params.PageNumber - 1) * @params.PageSize).Take(@params.PageSize).ToList();
            return new PagedList<T>(items, count, @params.PageNumber, @params.PageSize);
        }
    }
}
