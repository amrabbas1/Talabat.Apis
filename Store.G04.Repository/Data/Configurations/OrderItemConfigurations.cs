using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.G04.core.Entities.OdrerEntities;

namespace Store.G04.Repository.Data.Configurations
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(P => P.Price).HasColumnType("decimal(18,2)");

            builder.OwnsOne(O => O.Product, P => P.WithOwner());//kda hkhly el Product
                                                                //goz2 mn el table msh table gded
        }
    }
}
