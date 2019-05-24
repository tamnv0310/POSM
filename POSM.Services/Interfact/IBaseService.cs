using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSM.Services.Interfact
{
    public interface IBaseService<T> where T : class
    {
        Task InsertAsync<T>(T enity) where T : class;

        Task InsertManyAsync<T>(List<T> entities) where T : class;

        Task UpdateAsync<T>(T entity) where T : class;

        Task UpdateManyAsync<T>(List<T> entities) where T : class;

        Task DeleteAsync<T>(T entity) where T : class;

        Task DeleteManyAsync<T>(List<T> entities) where T : class;
    }
}
