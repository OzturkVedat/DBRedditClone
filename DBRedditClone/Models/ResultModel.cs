namespace DBRedditClone.Models
{
    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class SuccessResult : ResultModel
    {
        public SuccessResult(string message)
        {
            IsSuccess = true;
            Message = message;
        }
    }

    public class FailureResult : ResultModel
    {
        public FailureResult(string message)
        {
            IsSuccess= false;
            Message = message;
        }
    }

    public class SuccessDataResult<T> : ResultModel
    {
        public T Data { get; set; }
        public SuccessDataResult(T data)
        {
            IsSuccess = true;
            Message = "Successfuly retrieved the requested data.";
            Data = data;
        }
    }

}
