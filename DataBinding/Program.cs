using System;
using System.Collections.Generic;

namespace DataBinding
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // setup the json loader, can be extended to another implementation through the interface
            IJsonFileLoader jsonLoader = new AppDomainJsonLoader();
            string dataBindConfig = jsonLoader.GetJsonFrom("Resources/data_bindings.json");

            // create data type map payload since we want to load custom data structures
            Dictionary<string, Type> dataTypeMapPayload = new Dictionary<string, Type>()
            {
                {"ControlData",         typeof(ControlData)},
                {"AppOrientationData",  typeof(AppOrientationData)}
            };
            DataBinderService dataBinder = new DataBinderService();
            dataBinder.AddTypeMap(dataTypeMapPayload);

            // setup the data biding from its configuration file
            // the deserializer uses Newtonsoft Json to parse the data, can be extended to another implementation through the interface
            IDataBindDeserializer deserializer = new NewtonsoftDataBindDeserializer(dataBindConfig, dataBinder, jsonLoader);
            dataBinder.Initialize(deserializer);

            Console.Write(dataBinder.ToString());
        }
    }

    public class ControlData
    {
        public string controlType;
        public string[] supportedSensors;

        public override string ToString()
        {
            return $"control type: {controlType}, supportedSensors: {supportedSensors.Length}";
        }
    }

    public class AppOrientationData
    {
        public string[] supported;
        public bool locked;

        public override string ToString()
        {
            return $"supported: {supported.Length}, locked: {locked}";
        }
    }
}
