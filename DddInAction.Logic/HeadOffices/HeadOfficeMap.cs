using System;

using DddInAction.Logic.Common;


namespace DddInAction.Logic.HeadOffices
{
    public class HeadOfficeMap : EntityMap<HeadOffice>
    {
        public HeadOfficeMap()
        {
            Map(x => x.Balance);

            Component(x => x.Cash, y =>
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
