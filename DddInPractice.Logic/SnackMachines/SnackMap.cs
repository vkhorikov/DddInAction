using FluentNHibernate.Mapping;

namespace DddInPractice.Logic.SnackMachines
{
    public class SnackMap : ClassMap<Snack>
    {
        public SnackMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
