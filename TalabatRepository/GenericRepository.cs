using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Repository.Data;
using TalabatCore.Entities;
using TalabatCore.Repositories;
using TalabatCore.Specifications;

namespace TalabatRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly TalabatDbContext _dbContext;

        public GenericRepository(TalabatDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
                return (IReadOnlyList<T>)await _dbContext.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> Spec)
        {
            return await ApplySpecification(Spec).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            if (typeof(T) == typeof(Product))
                return (T)_dbContext.Products.Where(P => P.Id == id)
                    .Include(p => p.ProductBrand)
                    .Include(p => p.ProductType);

            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdWithSpecification(ISpecification<T> Spec)
        {
            return await ApplySpecification(Spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> Spec)
        {
            return  SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }
    }
}
