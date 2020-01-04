using System;

namespace TinyCsvParser.Mapping
{
    public interface INestedPropertyMapping<TEntity,TData, TCoordinate>
    {
        bool TryMapValue(TEntity e, TData data, TCoordinate coordinate);
      
    }
}
