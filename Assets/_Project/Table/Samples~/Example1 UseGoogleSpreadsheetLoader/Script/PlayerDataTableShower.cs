using System.Linq;
using GoogleSpreadsheetLoader;
using UnityEngine;

namespace CizaTable.Example1
{
	public class PlayerDataTableShower : MonoBehaviour
	{
		[SerializeField]
		private string _playerKey;

		[SerializeField]
		private SheetContent _sheetContent;

		private readonly PlayerTable _playerTable = new PlayerTable();

		private void Awake()
		{
			var dataUnits = _sheetContent.DataUnits.ToArray();
			_playerTable.Initialize(dataUnits);
		}

		private void OnEnable()
		{
			if (_playerTable.TryGetTableData(_playerKey, out var playerTableData))
			{
				Debug.Log($"Key: {playerTableData.Key},\nHp1_1: {playerTableData.Hp1_1},\nMp1_1: {playerTableData.Mp1_1}\nPosition: {playerTableData.Position}");
			}
		}
	}
}
