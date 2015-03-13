using System;

using DddInAction.Logic.Common;

using NHibernate.Event;


namespace DddInAction.Logic.Utils
{
    internal class EventListener : IPostInsertEventListener, IPostUpdateEventListener
    {
        public void OnPostInsert(PostInsertEvent ev)
        {
            DispatchEvents(ev.Entity as Entity);
        }


        public void OnPostUpdate(PostUpdateEvent ev)
        {
            DispatchEvents(ev.Entity as Entity);
        }


        private static void DispatchEvents(Entity entity)
        {
            foreach (IDomainEvent domainEvent in entity.DomainEvents)
            {
                DomainEvents.Dispatch(domainEvent);
            }
            
            entity.ClearEvents();
        }
    }
}
