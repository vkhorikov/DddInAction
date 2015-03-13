using System;

using DddInAction.Logic.Common;
using DddInAction.Logic.HeadOffices;

using NullGuard;


[assembly: NullGuard(ValidationFlags.All)]

namespace DddInAction.Logic.Utils
{
    public static class Initer
    {
        public static void Init()
        {
            SessionFactory.Init(@"Server=VLADIMIR-PC\SQL2012;Database=DddInAction;Trusted_Connection=true");
            DomainEvents.Init();
            HeadOfficeInstance.Init();
        }
    }
}
