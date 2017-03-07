using System.Collections.Generic;

namespace DataBinding
{
    public interface IDataBindDeserializer
    {
        List<DataTypeMap> DeserializeDataTypeMap();
        List<object> DeserializeDefaultData();
    }
}