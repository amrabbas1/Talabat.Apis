using Store.G04.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.core.Specifications.Products
{
    public class ProductSpecifications : BaseSpecifications<Product,int>
    {
        public ProductSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }
        public ProductSpecifications(ProductSpecParams productSpec) : base(
            P=>
            (!productSpec.BrandId.HasValue || productSpec.BrandId == P.BrandId)
            &&
            (!productSpec.TypeId.HasValue || productSpec.TypeId == P.TypeId)
            )
        {
            if(!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch(productSpec.Sort)
                {
                    case "priceAsc":
                        OrderBy = P => P.Price;
                        break;
                    case "priceDesc":
                        OrderByDescending = P => P.Name;
                        break;
                    default:
                        OrderBy = P => P.Name;
                        break;
                }
            }
            else
            {
                OrderBy = P => P.Name;
            }
            ApplyIncludes();
            ApplyPagination(productSpec.PageSize.Value * (productSpec.PageIndex.Value - 1), productSpec.PageSize.Value);
        }
        private void ApplyIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);
        }
    }
}
