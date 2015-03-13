using System;

using DddInAction.Logic.Common;
using DddInAction.Logic.Utils;


namespace DddInAction.Logic.HeadOffices
{
    public class HeadOfficeRepository : Repository<HeadOffice>
    {
        public void Save(HeadOffice headOffice)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                unitOfWork.SaveOrUpdate(headOffice);
                unitOfWork.Commit();
            }
        }
    }
}
