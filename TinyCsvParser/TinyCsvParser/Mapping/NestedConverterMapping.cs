using System;
using TinyCsvParser.Model;
using TinyCsvParser.NestedTypeConverter;
using TinyCsvParser.Ranges;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Mapping
{
      public class NestedRefTypeConverterMapping<TTargetType> : IConverterMapping<TTargetType>
    {
        public TableRect Rect { get; set; }
        private ITableTree tableContext;
        private IParseAddress parseAddress;
        public INestedTypeConverter<TTargetType> ConverterMapping { get; set; }
        public NestedRefTypeConverterMapping(TableRect rect, ITableTree tree,INestedTypeConverter<TTargetType> converter, IParseAddress parseaddress)
        {
            this.Rect = rect;
            this.tableContext = tree;
            this.parseAddress = parseaddress;
            this.ConverterMapping = converter;
        }
        public bool TryConverValue(TableRect rect,ITable value, out TTargetType convertedValue)
        {
            TableRect relrect = value.Rect.GetRelativeCoordinateRect(rect).GetRelativeCoordinateRect(Rect);
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
            if (!ConverterMapping.TryConvert(tableRect, table, out convertedValue))
            {
                return false;
            }

            return true;
        }
        public override string ToString()
        {
            return $"IndexToPropertyMapping (Range = {Rect}, PropertyMapping = {ConverterMapping}";
        }

      
    }

    public class NestedConverterMapping<TTargetType>:IConverterMapping<TTargetType>
    {
        public TableRect Rect { get; set; }

        public INestedTypeConverter<TTargetType> ConverterMapping { get; set; }
        public NestedConverterMapping(TableRect rect, INestedTypeConverter<TTargetType> nestedTypeConverter)
        {
            this.Rect = rect;
            this.ConverterMapping = nestedTypeConverter;

        }

        public override string ToString()
        {
            return $"IndexToPropertyMapping (Range = {Rect}, PropertyMapping = {ConverterMapping}";
        }

        public bool TryConverValue(TableRect rect,ITable table, out TTargetType value)
        {
            TableRect relrect = rect.GetRelativeCoordinateRect(Rect);
            return ConverterMapping.TryConvert(relrect,table.GetTable(relrect), out value);
        }
    }
    public class NestedBaseTypeConverterMapping<TTargetType>: IConverterMapping<TTargetType>
    {
        public TableRect Rect { get; set; }

        public ITypeConverter<TTargetType> ConverterMapping { get; set; }

        public NestedBaseTypeConverterMapping(TableRect rect, ITypeConverter<TTargetType> converter)
        {
            this.Rect = rect;
            this.ConverterMapping = converter;
        }
        public override string ToString()
        {
            return $"IndexToPropertyMapping (Range = {Rect}, PropertyMapping = {ConverterMapping}";
        }


        public bool TryConverValue(TableRect rect,ITable table, out TTargetType value)
        {
            TableRect relrect = table.Rect.GetRelativeCoordinateRect(rect).GetRelativeCoordinateRect(Rect);
            return ConverterMapping.TryConvert(table.GetCellData(relrect),out value);
        }
    }
    public interface IConverterMapping<TTargetType>:IConverterMapping
    {
        bool TryConverValue(TableRect rect,ITable table,out TTargetType value);
    }
    public interface IConverterMapping { }
}

