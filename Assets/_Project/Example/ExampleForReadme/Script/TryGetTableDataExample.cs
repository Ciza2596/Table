using UnityEngine;

public class TryGetTableDataExample : MonoBehaviour
{
    [SerializeField]
    private CharacterDataUnitOverview _characterDataUnitOverview;
    private void Awake()
    {
        var dataUnits = _characterDataUnitOverview.DataUnits;
        var characterTable = new CharacterTable();
        
        characterTable.Initialize(dataUnits);

        characterTable.TryGetTableData("Player", out var playerTableData);
        Debug.Log($"Key: {playerTableData.Key}, Hp: {playerTableData.Hp}, Position: {playerTableData.Position}");
        
        characterTable.TryGetTableData("Gobin", out var gobinTableData);
        Debug.Log($"Key: {gobinTableData.Key}, Hp: {gobinTableData.Hp}, Position: {gobinTableData.Position}");
    }
}