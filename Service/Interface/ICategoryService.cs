using Course_Selling_System.Dtos;

namespace Course_Selling_System.Service.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken ct = default);
    }
}
