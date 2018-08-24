// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace TinyCsvParser.Mapping
{

    public class IndexToPropertyMapping<TEntity>
    {
        public int ColumnIndex { get; set; }

        public ICsvPropertyMapping<TEntity> PropertyMapping { get; set; }

        public override string ToString()
        {
            return string.Format("IndexToPropertyMapping (ColumnIndex = {0}, PropertyMapping = {1}", ColumnIndex, PropertyMapping);
        }
    }
    
}