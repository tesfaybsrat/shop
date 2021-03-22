

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } =
            new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDesending { get; private set; }

        protected void AddInculude(Expression<Func<T, object>> inculudeExpression)
        {
            Includes.Add(inculudeExpression);
        }

        protected void AddOrderBy(Expression<Func<T, Object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDesending(Expression<Func<T, Object>> orderByDesendingExpression)
        {
            OrderByDesending = orderByDesendingExpression;
        }

        public int Take {get; private set;}

        public int Skip {get; private set;}

        public bool IsPagingEnabled {get; private set;}

        protected void ApplyPaging(int skip, int take){
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

    }
}