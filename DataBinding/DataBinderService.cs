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

        private List<DataTypeMap> dataTypeMap = new List<DataTypeMap>();
        private List<PseudoData> defaultData = new List<PseudoData>();

        private bool initialized = false;

        public DataBinderService AddTypeMap(Dictionary<string, Type> payload)
        {
            if (initialized)
            {
                Console.WriteLine("[DataBinderService] System already initialized and cannot add payloads, please do this before the service setups");
                return this;
            }

            foreach (var payloadPart in payload)
                if (!typeMap.ContainsKey(payloadPart.Key))
                    typeMap.Add(payloadPart.Key, payloadPart.Value);
            return this;
        }

        public void Initialize(IDataBindDeserializer deserializer)
        {
            dataTypeMap = deserializer.DeserializeDataTypeMap();
            defaultData = deserializer.DeserializeDefaultData();

            initialized = true;
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