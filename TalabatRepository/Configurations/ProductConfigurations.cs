using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace Talabat.Repository.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.ProductBrand)
                .WithMany()
                .HasForeignKey(P => P.ProductBrandId);

            builder.HasOne(P => P.ProductType)
                .WithMany()
                .HasForeignKey(P => P.ProductTypeId);

            builder.Property(P => P.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(P => P.Description).IsRequired();

            builder.Property(P => P.PictureUrl).IsRequired();
        }
    }
}
