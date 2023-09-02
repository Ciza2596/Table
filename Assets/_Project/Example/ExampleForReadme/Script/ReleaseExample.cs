using UnityEngine;

public class ReleaseExample : MonoBehaviour
{
    [SerializeField]
    private CharacterDataUnitOverview _characterDataUnitOverview;
    private void Awake()
    {
        var dataUnits = _characterDataUnitOverview.DataUnits;
        var characterTable = new CharacterTable();
        
        characterTable.Initialize(dataUnits);
        
        characterTable.Release();
    }
}