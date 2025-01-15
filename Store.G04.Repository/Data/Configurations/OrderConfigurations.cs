using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.G04.core.Entities.OdrerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.Repository.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(O => O.SubTotal).HasColumnType("decimal(18,2)");
            builder.Property(O => O.Status)
                   .HasConversion(OStatus => OStatus.ToString()
                   ,OStatus =>(OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus));

            builder.OwnsOne(O => O.ShippingAddress, SA => SA.WithOwner());//kda hkhly el ShippingAddress
                                                                          //goz2 mn el table msh table gded

            builder.HasOne(O => O.DeliveryMethod).WithMany().HasForeignKey(O => O.DeliveryMethodId);
        }
    }
}
