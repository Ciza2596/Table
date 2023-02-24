using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
    [Serializable]
    public class SheetContentInfo
    {
        //private variable
        [TableColumnWidth(200)] [VerticalGroup("ScriptableObject")] [ReadOnly] [SerializeField]
        private SheetContent _sheetContent;

        private GoogleSpreadsheetLoader _googleSpreadsheetLoader;

        private string _sheetInfoId;

        private bool _isBusy;

        private string _spreadSheetId;
        private string _sheetId;


        //constructor
        public SheetContentInfo(string sheetInfoId, SheetContent sheetContent,
            GoogleSpreadsheetLoader googleSpreadsheetLoader)
        {
            _sheetInfoId = sheetInfoId;
            _sheetContent = sheetContent;
            _googleSpreadsheetLoader = googleSpreadsheetLoader;
        }


        //public variable
        public string SheetInfoId => _sheetInfoId;


        public bool IsBusy => _isBusy;

        public string SpreadSheetId => _spreadSheetId;
        public string SheetId => _sheetId;


        //public method
        public void SetIsBusy(bool isBusy) =>
            _isBusy = isBusy;


        //private method
        [HorizontalGroup("動作")]
        [Button("更新")]
        [GUIColor(0, 1, 0)]
        [DisableIf("IsBusy")]
        public async Task Update()
        {
            try
            {
                await _googleSpreadsheetLoader.UpdateSheetContentInfo(this);
            }
            catch
            {
                _isBusy = false;
            }
        }

        public void Update(string sheetName, string csv)
        {
            CreateDataUnitsAndRawData(csv, out var dataUnits, out var rawData);
            _sheetContent.Update(sheetName, dataUnits.ToArray(), rawData);
        }

        [HorizontalGroup("動作")]
        [GUIColor(1, 0, 0)]
        [Button("移除")]
        [DisableIf("IsBusy")]
        public void Remove()
        {
            var subSheetContent = _sheetContent;
            _sheetContent = null;

            var assetPath = AssetDatabase.GetAssetPath(subSheetContent);
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"[SubSheetContentInfo::Remove] Remove content file : {assetPath}.");
        }

        private void CreateDataUnitsAndRawData(string csv, out List<DataUnit> dataUnits, out string[,] rawData)
        {
            //讀入 CSV 檔案，使其分為 string 二維陣列
            var csvParser = new CsvParser.CsvParser();
            var csvTable = csvParser.Parse(csv);

            dataUnits = new List<DataUnit>();
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
            rawData = new string[usedLength, csvTable.Length];
            for (var i = 0; i < csvTable.Length; i++)
            for (var j = 0; j < usedLength; j++)
                rawData[j, i] = csvTable[i][j];
        }
    }
}