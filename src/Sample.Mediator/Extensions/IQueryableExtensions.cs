using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Mediator
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Conditional where clause.
        /// <para>
        /// if <paramref name="condition"/> is true, use predicate on where clause. or not.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereDependOn<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (condition)
            {
                return query.Where(predicate);
            }

            return query;
        }

    }

}
