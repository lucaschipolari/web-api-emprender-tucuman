
namespace EmprenderTucumanWebApi.API
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public int StatusCode { get; set; }
        public static ApiResponse<T> CreateSuccess(T data, string message = "Success")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message,
                StatusCode = 200
            };
        }
        public static ApiResponse<T> CreateError(string message, int statusCode = 500)
        {
            return new ApiResponse<T>
            {

                Success = false,
                Message = message,
                StatusCode = statusCode
            };
        }
        public static ApiResponse<T> CreateNotFound(string message = "Recurso no encontrado")
        {

            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = 404
            };
        }
        public static ApiResponse<T> CreateValidationError(List<string> errors) {

            return new ApiResponse<T>
            {
                Success = false,
                Message = "Fallo en validacion",
                StatusCode = 400
            };
        }
    }
}
