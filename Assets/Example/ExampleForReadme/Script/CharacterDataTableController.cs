using UnityEngine;

public class CharacterDataTableController : MonoBehaviour
{
    [SerializeField]
    private CharacterDataUnitOverview _characterDataUnitOverview;
    private CharacterDataTable _characterDataTable = new CharacterDataTable();
    
    private void Awake()
    {
        var dataUnits = _characterDataUnitOverview.DataUnits;
        _characterDataTable.Initialize(dataUnits);

        _characterDataTable.TryGetTableData("Player", out var playerTableData);
        Debug.Log($"Key: {playerTableData.Key}, Hp: {playerTableData.Hp}, Position: {playerTableData.Position}");
        
        _characterDataTable.TryGetTableData("Gobin", out var gobinTableData);
        Debug.Log($"Key: {gobinTableData.Key}, Hp: {gobinTableData.Hp}, Position: {gobinTableData.Position}");
    }
}
