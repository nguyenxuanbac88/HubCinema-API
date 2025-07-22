using API_Project.Enums;
using API_Project.Models.Entities;

namespace API_Project.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }               // true/false
        public ScheduleErrorCode ErrorCode { get; set; } // enum lỗi
        public string Message { get; set; }             // thông báo lỗi hoặc thành công
        public T Data { get; set; }                     // dữ liệu chính

        // Phản hồi thành công
        public static ApiResponse<T> Ok(T data)
        {
            return new ApiResponse<T>
            {
                Success = true,
                ErrorCode = ScheduleErrorCode.None,
                Message = null,
                Data = data
            };
        }


        // Phản hồi thất bại
        public static ApiResponse<T> Fail(ScheduleErrorCode errorCode, string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                ErrorCode = errorCode,
                Message = message,
                Data = default
            };
        }

        internal static ApiResponse<List<Invoice>> Fail(InvoiceErrorCode error, string message)
        {
            return new ApiResponse<List<Invoice>>
            {
                Success = false,
                ErrorCode = ScheduleErrorCode.SaveError, // hoặc định nghĩa thêm enum mới nếu cần
                Message = $"[{error}] {message}",
                Data = default
            };
        }

    }
}
