// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Mapping
{
    public class CsvPropertyNestedMapping<TEntity> : ICsvPropertyMapping<TEntity>
    {
        private string propertyName;
        private Action<TEntity,string> propertySetter;

        public CsvPropertyNestedMapping(Action<TEntity, string> propertySetter)
        {
            this.propertySetter = propertySetter;
        }
       

        public override string ToString()
        {
            return string.Format("CsvPropertyMapping (PropertyName = {0})", propertyName);
        }

        public bool TryMapValue(TEntity entity, string value)
        {
            propertySetter(entity, value);
            return true;
        }
    }
    public class CsvBaseTypePropertyMapping<TEntity,TProperty> : ICsvPropertyMapping<TEntity>
    {
        private string propertyName;
        private ITypeConverter<TProperty> propertyConverter;
        private Action<TEntity, TProperty> propertySetter;

        public CsvBaseTypePropertyMapping(Action<TEntity, TProperty> property, ITypeConverter<TProperty> typeConverter)
        {
            propertyConverter = typeConverter;
          
        }

        public bool TryMapValue(TEntity entity, string value)
        {
            TProperty convertedValue;

            if (!propertyConverter.TryConvert(value, out convertedValue))
            {
                return false;
            }

            propertySetter(entity, convertedValue);

            return true;
        }

        public override string ToString()
        {
            return string.Format("CsvPropertyMapping (PropertyName = {0}, Converter = {1})", propertyName, propertyConverter);
        }
    }
}