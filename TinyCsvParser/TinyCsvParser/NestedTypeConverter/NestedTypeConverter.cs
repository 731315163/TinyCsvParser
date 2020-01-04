using System;
using System.Collections.Generic;
using TinyCsvParser.Mapping;
using TinyCsvParser.Model;
using TinyCsvParser.Ranges;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.NestedTypeConverter
{
    public class NestedTypeConverter<TEntity>:INestedTypeConverter<TEntity>
    {
        private readonly ITypeConverterProvider typeConverterProvider;
       
        private readonly List<NestedRectToPropertyMapping<TEntity>> csvPropertyMappings;
        protected Func<TEntity> newObject;

        public Type TargetType 
        {
            get 
            {
                return typeof(TEntity);
            }
        }
        private readonly INestedTypeConverterProvider nestedtypeConverterProvider;
        public INestedTypeConverterProvider NestedTypeConverterProvider 
        { 
            get
            {
                return nestedtypeConverterProvider;

            } 
        }

        public Func<TEntity> CreateInstance { get { return newObject; } set => throw new NotImplementedException(); }
        #region base type
        protected NestedBaseTypePropertyMapping<TEntity,TProperty> MapProperty<TProperty>(int columnIndex, Action<TEntity, TProperty> setproperty)
        {
            return MapProperty<TProperty>(columnIndex, setproperty, typeConverterProvider.Resolve<TProperty>());
        }
        protected NestedBaseTypePropertyMapping<TEntity, TProperty> MapProperty<TProperty>(int columnIndex, Action<TEntity, TProperty> property, TypeConverter.ITypeConverter<TProperty> typeConverter)
        {
            var range = new TableRect(columnIndex,0);
            var propertyMapping = new NestedBaseTypePropertyMapping<TEntity, TProperty>(range , property, typeConverter);

            AddPropertyMapping(range,propertyMapping);

            return propertyMapping;
        }
       
        #endregion
        #region nested type
        protected NestedPropertyMapping<TEntity, TProperty> MapProperty<TProperty>(int start,int end ,Action<TEntity, TProperty> setproperty)
        {
            return MapProperty<TProperty>(start,end, setproperty, nestedtypeConverterProvider.Resolve<TProperty>());
        }
        protected NestedPropertyMapping<TEntity, TProperty> MapProperty<TProperty>(int start, int end, Action<TEntity, TProperty> property, INestedTypeConverter<TProperty> nestedTypeConverter)
        {
            var range = new TableRect(start, 0,end - start + 1,1);
            var propertyMapping =new NestedPropertyMapping<TEntity,TProperty>(range,property,nestedTypeConverter);

            AddPropertyMapping(range, propertyMapping);

            return propertyMapping;
        }

        #endregion
      
        #region nested type in other table 
        protected NestedRefTypePropertyMapping<TEntity, TProperty> MapProperty<TProperty>(int columnIndex, Action<TEntity, TProperty> setproperty,IParseAddress parse,ITableTree roottree)
        {
            return MapRefProperty<TProperty>(columnIndex, setproperty, nestedtypeConverterProvider.Resolve<TProperty>(),parse,roottree);
        }
        protected NestedRefTypePropertyMapping<TEntity, TProperty> MapRefProperty<TProperty>(int columnIndex, Action<TEntity, TProperty> property, INestedTypeConverter<TProperty> nestedTypeConverter,IParseAddress parse,ITableTree roottree)
        {
            var range = new TableRect(columnIndex,0);
            var propertyMapping = new NestedRefTypePropertyMapping<TEntity, TProperty>(range, property, nestedTypeConverter,roottree,parse);

            AddPropertyMapping(range, propertyMapping);

            return propertyMapping;
        }
        #endregion
        protected void AddPropertyMapping(TableRect range, INestedPropertyMapping<TEntity, ITable,TableRect> propertyMapping)
        {
            var rangtoPropertyMap = new NestedRectToPropertyMapping<TEntity>
            {
                Rect = range,
                PropertyMapping = propertyMapping
            };
            csvPropertyMappings.Add(rangtoPropertyMap);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="value"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool TryConvert(TableRect rect ,ITable value, out TEntity entity)
        {
            
            entity = CreateInstance();
            bool flag = true;
            for(int i = 0;i < csvPropertyMappings.Count; i++)
            {
                csvPropertyMappings[i].PropertyMapping.TryMapValue(entity, value,rect);
            }
            return flag;
        }
    }
}
