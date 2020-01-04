using System;
using TinyCsvParser.Model;
using TinyCsvParser.NestedTypeConverter;
using TinyCsvParser.Ranges;

namespace TinyCsvParser.Mapping
{
    public class NestedRefTypePropertyMapping<TEntity, TProperty> : INestedPropertyMapping<TEntity, ITable,TableRect>
    {
        private string propertyName;
        private INestedTypeConverter<TProperty> propertyConverter;
        private Action<TEntity, TProperty> propertySetter;
        private ITableTree tableContext;
        private IParseAddress parseAddress;
        public TableRect Rect { get; set; }
        public ICsvPropertyMapping<TEntity, string[]> PropertyMapping { get; set; }


        public NestedRefTypePropertyMapping(TableRect rect, Action<TEntity, TProperty> property, INestedTypeConverter<TProperty> typeConverter,ITableTree contex,IParseAddress parseaddress)
        {
            propertySetter = property;
            propertyConverter = typeConverter;
            Rect = rect;
            tableContext = contex;
            parseAddress = parseaddress;
        }

        public bool TryMapValue(TEntity entity, ITable value,TableRect rect)
        {
            TableRect relrect =value.Rect.GetRelativeCoordinateRect(rect).GetRelativeCoordinateRect(Rect);
            string key = value.GetCellData(relrect);
            TableRect tableRect = parseAddress.ParseRect(key);
            string tablename = parseAddress.GetTableName(key);
            string sheetname = parseAddress.GetSheetName(key);
            tablename = String.IsNullOrEmpty(tablename) ? value.TableName : tablename;
            sheetname = String.IsNullOrEmpty(sheetname) ? value.SheetName : sheetname;
            ITable table = tableContext.GetTable(tablename, sheetname);
            if (tableRect.Area == 0)
            {
                tableRect = table.Rect;
            }
            if (!propertyConverter.TryConvert(tableRect,table, out TProperty convertedValue))
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
