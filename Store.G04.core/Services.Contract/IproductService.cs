using Store.G04.core.Dtos.Products;
using Store.G04.core.Entities;
using Store.G04.core.Helper;
using Store.G04.core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.core.Services.Contract
{
    public interface IproductService
    {
        Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpec);
        Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync();
        Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
    }
}
