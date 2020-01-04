using System;
using System.Collections.Generic;
using TinyCsvParser.Mapping;
using TinyCsvParser.Model;
using TinyCsvParser.Ranges;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.NestedTypeConverter
{
    public abstract class NestedCollectionTypeConverter<TEntity> : INestedTypeConverter<TEntity>
    {

        public INestedTypeConverterProvider NestedTypeConverterProvider => throw new NotImplementedException();
        protected readonly ITypeConverterProvider typeConverterProvider;
        
        protected readonly List<IConverterMapping> converterMappings;
       
        public Type TargetType
        {
            get
            {
                return typeof(TEntity);
            }
        }

        public Func<TEntity> CreateInstance { get ; set ; }

        #region base type
        protected NestedBaseTypeConverterMapping<TTargetType> MapConverter<TTargetType>(int columnIndex)
        {
            return MapConverter<TTargetType>(columnIndex, typeConverterProvider.Resolve<TTargetType>());
        }
        protected NestedBaseTypeConverterMapping<TTargetType> MapConverter<TTargetType>(int columnIndex,TypeConverter.ITypeConverter<TTargetType> typeConverter)
        {
            var range = new TableRect(columnIndex, 0);
            var mapping = new NestedBaseTypeConverterMapping<TTargetType>(range, typeConverter);

            AddConverterMapping(mapping);

            return mapping;
        }

        #endregion
        #region nested type
        protected NestedConverterMapping<TTargetType> MapConverter<TTargetType>(int start, int end)
        {
            return MapConverter<TTargetType>(start, end, NestedTypeConverterProvider.Resolve<TTargetType>());
        }
        protected NestedConverterMapping<TTargetType> MapConverter<TTargetType>(int start, int end, INestedTypeConverter<TTargetType> nestedTypeConverter)
        {
            var rect = new TableRect(start, 0, end - start + 1, 1);
            var propertyMapping = new NestedConverterMapping<TTargetType>(rect, nestedTypeConverter);

            AddConverterMapping(propertyMapping);

            return propertyMapping;
        }

        #endregion
        #region nested type in other table 
        protected NestedRefTypeConverterMapping<TTargetType> MapConverter<TTargetType>(int columnIndex,  ITableTree roottree,IParseAddress parse = null)
        {
            if (parse == null) parse = ParseAddress.Instance;
            return MapConverter<TTargetType>(columnIndex, NestedTypeConverterProvider.Resolve<TTargetType>(), parse, roottree);
        }
        protected NestedRefTypeConverterMapping<TTargetType> MapConverter<TTargetType>(int columnIndex, INestedTypeConverter<TTargetType> nestedTypeConverter, IParseAddress parse, ITableTree roottree)
        {
            var range = new TableRect(columnIndex, 0);
            var mapping = new NestedRefTypeConverterMapping<TTargetType> (range,  roottree, nestedTypeConverter, parse);

            AddConverterMapping<TTargetType>(mapping);

            return mapping;
        }
        #endregion
        protected void AddConverterMapping<TTargetType>(IConverterMapping<TTargetType> converterMapping)
        {
            converterMappings.Add(converterMapping);
        }

        public abstract bool TryConvert(TableRect coordinate, ITable value, out TEntity result);
        
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">集合</typeparam>
    /// <typeparam name="TType">集合中元素的类行</typeparam>
    public class NestedCollectionTypeConverter<TEntity, TType> : NestedCollectionTypeConverter<TEntity>
    {
        protected Action<TEntity, TType> SetElements;
        protected void MapElements(Action<TEntity, TType> set)
        {
            SetElements = set;
        }
        public override bool TryConvert(TableRect coordinate, ITable value, out TEntity result)
        {
            var mapping = converterMappings[0] as IConverterMapping<TType>;
            var range = value.Rect.GetRelativeCoordinateRect(coordinate);
            result = CreateInstance();
            bool ret = true;
            for (int i = range.y; i <  range.heigh ; i++)
            {
                var rect = new TableRect(range.x, i, range.width, 1);
                if( ! mapping.TryConverValue(rect, value, out TType targettype))
                {
                    ret = false;
                }
                SetElements(result, targettype);
            }
            return ret;
        }
    }
    public class NestedCollectionTypeConverter<TEntity, TType,TType1> : NestedCollectionTypeConverter<TEntity>
    {
        protected Action<TEntity, TType, TType1> SetElements;
        protected void MapElements(Action<TEntity, TType, TType1> set)
        {
            SetElements = set;
        }
        public override bool TryConvert(TableRect coordinate, ITable value, out TEntity result)
        {
            var mapping = converterMappings[0] as IConverterMapping<TType>;
            var mapping1 = converterMappings[1] as IConverterMapping<TType1>;
            var range = value.Rect.GetRelativeCoordinateRect(coordinate);
            result = CreateInstance();
            bool ret = true;
            for (int i = range.y; i < range.heigh; i++)
            {
                var rect = new TableRect(range.x, i, range.width, 1);
                if (!mapping.TryConverValue(rect, value, out TType targettype))
                {
                    ret = false;
                }
                if(!mapping1.TryConverValue(rect,value,out TType1 targettype1))
                {
                    ret = false;
                }
                SetElements(result, targettype,targettype1);
            }
            return ret;
        }
    }
}
