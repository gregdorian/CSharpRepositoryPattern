namespace Empresa.Infra.Data.Repositories
{
    using Domain.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;


    public partial class xmlRepositoryBase : IRepository<TEntity> where TEntity : class
    {
        readonly string _xmlFileLocation;

        public XmlContext(ApplicationSettings appSettings)
        {
            _xmlFileLocation = appSettings.DataConnectionString;
        }



        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Edit(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public TEntity GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
 