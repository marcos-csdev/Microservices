using Microservices.Web.Client.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microservices.Web.Client.Services.Abstractions
{
    public interface IRepositoryService
    {
        Task<ResponseDto?> GetAllEntitiesAsync();
        Task<ResponseDto?> GetEntityByIdAsync(int id);
        Task<ResponseDto?> RemoveEntityAsync(int id);

        Task<ResponseDto?> AddEntityAsync<TEntity>(TEntity? entityDto)
            where TEntity : class;

        Task<ResponseDto?> UpdateEntityAsync<TEntity>(string id, TEntity? entityDto) where TEntity : class;
    }
}
