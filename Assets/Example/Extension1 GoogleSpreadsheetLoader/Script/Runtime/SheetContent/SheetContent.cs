using System;
using System.Collections.Generic;
using DataTable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoogleSpreadsheetLoader
{
    [Serializable]
    public class SheetContent : SerializedScriptableObject
    {
        //private variable
        [TableList] [SerializeField] private List<DataUnit> _dataUnits;

        [Header("已匯入資料(Raw)")] private string[,] _rawData;


        //public variable
        public IReadOnlyList<IDataUnit> DataUnits => _dataUnits;


        //public method
        public void Update(string sheetName, string csv)
        {
            this.name = sheetName;
            
            //讀入 CSV 檔案，使其分為 string 二維陣列
            var csvParser = new CsvParser.CsvParser();
            var csvTable = csvParser.Parse(csv);

            var dataUnits = new List<DataUnit>();
            var labels = new List<string>();
            var usedLength = csvTable[0].Length;
            for (var i = 0; i < csvTable[0].Length; i++)
            {
                var key = csvTable[0][i];
                if (string.IsNullOrWhiteSpace(key))
                {
                    usedLength = i;
                    break;
                }

                labels.Add(key);
            }

            for (var i = 1; i < csvTable.Length; i++)
            {
                var dataValues = new List<DataValue>();

                for (var j = 0; j < usedLength; j++)
                {
                    var name = labels[j];
                    var value = csvTable[i][j];

                    var dataValue = new DataValue(name, value);
                    dataValues.Add(dataValue);
                }

                var key = csvTable[i][0];

                var dataUnit = new DataUnit(key, dataValues.ToArray());
                dataUnits.Add(dataUnit);
            }

            //Read Raw Data
            var rawData = new string[usedLength, csvTable.Length];
            for (var i = 0; i < csvTable.Length; i++)
            for (var j = 0; j < usedLength; j++)
                rawData[j, i] = csvTable[i][j];

            _dataUnits = dataUnits;
            _rawData = rawData;
        }
    }
}