using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Datas.Abstract
{
    public interface IResponsitory<T> where T : class
    {
        Task Insert(T entity);
        Task Insert(IEnumerable<T> entity);

        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T,bool>> expression);
        Task Commit();
        Task<IEnumerable<T> >Get(Expression<Func<T, bool>> expression);
        Task<T> GetSingle(Expression<Func<T, bool>> expression);
        Task<T> GetByID(object id);
        Task<IEnumerable<T>> Get();


    }
}
