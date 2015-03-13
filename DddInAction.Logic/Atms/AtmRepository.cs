using System;
using System.Collections.Generic;
using System.Linq;

using DddInAction.Logic.Common;
using DddInAction.Logic.Utils;


namespace DddInAction.Logic.Atms
{
    public class AtmRepository : Repository<Atm>
    {
        public Result Save(Atm atm)
        {
            return SaveCore(atm);
        }


        public IReadOnlyList<AtmDto> GetAtmList()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                return unitOfWork.Query<Atm>()
                    .ToList() // Fetch data into memory
                    .Select(x => new AtmDto(x.Id, x.MoneyInside.Amount))
                    .ToList();
            }
        }
    }
}
