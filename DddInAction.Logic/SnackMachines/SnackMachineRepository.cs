using System;
using System.Collections.Generic;
using System.Linq;

using DddInAction.Logic.Common;
using DddInAction.Logic.Utils;


namespace DddInAction.Logic.SnackMachines
{
    public class SnackMachineRepository : Repository<SnackMachine>
    {
        public Result Save(SnackMachine machine)
        {
            return SaveCore(machine);
        }


        public IReadOnlyList<SnackMachineDto> GetSnackMachineList()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                return unitOfWork.Query<SnackMachine>()
                    .ToList() // Fetch data into memory
                    .Select(x => new SnackMachineDto(x.Id, x.MoneyInside.Amount))
                    .ToList();
            }
        }
    }
}
