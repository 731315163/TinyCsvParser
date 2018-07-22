﻿// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Test.Benchmarks
{

    [TestFixture]
    public class LocalWeatherDataBenchmark
    {
        public class LocalWeatherData
        {
            public string WBAN { get; set; }
            public DateTime Date { get; set; }
            public string SkyCondition { get; set; }
        }

        public class LocalWeatherDataMapper : CsvMapping<LocalWeatherData>
        {
            public LocalWeatherDataMapper()
            {
                MapProperty(0, x => x.WBAN);
                MapProperty(1, x => x.Date, new DateTimeConverter("yyyyMMdd"));
                MapProperty(4, x => x.SkyCondition);
            }
        }

        [Test, Ignore("Performance Test for a Sequential Read")]
        public void SeqReadTest()
        {
            MeasurementUtils.MeasureElapsedTime(string.Format("Sequential Read"), () =>
            {
                var a = File.ReadLines(@"C:\Users\philipp\Downloads\csv\201503hourly.txt", Encoding.ASCII)
                    .AsParallel()
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Trim().Split(new[] { ';' })).ToList();
            });
        }


        [Test]
        public void LocalWeatherReadTest()
        {
            var dialect = new Dialect()
            {
                Name = "Unit Test",
                QuoteChar = '"',
                Delimiter = ',',
                DoubleQuote = true,
                EscapeChar = '\\',
                SkipInitialSpace = true,
                Quoting = QuoteStyleEnum.QUOTE_ALL,
                Strict = true
            };

            bool[] keepOrder = new bool[] { true, false };
            int[] degreeOfParallelismList = new[] { 4, 3, 2, 1 };

            foreach (var order in keepOrder)
            {
                foreach (var degreeOfParallelism in degreeOfParallelismList)
                {
                    CsvParserOptions csvParserOptions = new CsvParserOptions(dialect, true, degreeOfParallelism, order);
                    LocalWeatherDataMapper csvMapper = new LocalWeatherDataMapper();
                    CsvParser<LocalWeatherData> csvParser = new CsvParser<LocalWeatherData>(csvParserOptions, csvMapper);

                    MeasurementUtils.MeasureElapsedTime(string.Format("LocalWeather (DegreeOfParallelism = {0}, KeepOrder = {1})", degreeOfParallelism, order),
                        () =>
                        {

                            using (var stream = new StreamReader(
                                stream: File.OpenRead(@"D:\datasets\201503hourly.txt"),
                                detectEncodingFromByteOrderMarks: false,
                                encoding: Encoding.ASCII))
                            {
                                foreach (var element in csvParser.Parse(stream))
                                {

                                }
                            }

                        });
                }
            }
        }
    }
}