using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Servicios.api.Libreria.Core;
using Servicios.api.Libreria.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Servicios.api.Libreria.Repository
{
    public class MongoRepository<T> : IMongoRepository<T> where T : IDocument
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IOptions<MongoSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);

            var db = client.GetDatabase(options.Value.Database);

            _collection = db.GetCollection<T>(GetCollectionName(typeof(T)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault()).CollectionName;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _collection.Find(p => true).ToListAsync();
        }

        public async Task<T> GetById(string id)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);

            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task InsertDocument(T document)
        {
            await _collection.InsertOneAsync(document);
        }

        public async Task UpdateDocument(T document)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, document.Id);

            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public async Task DeleteById(string id)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);

            await _collection.FindOneAndDeleteAsync(filter);
        }

        public async Task<PaginationEntity<T>> PaginationBy(Expression<Func<T, bool>> filterExpression, PaginationEntity<T> pagination)
        {
            var sort = Builders<T>.Sort.Ascending(pagination.Sort);

            if(pagination.SortDirection == "desc")
            {
                sort = Builders<T>.Sort.Descending(pagination.Sort);
            }

            if(string.IsNullOrEmpty(pagination.Filter))
            {
                pagination.Data = await _collection.Find(p => true).Sort(sort).Skip( (pagination.Page - 1) * pagination.PageSize ).Limit(pagination.PageSize).ToListAsync();
            }else
            {
                pagination.Data = await _collection.Find(filterExpression).Sort(sort).Skip((pagination.Page - 1) * pagination.PageSize).Limit(pagination.PageSize).ToListAsync();
            }

            long totalDocuments = await _collection.CountDocumentsAsync(FilterDefinition<T>.Empty);

            var totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalDocuments / pagination.PageSize)));

            pagination.PagesQuantity = totalPages;

            return pagination;
        }

        public async Task<PaginationEntity<T>> PaginationByFilter(PaginationEntity<T> pagination)
        {
            var sort = Builders<T>.Sort.Ascending(pagination.Sort);

            if (pagination.SortDirection == "desc")
            {
                sort = Builders<T>.Sort.Descending(pagination.Sort);
            }

            var totalDocuments = 0;

            if (pagination.FilterValue is null)
            {
                pagination.Data = await _collection.Find(p => true)
                    .Sort(sort).Skip((pagination.Page - 1) * pagination.PageSize).Limit(pagination.PageSize).ToListAsync();

                totalDocuments = (await _collection.Find(p => true).ToListAsync()).Count();
            }
            else
            {
                //expresion regular
                var valueFilter = ".*" + pagination.FilterValue.Valor + ".*";

                var filter = Builders<T>.Filter.Regex(pagination.FilterValue.Propiedad, new BsonRegularExpression(valueFilter, "i"));

                pagination.Data = await _collection.Find(filter).Sort(sort).Skip((pagination.Page - 1) * pagination.PageSize).Limit(pagination.PageSize).ToListAsync();

                totalDocuments = (await _collection.Find(filter).ToListAsync()).Count();
            }

            //long totalDocuments = await _collection.CountDocumentsAsync(FilterDefinition<T>.Empty);

            var rounded = Math.Ceiling(totalDocuments / Convert.ToDecimal(pagination.PageSize));

            var totalPages = Convert.ToInt32(rounded);

            pagination.PagesQuantity = totalPages;
            pagination.TotalRows = Convert.ToInt32(totalDocuments);

            return pagination;
        }
    }
}
