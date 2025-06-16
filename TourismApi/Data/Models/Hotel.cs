namespace TourismApi.Data.Models
{
    public class Hotel : Service
    {
        public bool HasWifi { get; set; }
        public bool HasParking { get; set; }

        public string RoomView { get; set; }
    }

}
