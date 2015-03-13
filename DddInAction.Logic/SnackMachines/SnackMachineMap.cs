using System;

using DddInAction.Logic.Common;

using FluentNHibernate;


namespace DddInAction.Logic.SnackMachines
{
    public class SnackMachineMap : EntityMap<SnackMachine>
    {
        public SnackMachineMap()
        {
            Component(x => x.MoneyInside, y =>
            {
                y.Map(x => x.OneCentCount);
                y.Map(x => x.TenCentCount);
                y.Map(x => x.QuarterCount);
                y.Map(x => x.OneDollarCount);
                y.Map(x => x.FiveDollarCount);
                y.Map(x => x.TwentyDollarCount);
            });

            HasMany<Slot>(Reveal.Member<SnackMachine>("Slots")).Not.LazyLoad();
        }
    }
}
