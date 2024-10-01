using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatCore.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecifications(ProductsSpecificationParams Params) 
            : base(P => 
            (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId)
            &&
            (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
            )
        {

            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);

            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default: 
                        AddOrderBy(P => P.Name);
                    break;
                }
            }

            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }

        public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
