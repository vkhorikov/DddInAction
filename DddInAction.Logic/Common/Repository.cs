using System;

using DddInAction.Logic.Utils;

using NHibernate;


namespace DddInAction.Logic.Common
{
    public abstract class Repository<T>
        where T : Entity
    {
        public Maybe<T> GetById(long id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                return unitOfWork.Get<T>(id);
            }
        }


        protected Result SaveCore(T entity)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    unitOfWork.SaveOrUpdate(entity);
                    unitOfWork.Commit();
                }
                return Result.Ok();
            }
            catch (HibernateException)
            {
                return Result.Fail("Unable to connect to server");
            }
        }
    }
}
