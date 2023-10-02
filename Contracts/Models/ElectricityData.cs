namespace Contracts.Models
{
    public class ElectricityData
    {
        public string Tinklas { get; set; }
        public string ObtPavadinimas { get; set; }
        public string ObjGvTipas { get; set; }
        public int ObjNumeris { get; set; }
        public decimal? PPlus { get; set; }
        public DateTime PlT { get; set; }
        public decimal? PMinus { get; set; }
    }
}
