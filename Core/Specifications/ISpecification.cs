using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        //where
         Expression<Func<T, bool>> Criteria {get;}
         //inculude
         List<Expression<Func<T, object>>> Includes {get;}
    }
}