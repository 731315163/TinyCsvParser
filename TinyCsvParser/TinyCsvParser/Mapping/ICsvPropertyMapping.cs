// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using TinyCsvParser.Model;

namespace TinyCsvParser.Mapping
{
    public interface ICsvPropertyMapping<TEntity>
    {
        bool TryMapValue(TEntity entity, string value);
    }
}
