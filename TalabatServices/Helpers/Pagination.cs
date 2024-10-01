namespace TalabatServices.Helpers
{
    public class Pagination<T>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public Pagination()
        {
            
        }

        public Pagination(int pageSize, int pageIndex, IReadOnlyList<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = data.Count;
        }
    }
}
