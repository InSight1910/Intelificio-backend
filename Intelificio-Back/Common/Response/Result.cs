namespace Backend.Common.Response
{
    public class Result
    {
        private Result(bool isSuccess)
        {
            if (
                !isSuccess
                ) throw new ArgumentException("Invalid Response");
            IsSuccess = isSuccess;
        }
        private Result(bool isSuccess, ResponseData result)
        {
            if (
                !isSuccess
                ) throw new ArgumentException("Invalid Response");
            IsSuccess = isSuccess;
            Response = result;
        }

        private Result(bool isSuccess, Error error)
        {
            if (
                isSuccess && error == null ||
                !isSuccess && error == null
                ) throw new ArgumentException("Invalid Error", nameof(error));

            IsSuccess = isSuccess;
            Error = error;
        }
        private Result(bool isSuccess, Error[] errors)
        {
            if (
                isSuccess && errors == null ||
                !isSuccess && errors == null
                ) throw new ArgumentException("Invalid Error", nameof(errors));

            IsSuccess = isSuccess;
            Errors = errors;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }
        public Error[] Errors { get; }
        public ResponseData Response { get; }
        public static Result Success() => new(true);
        public static Result WithResponse(ResponseData response) => new(true, response);
        public static Result Failure(Error error) => new(false, error);
        public static Result WithErrors(Error[] errors) => new(false, errors);

    }
    public static class ResultExtension
    {
        public static T Match<T>(
                this Result result,
                Func<ResponseData, T> onSuccess,
                Func<Error, T> onFailure)
        {
            return result.IsSuccess ? onSuccess(result.Response) : onFailure(result.Error);
        }
        public static T Match<T>(
                this Result result,
                Func<T> onSuccess,
                Func<Error, T> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result.Error);
        }
    }
}
