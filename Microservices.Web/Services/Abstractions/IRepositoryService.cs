using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microservices.Web.Services.Abstractions
{
    public interface IRepositoryService
    {
        Task<T?> GetAllEntitiesAsync<T>() where T : class;
        Task<T?> GetEntityByIdAsync<T>(int id) where T : class;
        Task<T?> RemoveEntityAsync<T>(int id) where T : class;
        Task<T?> AddEntityAsync<T, TEntity>(TEntity? entityDto) 
            where TEntity : class
            where T : class;
        Task<T?> UpdateEntityAsync<T, TEntity>(string id, TEntity? entityDto) where TEntity : class
          where T: class;
    }
}
