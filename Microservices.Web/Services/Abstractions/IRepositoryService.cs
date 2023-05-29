namespace Microservices.Web.Services.Abstractions
{
    public interface IRepositoryService
    {
        Task<T?> GetAllEntitiesAsync<T>() where T : class;
        Task<T?> GetEntityByIdAsync<T>(int id) where T : class;
        Task<T?> RemoveEntityAsync<T>(int id) where T : class;
        Task<T?> AddEntityAsync<T>(T entityDto) where T : class;
        Task<T?> UpdateEntityAsync<T>(string id, T entityDto) where T : class;
    }
}
