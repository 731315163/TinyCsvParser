using System;
using TinyCsvParser.Model;
using TinyCsvParser.Ranges;

namespace TinyCsvParser.NestedTypeConverter
{
    public interface IGenericTypeConverter<TEntity>
    {
        INestedTypeConverterProvider NestedTypeConverterProvider { get; }
        Func<TEntity> CreateInstance { get; set; } 
    }
    public interface INestedTypeConverter<TTargetType> :IGenericTypeConverter<TTargetType>
    {
        bool TryConvert(TableRect coordinate, ITable value, out TTargetType result);

        Type TargetType { get; }
    }

    public interface INestedCollectionTypeConverter<TTargetType> : IGenericTypeConverter<TTargetType>
    {
        bool TryConvert(TableRect coordinate, ITable value, out TTargetType result);

        Type TargetType { get; }
    }
}
