using System;
using System.Collections.Generic;
using TinyCsvParser.Model;
using TinyCsvParser.Ranges;
using TinyCsvParser.TypeConverter;

//namespace TinyCsvParser.Mapping
//{
//    public class NestedMapping<TEntity>: INestedCsvMapping<TEntity>
//    {   
       
//        //mapping
//        private readonly ITypeConverterProvider typeConverterProvider;
//        //private readonly List<IndexToPropertyMapping<TEntity>> csvPropertyMappings;
//        private readonly List<RangeToPropertyMapping<TEntity>> csvPropertyMappings;
//        protected Func<TEntity> newObject;
//        /// <summary>
//        ///  csv convert table数据
//        /// </summary>
//        protected ITableTree tableContext;
        
//        protected NestedMapping()
//            : this(new TypeConverterProvider())
//        { }
//        protected NestedMapping(ITableTree tableContext, ITypeConverterProvider provider,Func<TEntity> newObj)
//        {
//            this.tableContext = tableContext;
//            this.typeConverterProvider = provider;
//            this.newObject = newObj;
//        }
//        protected NestedMapping(ITableTree tableContext, ITypeConverterProvider provider)
//        {
//            this.typeConverterProvider = provider;
//            this.tableContext = tableContext;
//        }
//        protected NestedMapping(ITypeConverterProvider provider)
//        {
//            this.typeConverterProvider = provider;
//        }
//         //base type property mapping
//         protected NestedBaseTypePropertyMapping<TEntity, TProperty> MapProperty<TProperty>(int columnIndex, Action<TEntity,TProperty> setproperty)
//        {
//            return MapProperty<TProperty>(columnIndex,setproperty,typeConverterProvider.Resolve<TProperty>());
//        }
//        // nested mapping
       
       
//        protected NestedBaseTypePropertyMapping<TEntity, TProperty> MapProperty<TProperty>(int columnIndex,Action<TEntity, TProperty> property, ITypeConverter<TProperty> typeConverter)
//        {
           
//            ThrowInvalidOperationException(columnIndex);
//            RangeDefinition range = new RangeDefinition(columnIndex, columnIndex);
//            var propertyMapping = new NestedBaseTypePropertyMapping<TEntity, TProperty>(range,property, typeConverter);

//            AddPropertyMapping(range, propertyMapping);

//            return propertyMapping;
//        }
//        protected void ThrowInvalidOperationException(int columnIndex)
//        {
//            for (var i = 0; i < csvPropertyMappings.Count; i++)
//            {
//                var x = csvPropertyMappings[i];
                
//                if (x.Range.Start > columnIndex && x.Range.End < columnIndex)
//                {
//                    throw new InvalidOperationException(string.Format("Duplicate mapping for column index {0}",
//                        columnIndex));
//                }
//            }
           
//        }
      
//        public void MapProperty<TProperty>(int columnIndex,Action<TEntity, ITable> property)
//        {
//            Action<TEntity,string> propertySetter = (e, tablekey) =>
//            {
//                var csvtable = tableContext.GetTable(tablekey);
//                property(e, csvtable);
//            };
//            var propertyMapping = new NestedPropertyMapping<TEntity>(propertySetter);

//            AddPropertyMapping(columnIndex, propertyMapping);
            
//        }
//        private void AddPropertyMapping<TProperty>(int start,int end, NestedBaseTypePropertyMapping<TEntity, TProperty> propertyMapping)
//        {
//            var indexToPropertyMapping = new IndexToPropertyMapping<TEntity>
//            {
//                ColumnIndex = columnIndex,
//                PropertyMapping = propertyMapping
//            };

//            csvPropertyMappings.Add(indexToPropertyMapping);
//        }
//        private void AddPropertyMapping(int start,int end, NestedCsvPropertyMapping<TEntity> propertyMapping)
//        {
//            var toPropertyMapping = new RangeToPropertyMapping<TEntity>
//            {
//                Range = new Ranges.RangeDefinition(start,end),
//                PropertyMapping = propertyMapping
//            };

//            csvPropertyMappings.Add(indexToPropertyMapping);
//        }
      
//        public CsvMappingResult<TEntity> Map(TokenizedRow values)
//        {
//            TEntity entity = newObject();

//            for (int pos = 0; pos < csvPropertyMappings.Count; pos++)
//            {
//                var indexToPropertyMapping = csvPropertyMappings[pos];

//                var columnIndex = indexToPropertyMapping.ColumnIndex;

//                if (columnIndex >= values.Tokens.Length)
//                {
//                    return new CsvMappingResult<TEntity>()
//                    {
//                        RowIndex = values.Index,
//                        Error = new CsvMappingError()
//                        {
//                            ColumnIndex = columnIndex,
//                            Value = string.Format("Column {0} is Out Of Range", columnIndex)
//                        }
//                    };
//                }

//                var value = values.Tokens[columnIndex];

//                if (!indexToPropertyMapping.PropertyMapping.TryMapValue(entity, value))
//                {
//                    return new CsvMappingResult<TEntity>()
//                    {
//                        RowIndex = values.Index,
//                        Error = new CsvMappingError
//                        {
//                            ColumnIndex = columnIndex,
//                            Value = string.Format("Column {0} with Value '{1}' cannot be converted", columnIndex, value)
//                        }
//                    };
//                }
//            }

//            return new CsvMappingResult<TEntity>()
//            {
//                RowIndex = values.Index,
//                Result = entity
//            };
//        }
//        public CsvMappingResult<TEntity> Map(ArraySegment<string> values)
//        {
//            TEntity entity = newObject();
//            for (int i = 0; i < csvPropertyMappings.Count; ++i)
//            { var value = values.Array[values.Offset + i];
//                if (!csvPropertyMappings[i].PropertyMapping.TryMapValue(entity, value))
//                {
//                    return new CsvMappingResult<TEntity>()
//                    {
//                        RowIndex = values.Offset + i,
//                        Error = new CsvMappingError
//                        {
                            
//                            Value = string.Format("Row{0}  with Value '{1}' cannot be converted", values.Offset + i,value)
//                        }
//                    };
//                }
//            }
            
//            return new CsvMappingResult<TEntity>()
//            {
//                Result = entity
//            };
//        }

     
//    }
//}
