namespace HDA.Core.Models.HDACore
{
    public class Region
    {
        public int RegionID { get; set; }
        public int RegionCode { get; set; }
        public string RegionNameEn { get; set; }
        public string RegionNameAr { get; set; }
        public int CountryID { get; set; }
        public virtual Country Country { get; set; }
        public string ShapeFile { get; set; }
    }
}