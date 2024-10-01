using Microsoft.EntityFrameworkCore;
using TalabatCore.Entities;
using TalabatCore.Specifications;

namespace TalabatRepository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var Query = inputQuery;
            if (specification.Criteria is not null)
            {
                Query = Query.Where(specification.Criteria);
            }

            if (specification.OrderBy is not null)
            {
                Query = Query.OrderBy(specification.OrderBy);
            }

            if (specification.OrderByDescending is not null)
            {
                Query = Query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.IsPaginationEnable)
            {
                Query = Query.Skip(specification.Skip).Take(specification.Take);
            }

            Query = specification.Includes.Aggregate(Query, (CurrentQuery, IncludeExpression) => CurrentQuery.Include(IncludeExpression));
            return Query;
        }
    }
}
