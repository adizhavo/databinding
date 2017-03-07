using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DataBinding
{
    public class NewtonsoftDataBindDeserializer : IDataBindDeserializer
    {
        private readonly string json;
        private readonly DataBinderService dataBinder;
        private readonly IJsonFileLoader jsonFileLoader;

        public NewtonsoftDataBindDeserializer(string json, DataBinderService dataBinder, IJsonFileLoader jsonFileLoader)
        {
            this.json = json;
            this.dataBinder = dataBinder;
            this.jsonFileLoader = jsonFileLoader;
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
                #if DEBUG
                Console.WriteLine($"[NewtonsoftDataBindDeserializer] add data type map: {dtm}");
                #endif
            }

            return dataTypeMap;
        }

        public List<object> DeserializeDefaultData()
        {
            List<object> data = new List<object>();

            JObject defaultDataConfig = JObject.Parse(json);
            JEnumerable<JToken> defaultDataToken = defaultDataConfig["defaultData"].Children();
            foreach (var token in defaultDataToken)
            {
                JObject dataObject = JObject.Parse(token.ToString());
                string branch = dataObject["branch"].ToString();
                Type dataType = dataBinder.GetDataType(branch);

                if (dataType != null)
                {
                    object dataPiece = null;
                    Type genericDataType = typeof(Data<>).MakeGenericType(dataType);

                    try
                    {
                        string jsonPath = dataObject["json"].ToString();
                        dataPiece = JsonConvert.DeserializeObject(jsonFileLoader.GetJsonFrom(jsonPath), genericDataType);
                        ((Branch) dataPiece).branch = branch;
                    }
                    catch (Exception e)
                    {
                        #if DEBUG
                        Console.WriteLine($"[NewtonsoftDataBindDeserializer] branch: {branch} doesn\'t contain a json path, will deserialize based on the type");
                        #endif
                        dataPiece = JsonConvert.DeserializeObject(token.ToString(), genericDataType);
                    }

                    data.Add(dataPiece);
                }
            }

            return data;
        }
    }
}