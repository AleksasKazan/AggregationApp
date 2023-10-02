using Microsoft.EntityFrameworkCore;

namespace Contracts.Models
{
    [PrimaryKey(nameof(Tinklas))]
    public class AggregatedData
    {
        public string Tinklas { get; set; }
        public int TotalRecords { get; set; }
        public decimal? PPlusSum { get; set; }
        public decimal? PMinusSum { get; set; }
    }
}
