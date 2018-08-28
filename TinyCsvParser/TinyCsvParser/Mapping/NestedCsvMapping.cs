using System;
using System.Collections.Generic;
using System.Linq;
using TinyCsvParser.Load;
using TinyCsvParser.Model;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Mapping
{
    public class NestedCsvMapping<TEntity>
    {
        private readonly ITypeConverterProvider typeConverterProvider;
        
        private readonly List<IndexToPropertyMapping<TEntity>> csvPropertyMappings;

        protected readonly IParseAddress parseAddress;
        protected Func<TEntity> newobject = () => { return Activator.CreateInstance<TEntity>(); };
        /// <summary>
        /// 这个类的使用的table数据
        /// </summary>
        protected ITable table;
        protected NestedCsvMapping()
            : this(new TypeConverterProvider())
        { }

        protected NestedCsvMapping(ITypeConverterProvider provider)
        {
            this.typeConverterProvider = provider;
        }
  
        protected ICsvPropertyMapping<TEntity> MapProperty<TProperty>(int columnIndex, Action<TEntity,TProperty> setproperty)
        {
            return MapProperty<TProperty>(columnIndex,setproperty,typeConverterProvider.Resolve<TProperty>());
        }

        protected ICsvPropertyMapping<TEntity> MapProperty<TProperty>(int columnIndex,Action<TEntity, TProperty> property, ITypeConverter<TProperty> typeConverter)
        {
            for (var i = 0; i < csvPropertyMappings.Count; i++)
            {
                var x = csvPropertyMappings[i];
                if (x.ColumnIndex == columnIndex)
                {
                    throw new InvalidOperationException(string.Format("Duplicate mapping for column index {0}",
                        columnIndex));
                }
            }

            var propertyMapping = new CsvBaseTypePropertyMapping<TEntity, TProperty>(property, typeConverter);

            AddPropertyMapping(columnIndex, propertyMapping);

            return propertyMapping;
        }

        protected ICsvPropertyMapping<TEntity> MapProperty(int columnIndex,Action<TEntity, ITable> property)
        {
            Action<TEntity,string> propertySetter = (e, s) =>
            {
                var csvtable = TableContext.Instance.GetTable(s, this.table);
                property(e, csvtable);
            };
            var propertyMapping = new CsvPropertyNestedMapping<TEntity>(propertySetter);

            AddPropertyMapping(columnIndex, propertyMapping);

            return propertyMapping;
        }
        private void AddPropertyMapping<TProperty>(int columnIndex, CsvBaseTypePropertyMapping<TEntity, TProperty> propertyMapping)
        {
            var indexToPropertyMapping = new IndexToPropertyMapping<TEntity>
            {
                ColumnIndex = columnIndex,
                PropertyMapping = propertyMapping
            };

            csvPropertyMappings.Add(indexToPropertyMapping);
        }
        private void AddPropertyMapping(int columnIndex, CsvPropertyNestedMapping<TEntity> propertyMapping)
        {
            var indexToPropertyMapping = new IndexToPropertyMapping<TEntity>
            {
                ColumnIndex = columnIndex,
                PropertyMapping = propertyMapping
            };

            csvPropertyMappings.Add(indexToPropertyMapping);
        }

        public CsvMappingResult<TEntity> Map(TokenizedRow values)
        {
            TEntity entity = newobject();

            for (int pos = 0; pos < csvPropertyMappings.Count; pos++)
            {
                var indexToPropertyMapping = csvPropertyMappings[pos];

                var columnIndex = indexToPropertyMapping.ColumnIndex;

                if (columnIndex >= values.Tokens.Length)
                {
                    return new CsvMappingResult<TEntity>()
                    {
                        RowIndex = values.Index,
                        Error = new CsvMappingError()
                        {
                            ColumnIndex = columnIndex,
                            Value = string.Format("Column {0} is Out Of Range", columnIndex)
                        }
                    };
                }

                var value = values.Tokens[columnIndex];

                if (!indexToPropertyMapping.PropertyMapping.TryMapValue(entity, value))
                {
                    return new CsvMappingResult<TEntity>()
                    {
                        RowIndex = values.Index,
                        Error = new CsvMappingError
                        {
                            ColumnIndex = columnIndex,
                            Value = string.Format("Column {0} with Value '{1}' cannot be converted", columnIndex, value)
                        }
                    };
                }
            }

            return new CsvMappingResult<TEntity>()
            {
                RowIndex = values.Index,
                Result = entity
            };
        }
        public CsvMappingResult<TEntity> Map(ArraySegment<string> values)
        {
            TEntity entity = newobject();

            return new CsvMappingResult<TEntity>()
            {
              
                Result = entity
            };
        }

    }
}
