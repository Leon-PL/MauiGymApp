using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiGymApp.Calculations;
using MauiGymApp.Models;
using MauiGymApp.Models.DTOs.Measurables;
using Microsoft.Data.Analysis;
using UnitsNet;
using UnitsNet.Units;

namespace MauiGymApp.Data.Importing
{
    public static class FitNotesBodyImports
    {
        public static readonly Dictionary<string, Enum> FitNotesUnitAbbreviationUnitsNetUnitType = new Dictionary<string, Enum>()
        {
            {"in", LengthUnit.Inch },
            {"cm", LengthUnit.Centimeter },
            {"kgs", MassUnit.Kilogram },
        };

        public static readonly Dictionary<string, QuantityType> FitNotesUnitAbbreviationQuantityType = new Dictionary<string, QuantityType>()
        {
            {"in", QuantityType.Length },
            {"cm", QuantityType.Length },
            {"kgs", QuantityType.Mass },
        };


        public static DataFrame GetFitNotesDataFrame(string filePath = @"C:\Users\leonp\source\repos\MauiGymApp\MauiGymApp\Data\Importing\FitNotes_BodyTracker_Export.csv")
        {   
            var df = DataFrame.LoadCsv(filePath);

            PrimitiveDataFrameColumn<DateTime> dtCol = new("DateTime", df.Rows.Count);
            df.Columns.Add(dtCol);

            var dates = df["Date"].Cast<DateTime>().Select(i => i).ToList();
            var times = df["Time"].Cast<DateTime>().Select(i => i).ToList();

            for (int i = 0; i < df.Rows.Count; i++)
            {
                var date = dates[i].Date;
                var time = times[i].TimeOfDay;

                df["DateTime"][i] = date.Add(time);
            }

            df.Columns.Remove("Date");
            df.Columns.Remove("Time");

            return df;

        }

        public static Dictionary<string, QuantityType> GetMeasurableQuantityTypeFromFitNotes(DataFrame fitNotesDataFrame, IEnumerable<string> measurableQuantities)
        {
            Dictionary<string, QuantityType> types = [];

            foreach (string mq in measurableQuantities)
            {
                var filtered = fitNotesDataFrame.Filter(fitNotesDataFrame["Measurement"].ElementwiseEquals(mq));
                var unit = filtered["Unit"][0].ToString();

                types[mq] = FitNotesUnitAbbreviationQuantityType[unit];
            }
            return types;
        }

        public static Dictionary<string, DateTime> GetMeasurableQuantityCreationDateTimeFromFitNotes(DataFrame fitNotesDataFrame, IEnumerable<string> measurableQuantities)
        {
            // ToDo: Implement logic
            Dictionary<string, DateTime> types = [];

            foreach (string mq in measurableQuantities)
            {   
                types[mq] = DateTime.UtcNow;
            }

            return types;
        }


        public static IEnumerable<string> GetMeasurmentQuantitesFromFitNotes(DataFrame fitNotesDataFrame)
        {
            return fitNotesDataFrame["Measurement"].Cast<string>().Select(x => x).Distinct().ToList();
        }

        public static IEnumerable<MeasurableQuantityDTO> ReadMeasurableQuantities(string filePath = @"C:\Users\leonp\source\repos\MauiGymApp\MauiGymApp\Data\Importing\FitNotes_BodyTracker_Export.csv")
        {
            var df = GetFitNotesDataFrame(filePath);

            var measurementQs = GetMeasurmentQuantitesFromFitNotes(df);
            var mqTypes = GetMeasurableQuantityTypeFromFitNotes(df, measurementQs);
            var creationDT = GetMeasurableQuantityCreationDateTimeFromFitNotes(df, measurementQs);

            List<MeasurableQuantityDTO> dTOs = new();
            foreach (string mq in measurementQs)
            {
                var dto = new MeasurableQuantityDTO()
                {
                    Name = mq,
                    QuantityType = mqTypes[mq],
                    DateCreated = creationDT[mq],
                    Goal = null
                };

                var mqDF = df.Filter(df["Measurement"].ElementwiseEquals(mq));
                for (int i = 0; i < mqDF.Rows.Count; i++)
                {
                    string unitAbbreviation = mqDF["Unit"][i].ToString();
                    var value = (float)mqDF["Value"][i];
                    IQuantity quantity = Quantity.From(value, FitNotesUnitAbbreviationUnitsNetUnitType[unitAbbreviation]);
                    var measurementDTO = new MeasurementDTO
                    {
                        DateCreated = (DateTime)mqDF["DateTime"][i],
                        DateTime = (DateTime)mqDF["DateTime"][i],
                        Image = null,
                        QuantityType = mqTypes[mq],
                        ValueSI = quantity.AsBaseUnit(),
                    };
                    dto.Measurements.Add(measurementDTO);
                }
                

                dTOs.Add(dto);
            }

            return dTOs;
        }
    }
}
