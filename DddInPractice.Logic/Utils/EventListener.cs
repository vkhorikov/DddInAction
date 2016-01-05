using DddInPractice.Logic.Common;
using NHibernate.Event;

namespace DddInPractice.Logic.Utils
{
    internal class EventListener : 
        IPostInsertEventListener, 
        IPostDeleteEventListener, 
        IPostUpdateEventListener, 
        IPostCollectionUpdateEventListener
    {
        public void OnPostUpdate(PostUpdateEvent ev)
        {
            DispatchEvents(ev.Entity as AggregateRoot);
        }

        public void OnPostDelete(PostDeleteEvent ev)
        {
            DispatchEvents(ev.Entity as AggregateRoot);
        }

        public void OnPostInsert(PostInsertEvent ev)
        {
            DispatchEvents(ev.Entity as AggregateRoot);
        }

        public void OnPostUpdateCollection(PostCollectionUpdateEvent ev)
        {
            DispatchEvents(ev.AffectedOwnerOrNull as AggregateRoot);
        }

        private void DispatchEvents(AggregateRoot aggregateRoot)
        {
            if (aggregateRoot == null)
                return;

            foreach (IDomainEvent domainEvent in aggregateRoot.DomainEvents)
            {
                DomainEvents.Dispatch(domainEvent);
            }

            aggregateRoot.ClearEvents();
        }
    }
}
