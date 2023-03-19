using UnityEngine;

public class ReleaseExample : MonoBehaviour
{
    [SerializeField]
    private CharacterDataUnitOverview _characterDataUnitOverview;
    private void Awake()
    {
        var dataUnits = _characterDataUnitOverview.DataUnits;
        var characterDataTable = new CharacterDataTable();
        
        characterDataTable.Initialize(dataUnits);
        
        characterDataTable.Release();
    }
}