# Table
```
https://github.com/Ciza2596/Table.git?path=Assets/_Project/Table
```

## Info
Support type:
  1. Primitive: bool, double, enum, float, int, long, string
  2. Unity: Vector2, Vector2Int, Vector3, Vector3Int

module | Test |
--- | :---: |
Table| ✔️ |


## Setup

1 - **Definition Table**

Reference character sheet to definition CharacterTable.

<img src="Document/Image/CharacterSheet.png?"/>

```csharp
using CizaTable;
using UnityEngine.Scripting;

public class CharacterTable : Table<CharacterTable.Data>{

    public class Data : TableData
    {
        [Preserve]
        public Data(string key) : base(key){}

        public float Hp { get; private set; }

        public Vector2 Position { get; private set; }
    }

}
```

2 - **Definition IDataUnit**

```csharp
[CreateAssetMenu(fileName = "CharacterDataUnitOverview", menuName = "Ciza/Table/CharacterDataUnitOverview")]
public class CharacterDataUnitOverview : ScriptableObject
{
    [SerializeField] private DataUnitImp[] _dataUnitImps;
    public IDataUnit[] DataUnits => _dataUnitImps;
    
    [Serializable]
    private class DataUnitImp : IDataUnit
    {
        public string Key => _key;
        public IDataValue[] DataValues => _dataValueImps;

        [SerializeField] private string _key;
        [SerializeField] private DataValueImp[] _dataValueImps;
    }

    [Serializable]
    private class DataValueImp : IDataValue
    {
        public string Name => _name;
        public string ValueString => _valueString;

        [SerializeField] private string _name;
        [SerializeField] private string _valueString;
    }
}
```
<img src="Document/Image/CharacterDataUnitOverviewInspector.png?"/>

## Method

1 - **Initialize**

```csharp
public class InitializeExample : MonoBehaviour
{
    [SerializeField]
    private CharacterDataUnitOverview _characterDataUnitOverview;
    private void Awake()
    {
        var dataUnits = _characterDataUnitOverview.DataUnits;
        var characterTable = new CharacterTable();
        
        characterTable.Initialize(dataUnits);
    }
}
```

2 - **TryGetTableData**

```csharp
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
```

3 - **Release**

```csharp
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
```
