// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

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
}