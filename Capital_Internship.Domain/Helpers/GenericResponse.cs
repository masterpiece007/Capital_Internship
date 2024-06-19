namespace Capital_Internship.Domain.Helpers
{
    public class GenericResponse<T>
    {
        public string Code { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }


        public GenericResponse<T> Failure(string msg) => new GenericResponse<T>
        { Success = false, Message = msg, Code = ResponseCodes.FAILURE, Data = Data };

        public GenericResponse<T> Successful(string msg) => new GenericResponse<T> { Success = true, Message = msg, Code = ResponseCodes.SUCCESS, Data = Data };


    }

    public class GenericResponse
    {
        public string Code { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }


        public GenericResponse Failure(string msg) => new GenericResponse
        { Success = false, Message = msg, Code = ResponseCodes.FAILURE };

        public GenericResponse Successful(string msg) => new GenericResponse { Success = true, Message = msg, Code = ResponseCodes.SUCCESS, Data = Data };


    }

    public static class ResponseCodes
    {
        #region APPLICATION SPECIFIC CODES
        public const string FAILURE = "99";
        public const string SUCCESS = "00";
        #endregion

    }

}
