namespace Shinobytes.Terrible.Models
{
    public struct GeoBounds
    {
        public GeoCoordinate Min { get; set; }
        public GeoCoordinate Max { get; set; }
        public GeoCoordinate Center { get; set; }
    }
}