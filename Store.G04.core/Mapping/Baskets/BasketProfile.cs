using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.G04.core.Dtos.Baskets;
using Store.G04.core.Dtos.Products;
using Store.G04.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.core.Mapping.Baskets
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
        }
    }
}
