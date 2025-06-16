namespace TourismApi.Models
{
    public class mdlHotel : mdlService
    {
        public bool HasWifi { get; set; }
        public bool HasParking { get; set; }

        public string RoomView { get; set; }
    }
}
