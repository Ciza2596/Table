using System;
using CizaTable;
using UnityEngine;

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