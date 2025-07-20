namespace API_Project.Enums
{
    public class CinemaResult
    {
    }
    public enum ScheduleErrorCode
    {
        None = 0,
        MovieNotFound = 1,
        NoShowtimes = 2,
        InvalidDate = 3,
        InvalidCinema = 4,
        LayoutNotFound = 5,
        InvalidSeat = 6,
        PriceMismatch = 7,
        SaveError = 8,
        NotFound=9
    }
    public static class BookingStatuses
    {
        public const string ChoThanhToan = "Chờ thanh toán";
        public const string DaThanhToan = "Đã thanh toán";
        public const string DaHuy = "Đã hủy";
        public const string DaXem = "Đã xem";
        public const string HoanTien = "Hoàn tiền";
    }

}
