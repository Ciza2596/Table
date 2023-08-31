
using UnityEngine;

namespace CizaDataTable.Example1
{
    public class PlayerDataTable : BaseDataTable<PlayerTableData>
    {
    }

    public class PlayerTableData : BaseTableData
    {
        public PlayerTableData(string key) : base(key)
        {
        }

        public float Hp1_1 { get; private set; }

        public float Mp1_1 { get; private set; }

        public Vector2 Position { get; private set; }
    }
}
