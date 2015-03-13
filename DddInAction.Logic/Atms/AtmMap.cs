using System;

using DddInAction.Logic.Common;


namespace DddInAction.Logic.Atms
{
    public class AtmMap : EntityMap<Atm>
    {
        public AtmMap()
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
        }
    }
}
