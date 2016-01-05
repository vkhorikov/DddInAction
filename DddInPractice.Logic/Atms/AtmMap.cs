using FluentNHibernate.Mapping;

namespace DddInPractice.Logic.Atms
{
    public class AtmMap : ClassMap<Atm>
    {
        public AtmMap()
        {
            Id(x => x.Id);

            Map(x => x.MoneyCharged);

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
