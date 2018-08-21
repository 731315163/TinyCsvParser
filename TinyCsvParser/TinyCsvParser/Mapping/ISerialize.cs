using TinyCsvParser.Model;

namespace TinyCsvParser.Mapping
{
    public interface ISerialize<T>
    {
        bool TrySerialize( ITable table,out T property);
    }
}
