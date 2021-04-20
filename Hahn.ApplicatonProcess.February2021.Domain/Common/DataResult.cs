namespace Hahn.ApplicatonProcess.February2021.Domain.Common
{
    public class DataResult<T>
    {
        public T[] Result { get; set; }
        public int TotalCount { get; set; }
        public int Count { get; set; }
        public string NextLink { get; set; }
    }

    public class DataResult
    {
        public object Result { get; set; }
        public int TotalCount { get; set; }
        public int Count { get; set; }
        public string NextLink { get; set; }
    }
}
