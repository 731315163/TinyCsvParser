using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Mapping
{
    public class NestedCsvMapping<TEntity>
        where TEntity : class, new()
    {
        private readonly ITypeConverterProvider typeConverterProvider;
        private readonly List<IndexToPropertyMapping<TEntity>> csvPropertyMappings;

        protected NestedCsvMapping()
            : this(new TypeConverterProvider())
        { }

        protected NestedCsvMapping(ITypeConverterProvider provider)
        {
            this.typeConverterProvider = provider;
        }
        protected CsvPropertyMapping<TEntity, TProperty> MapProperty<TProperty>(int columnIndex, Expression<Func<TEntity, TProperty>> property)
        {
           // return MapProperty(columnIndex, property, typeConverterProvider.Resolve<TProperty>());
            return null;
        }
        protected ICsvPropertyNestedMapping<TEntity> MapProperty<TProperty>(int columnIndex, Action<TEntity,ISerialize<TProperty>> del)
        {
            return new CsvPropertyNestedMapping<TProperty>();
        }

        protected ICsvPropertyMapping<TEntity> MapProperty<TProperty>(int columnIndex, Expression<Func<TEntity, TProperty>> property, ITypeConverter<TProperty> typeConverter)
        {
            if (csvPropertyMappings.Any(x => x.ColumnIndex == columnIndex))
            {
                throw new InvalidOperationException(string.Format("Duplicate mapping for column index {0}", columnIndex));
            }

            var propertyMapping = new CsvPropertyMapping<TEntity, TProperty>(property, typeConverter);

            AddPropertyMapping(columnIndex, propertyMapping);

            return propertyMapping;
        }

        private void AddPropertyMapping<TProperty>(int columnIndex, CsvPropertyMapping<TEntity, TProperty> propertyMapping)
        {
            var indexToPropertyMapping = new IndexToPropertyMapping<TEntity>
            {
                ColumnIndex = columnIndex,
                PropertyMapping = propertyMapping
            };

            csvPropertyMappings.Add(indexToPropertyMapping);
        }
    }
}
