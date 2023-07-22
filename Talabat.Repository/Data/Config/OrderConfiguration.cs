using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());

            builder.Property(P => P.Status)
                .HasConversion(

                    OrderStatus => OrderStatus.ToString(), // Stored As String  In DB 
                    OrderStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OrderStatus) // Returned From  DB as OrderStatus
                );

            builder.Property(P => P.SubTotal).HasColumnType("decimal(18,2)");
        }
    }
}
