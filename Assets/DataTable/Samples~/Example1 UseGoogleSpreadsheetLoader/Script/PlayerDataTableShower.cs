using System.Linq;
using GoogleSpreadsheetLoader;
using UnityEngine;

namespace DataTable.Example1
{
    public class PlayerDataTableShower : MonoBehaviour
    {
        [SerializeField] private string _playerKey;
        [SerializeField] private SheetContent _sheetContent;
        
        
        private PlayerDataTable _playerDataTable = new PlayerDataTable();
        
        private void Awake()
        {
            var dataUnits = _sheetContent.DataUnits.ToArray();
            _playerDataTable.Initialize(dataUnits);
        }
        
        private void OnEnable()
        {
            if (_playerDataTable.TryGetTableData(_playerKey, out var playerTableData))
            {
                Debug.Log(
                    $"Key: {playerTableData.Key},\nHp1_1: {playerTableData.Hp1_1},\nMp1_1: {playerTableData.Mp1_1}\nPosition: {playerTableData.Position}");
            }
        }
    }
}