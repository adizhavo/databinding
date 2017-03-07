using System;
using System.Text;
using System.Collections.Generic;

namespace DataBinding
{
    public class DataBinderService
    {
        private Dictionary<string, Type> typeMap = new Dictionary<string, Type>()
        {
            {"int", typeof(int)},
            {"bool", typeof(bool)},
            {"float", typeof(float)},
            {"string", typeof(string)},
            {"double", typeof(double)}
        };

        public List<DataTypeMap> dataTypeMap = new List<DataTypeMap>();
        public List<PseudoData> defaultData = new List<PseudoData>();

        public DataBinderService AddTypeMap(Dictionary<string, Type> payload)
        {
            foreach (var payloadPart in payload)
                if (!typeMap.ContainsKey(payloadPart.Key))
                    typeMap.Add(payloadPart.Key, payloadPart.Value);
            return this;
        }

        public void Setup(IDataBindDeserializer deserializer)
        {
            dataTypeMap = deserializer.DeserializeDataTypeMap();
            defaultData = deserializer.DeserializeDefaultData();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("dataTypeMap:\n");

            foreach (var dtm in dataTypeMap)
                sb.Append(dtm.ToString() + "\n");

            sb.Append("defaultData:\n");

            foreach (var df in defaultData)
                sb.Append(df.ToString() + "\n");

            return sb.ToString();
        }
    }
}