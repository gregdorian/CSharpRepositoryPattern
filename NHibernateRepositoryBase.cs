namespace Empresa.Infra.Data.Repositories
{

    using Domain.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class NHibernateRepositoryBaseImpl<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public NHibernateRepositoryBaseImpl(ISessionFactory factory)
        {
            this.factory = factory;
        }

        public void Add(TEntity entity)
        {
            GetSession().Save(entity);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public void Edit(TEntity entity)
        {
            GetSession().Update(entity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            //return GetSession().CreateCriteria<TEntity>()
            //    .AddOrder(new Order(orderByField, ascending))
            //    .List<TEntity>();
            return GetSession().CreateCriteria<TEntity>().List<TEntity>();
        }

        public TEntity GetById(int? id)
        {
            return GetSession().Get<TEntity>(id);
        }

        public void Remove(TEntity entity)
        {
            if (!GetSession().Contains(entity))
                GetSession().Load(entity, entity.id);

            GetSession().Delete(entity);
        }

        //public virtual long Count()
        //{
        //    ICriteria crit = GetSession().CreateCriteria<TEntity>();
        //    crit.SetProjection(Projections.RowCount());
        //    object countMayBe_Int32_Or_Int64_DependingOnDatabase = crit.UniqueResult();
        //    return Convert.ToInt64(countMayBe_Int32_Or_Int64_DependingOnDatabase);
        //}

        //public virtual IEnumerable<TEntity> GetSlice(int slice, int quantity, string order, bool ascending)
        //{
        //    return GetSession().CreateCriteria<TEntity>()
        //        .SetFirstResult(slice * quantity)
        //        .SetMaxResults(quantity)
        //        .AddOrder(new Order(order, ascending))
        //        .List<TEntity>();
        //}

        //protected ISession GetSession()
        //{
        //    return factory.GetCurrentSession();
        //}
    }
}
