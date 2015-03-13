using System;


namespace DddInAction.Logic.Common
{
    public static class ResultExtensions
    {
        public static Result OnSuccess(this Result result, Func<Result> func)
        {
            if (result.Failure)
                return result;

            return func();
        }


        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.Failure)
                return result;

            action();

            return Result.Ok();
        }


        public static Result OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.Failure)
                return result;

            action(result.Value);

            return Result.Ok();
        }


        public static Result<T> OnSuccess<T>(this Result result, Func<T> func)
        {
            if (result.Failure)
                return Result.Fail<T>(result.Error);

            return Result.Ok(func());
        }


        public static Result<T> OnSuccess<T>(this Result result, Func<Result<T>> func)
        {
            if (result.Failure)
                return Result.Fail<T>(result.Error);

            return func();
        }


        public static Result OnSuccess<T>(this Result<T> result, Func<T, Result> func)
        {
            if (result.Failure)
                return result;

            return func(result.Value);
        }


        public static Result OnFailure(this Result result, Action action)
        {
            if (result.Failure)
            {
                action();
            }

            return result;
        }


        public static void OnBoth(this Result result, Action<Result> action)
        {
            action(result);
        }
    }
}
