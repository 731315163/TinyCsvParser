using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.NestedTypeConverter
{
    public interface INestedTypeConverterProvider
    {
        INestedTypeConverter<TTargetType> Resolve<TTargetType>();

        INestedCollectionTypeConverter<TTargetType> ResolveCollection<TTargetType>();
    }
}
