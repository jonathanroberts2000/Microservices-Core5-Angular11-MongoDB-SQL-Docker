using Servicios.api.Libreria.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Servicios.api.Libreria.Repository
{
    public interface IMongoRepository<T> where T : IDocument
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(string id);

        Task InsertDocument(T document);

        Task UpdateDocument(T document);

        Task DeleteById(string id);

        Task<PaginationEntity<T>> PaginationBy(Expression<Func<T, bool>> filterExpression, PaginationEntity<T> pagination);

        Task<PaginationEntity<T>> PaginationByFilter(PaginationEntity<T> pagination);
    }
}
