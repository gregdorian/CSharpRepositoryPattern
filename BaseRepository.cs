namespace Empresa.Infra.Data.Repositories
{
    using Context;
    using Domain.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;


    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        private bool disposed = false;

        protected EmpresaContext _contexto = new EmpresaContext();
        //private readonly DbSet<TEntity> _dbEntitySet;

        public void Add(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _contexto.Set<TEntity>().Add(entity);
            _contexto.SaveChanges(); //usar UoW???
        }

        public void Edit(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _contexto.Set<TEntity>().Attach(entity);
            _contexto.Entry(entity).State = EntityState.Modified;
            _contexto.SaveChanges();
        }

        public TEntity GetById(int? id)
        {
            return _contexto.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _contexto.Set<TEntity>().ToList();
        }

        public void Remove(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _contexto.Set<TEntity>().Attach(entity);
            _contexto.Set<TEntity>().Remove(entity);
            _contexto.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _contexto.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

}
