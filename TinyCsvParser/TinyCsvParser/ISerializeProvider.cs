using TinyCsvParser.Mapping;

namespace TinyCsvParser
{
   public interface ISerializeProvider
    {
        ISerialize<T> Resolve<T>();
    }
}
