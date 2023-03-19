# DataTable [HasTest]
```
https://github.com/Ciza2596/DataTable.git?path=Assets/DataTable
```

## Info
Support type:
  1. Primitive: bool, double, enum, float, int, long, string
  2. Unity: Vector2, Vector2Int, Vector3, Vector3Int

module | Test |
--- | :---: |
DataTable| ✔️ |


## Method

1 - **Definition DataTable**

Reference character sheet to definition CharacterDataTable.

<img src="Document/Image/CharacterSheet.png?"/>

```csharp
public class CharacterDataTable : BaseDataTable<PlayerTableData>{}

public class CharacterTableData : BaseTableData
{
    public CharacterTableData(string key) : base(key){}

    public float Hp { get; private set; }

    public Vector2 Position { get; private set; }
}
```

## Editor
