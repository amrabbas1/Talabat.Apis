using AutoMapper;
using Store.G04.core.Dtos.Products;
using Store.G04.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.core.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        { 
            CreateMap<Product, ProductDto>()
                .ForMember(D => D.BrandName, options => options.MapFrom(s => s.Brand.Name))
                .ForMember(D => D.TypeName, options => options.MapFrom(s => s.Type.Name));

            CreateMap<ProductBrand,TypeBrandDto>();
            CreateMap<ProductType, TypeBrandDto>();
        }
    }
}
