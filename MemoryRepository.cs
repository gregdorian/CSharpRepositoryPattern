using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empresa.Infra.Data.Repositories
{
    
    public class MemoryRepositoryImpl<T> : IRepository<T> where T : DomainEntity
    {
        static IDictionary<Type, IDictionary<object, T>> _dic;
        static IDictionary<Type, IDictionary<object, T>> dic
        {
            get
            {
                if (_dic == null)
                {
                    _dic = new Dictionary<Type, IDictionary<object, T>>();
                }
                return _dic;
            }
        }

        public T Get(object id)
        {
            if (dic.ContainsKey(typeof(T)) && dic[typeof(T)].ContainsKey(id))
                return dic[typeof(T)][id];
            else
                return default(T);
        }

        public IEnumerable<T> GetAll()
        {
            IList<T> all = new List<T>();
            if (dic.ContainsKey(typeof(T)))
            {
                foreach (T t in dic[typeof(T)].Values)
                {
                    all.Add(t);
                }
            }
            return all;
        }

        public void Add(T entity)
        {
            if (!dic.ContainsKey(typeof(T)))
                dic[typeof(T)] = new Dictionary<object, T>> ();

            // uso reflection para setear el id porque el setter no es publico
            entity.GetType().BaseType.GetProperty("Id").SetValue(entity, dic[typeof(T)].Count + 1, null);

            dic[typeof(T)].Add(entity.Id, entity);
        }

        public void Delete(T entity)
        {
            if (dic.ContainsKey(typeof(T)) && dic[typeof(T)].ContainsKey(entity.Id))
                dic[typeof(T)].Remove(entity.Id);
        }

        public void Update(T entity)
        {
            if (dic.ContainsKey(typeof(T)) && dic[typeof(T)].ContainsKey(entity.Id))
                dic[typeof(T)][entity.Id] = entity;
        }

        public long Count()
        {
            return GetAll().Count();
        }

        public IEnumerable<T> GetSlice(int slice, int quantity, string order, bool ascending)
        {
            if (!dic.ContainsKey(typeof(T)))
                return new List();

            return dic[typeof(T)].Values.ToList().GetRange(slice, quantity);
        }
    }
}
}
