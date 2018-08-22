// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using TinyCsvParser.Model;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Mapping
{
    public class CsvPropertyNestedMapping<TEntity, TProperty> : ICsvPropertyNestedMapping<TEntity>
        where TEntity : class, new()
    {
        private string propertyName;
        private ITypeConverter<TProperty> propertyConverter;
        private Action<TEntity,TProperty> propertySetter;
        private ISerialize<TProperty> serializer; 

        public CsvPropertyNestedMapping(Action<TEntity, TProperty> propertySetter,ITypeConverter<TProperty> propertyConverter,ISerialize<TProperty> serialize)
        {
            this.propertySetter = propertySetter;
            this.propertyConverter = propertyConverter;
        }
        public bool TryMapValue(TEntity e, ITable t)
        {
            TProperty p;
            if (serializer.TrySerialize(t, out p))
            {
                propertySetter(e, p);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format("CsvPropertyMapping (PropertyName = {0}, Converter = {1})", propertyName, propertyConverter);
        }
    }
}