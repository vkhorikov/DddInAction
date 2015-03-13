using System;

using DddInAction.Logic.Utils;

using NullGuard;


namespace DddInAction.Logic.Common
{
    public class Result
    {
        public bool Success { get; private set; }
        public string Error { get; private set; }

        public bool Failure
        {
            get { return !Success; }
        }


        protected Result(bool success, string error)
        {
            Contracts.Require(success || !error.IsNullOrEmpty());
            Contracts.Require(!success || error.IsNullOrEmpty());

            Success = success;
            Error = error;
        }


        public static Result Fail(string message)
        {
            return new Result(false, message);
        }


        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message);
        }


        public static Result Ok()
        {
            return new Result(true, String.Empty);
        }


        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, String.Empty);
        }
    }


    public class Result<T> : Result
    {
        private T _value;

        public T Value
        {
            get
            {
                Contracts.Require(Success);

                return _value;
            }
            [param: AllowNull] private set { _value = value; }
        }


        protected internal Result([AllowNull] T value, bool success, string error)
            : base(success, error)
        {
            Contracts.Require(value != null || !success);

            Value = value;
        }
    }
}
