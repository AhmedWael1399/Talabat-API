using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;
using TalabatCore.Specifications;

namespace TalabatCore.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region Without Specification
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);

        #endregion

        #region With Specification

        Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> Spec);
        Task<T> GetByIdWithSpecification(ISpecification<T> Spec);

        #endregion
    }
}
