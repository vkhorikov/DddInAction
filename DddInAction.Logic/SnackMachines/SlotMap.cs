using System;

using DddInAction.Logic.Common;


namespace DddInAction.Logic.SnackMachines
{
    public class SlotMap : EntityMap<Slot>
    {
        public SlotMap()
        {
            Map(x => x.Position);
            
            Component(x => x.SnackPile, y =>
            {
                y.Map(x => x.Amount);
                y.Map(x => x.Price);
                y.References(x => x.Snack).Not.LazyLoad();
            });

            References(x => x.SnackMachine);
        }
    }
}
