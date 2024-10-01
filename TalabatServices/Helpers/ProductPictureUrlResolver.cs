using AutoMapper;
using AutoMapper.Execution;
using AutoMapper.Internal;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using TalabatCore.DTOs;
using TalabatCore.Entities;

namespace TalabatServices.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["ApiResponse"]}/{source.PictureUrl}";
            }

            return string.Empty;
        }
    }
}
