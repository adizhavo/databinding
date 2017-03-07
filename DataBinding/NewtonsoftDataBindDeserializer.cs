using System
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DataBinding
{
    public class NewtonsoftDataBindDeserializer : IDataBindDeserializer
    {
        private string json;

        public NewtonsoftDataBindDeserializer(string json)
        {
            this.json = json;
        }

        public List<DataTypeMap> DeserializeDataTypeMap()
        {
            List<DataTypeMap> dataTypeMap = new List<DataTypeMap>();

            JObject dataBindingConfig = JObject.Parse(json);
            JEnumerable<JToken> jsonDataTypeMap = dataBindingConfig["dataTypeMap"].Children();
            foreach (JToken jdtm in jsonDataTypeMap)
            {
                DataTypeMap dtm = JsonConvert.DeserializeObject<DataTypeMap>(jdtm.ToString());
                dataTypeMap.Add(dtm);
            }

            return dataTypeMap;
        }

        public List<PseudoData> DeserializeDefaultData()
        {
            List<PseudoData> defaultData = new List<PseudoData>();

            JObject defaultDataConfig = JObject.Parse(json);
            JEnumerable<JToken> defaultDataToken = defaultDataConfig["defaultData"].Children();
            foreach (JToken token in defaultDataToken)
            {
                JObject dataObject = JObject.Parse(token.ToString());
                PseudoData data = new PseudoData();
                data.branch = dataObject["branch"].ToString();

                try
                {
                    data.json = dataObject["json"].ToString();
                }
                catch (Exception e)
                {
                    #if DEBUG
                    Console.WriteLine($"\nbranch: {data.branch} doesn\'t contain a json path\n");
                    #endif
                }

                defaultData.Add(data);
            }

            return defaultData;
        }
    }
}