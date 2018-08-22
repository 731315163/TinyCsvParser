using TinyCsvParser.Model;

namespace TinyCsvParser.Mapping
{
    public interface ISerialize<TProperty>
    {
        bool TrySerialize( ITable table,out TProperty property);
    }
}
