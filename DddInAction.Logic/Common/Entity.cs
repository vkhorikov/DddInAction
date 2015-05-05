using System;
using System.Collections.Generic;

using NHibernate;

using NullGuard;


namespace DddInAction.Logic.Common
{
    public abstract class Entity
    {
        public virtual long Id { get; protected set; }

        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public virtual IReadOnlyCollection<IDomainEvent> DomainEvents
        {
            get { return _domainEvents; }
        }


        protected virtual void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }


        public virtual void ClearEvents()
        {
            _domainEvents.Clear();
        }


        public override bool Equals([AllowNull] object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(compareTo, null))
                return false;

            if (ReferenceEquals(this, compareTo))
                return true;

            if (GetRealType() != compareTo.GetRealType())
                return false;

            if (!IsTransient() && !compareTo.IsTransient() && Id == compareTo.Id)
                return true;

            return false;
        }


        public static bool operator ==([AllowNull] Entity a, [AllowNull] Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }


        public static bool operator !=([AllowNull] Entity a, [AllowNull] Entity b)
        {
            return !(a == b);
        }


        public override int GetHashCode()
        {
            return (GetRealType().ToString() + Id).GetHashCode();
        }


        public virtual bool IsTransient()
        {
            return Id == 0;
        }


        public virtual Type GetRealType()
        {
            return NHibernateUtil.GetClass(this);
        }
    }
}
