// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace TinyCsvParser.Mapping
{

    public class IndexToPropertyMapping<TEntity>
    {
        public int ColumnIndex { get; set; }

        public ICsvPropertyMapping<TEntity, string> PropertyMapping { get; set; }

        public override string ToString()
        {
            return $"IndexToPropertyMapping (ColumnIndex = {ColumnIndex}, PropertyMapping = {PropertyMapping}";
        }
    }

}