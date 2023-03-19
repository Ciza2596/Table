using CizaDataTable;
using UnityEngine;

public class CharacterDataTable : BaseDataTable<CharacterTableData>{}

public class CharacterTableData : BaseTableData
{
    public CharacterTableData(string key) : base(key){}

    public float Hp { get; private set; }

    public Vector2 Position { get; private set; }
}