namespace ApexWebAPI.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Ok(T data, string? message = null) => new()
        {
            Success = true,
            StatusCode = 200,
            Message = message,
            Data = data
        };

        public static ApiResponse<T> Created(T data, string? message = null) => new()
        {
            Success = true,
            StatusCode = 201,
            Message = message,
            Data = data
        };

        public static ApiResponse<T> Fail(int statusCode, string message) => new()
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            Data = default
        };
    }

    public class ApiResponse : ApiResponse<object>
    {
        public static ApiResponse Ok(string? message = null) => new()
        {
            Success = true,
            StatusCode = 200,
            Message = message
        };

        public static new ApiResponse Fail(int statusCode, string message) => new()
        {
            Success = false,
            StatusCode = statusCode,
            Message = message
        };
    }
}
