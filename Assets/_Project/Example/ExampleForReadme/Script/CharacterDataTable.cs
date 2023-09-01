using CizaDataTable;
using UnityEngine;

public class CharacterDataTable : DataTable<CharacterTableData>{}

public class CharacterTableData : TableData
{
    public CharacterTableData(string key) : base(key){}

    public float Hp { get; private set; }

    public Vector2 Position { get; private set; }
}