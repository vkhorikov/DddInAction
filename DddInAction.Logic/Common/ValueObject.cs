using System;

using NullGuard;


namespace DddInAction.Logic.Common
{
    public abstract class ValueObject<T>
        where T : ValueObject<T>
    {
        public override bool Equals([AllowNull] object obj)
        {
            var valueObject = obj as T;

            if (ReferenceEquals(valueObject, null))
                return false;

            return EqualsCore(valueObject);
        }


        protected abstract bool EqualsCore(T other);


        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }


        protected abstract int GetHashCodeCore();


        public static bool operator ==([AllowNull] ValueObject<T> a, [AllowNull] ValueObject<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }


        public static bool operator !=([AllowNull] ValueObject<T> a, [AllowNull] ValueObject<T> b)
        {
            return !(a == b);
        }
    }
}
