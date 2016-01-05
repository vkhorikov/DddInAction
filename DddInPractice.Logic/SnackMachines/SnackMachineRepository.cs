using System.Collections.Generic;
using System.Linq;
using DddInPractice.Logic.Common;
using DddInPractice.Logic.Utils;
using NHibernate;
using NHibernate.Linq;

namespace DddInPractice.Logic.SnackMachines
{
    public class SnackMachineRepository : Repository<SnackMachine>
    {
        public IReadOnlyList<SnackMachineDto> GetSnackMachineList()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                return session.Query<SnackMachine>()
                    .ToList() // Fetch data into memory
                    .Select(x => new SnackMachineDto(x.Id, x.MoneyInside.Amount))
                    .ToList();
            }
        }
    }
}
