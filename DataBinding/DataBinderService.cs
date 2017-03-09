using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace DataBinding
{
    public class DataBinderService
    {
        private readonly Dictionary<string, Type> typeMap = new Dictionary<string, Type>()
        {
            {"int", typeof(int)},
            {"bool", typeof(bool)},
            {"float", typeof(float)},
            {"string", typeof(string)},
            {"double", typeof(double)}
        };

        private List<DataTypeMap> dataTypeMap = new List<DataTypeMap>();
        private List<object> data = new List<object>();

        private bool initialized;

        public DataBinderService AddTypeMap(Dictionary<string, Type> payload)
        {
            if (initialized)
            {
                Console.WriteLine("[DataBinderService] System already initialized and cannot add payloads, please do this before the initialization.");
                return this;
            }

            foreach (var payloadPart in payload)
                if (!typeMap.ContainsKey(payloadPart.Key))
                    typeMap.Add(payloadPart.Key, payloadPart.Value);
            return this;
        }

        public void Initialize(IDataBindDeserializer deserializer)
        {
            if (initialized)
                return;

            LogState(0);
            dataTypeMap = deserializer.DeserializeDataTypeMap();
            LogState(1);
            data = deserializer.DeserializeDefaultData();
            LogState(2);
            // set up the rest of the data
            LogState(3);
            initialized = true;
        }

        public Type GetDataType(string branch)
        {
            if (dataTypeMap == null || dataTypeMap.Count == 0)
                throw new Exception("[DataBinderService] the data type map is not initialized or empty, please set up the configuration json");

            foreach (var dtm in dataTypeMap)
                if (dtm.branch.Equals(branch))
                {
                    if (typeMap.ContainsKey(dtm.type))
                    {
                        return typeMap.First(p => typeMap.Comparer.Equals(p.Key, dtm.type)).Value;
                    }

                    throw new Exception($"[DataBinderService] the requested branch: {branch} doesn\'t have the data type defined");
                }

            Console.WriteLine($"the branch {branch} is not configured in the data bind service");
            return null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("dataTypeMap:\n");

            foreach (var dtm in dataTypeMap)
                sb.Append(dtm);

            sb.Append("\ndefaultData:\n");

            foreach (var d in data)
                sb.Append(d);

            return sb.ToString();
        }

        private void LogState(int step)
        {
            #if DEBUG
            switch (step)
            {
                case 0:
                    Console.WriteLine("[DataBinderService] starting loading the data type map.");
                    break;
                case 1:
                    Console.WriteLine("[DataBinderService] data type map loaded.");
                    Console.WriteLine("[DataBinderService] starting loading the default data.");
                    break;
                case 2:
                    Console.WriteLine("[DataBinderService] default data loaded.");
                    Console.WriteLine("[DataBinderService] settings up the remaining data.");
                    break;
                case 3:
                    Console.WriteLine("[DataBinderService] the service is ready to go.");
                    break;
            }
            #endif
        }
    }
}