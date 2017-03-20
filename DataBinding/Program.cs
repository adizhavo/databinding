#define DATABINDING_MAIN
using System;

namespace DataBinding
{
    #if DATABINDING_MAIN
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Create sample data
            // can be loaded from json or requested from a 
            // centralized data loader
            var control = new ControlData();
            control.controlType = "touch";
            control.supportedSensors = new string[]{"Accelerometer"};

            var appOrientation = new AppOrientationData();
            appOrientation.supported = new string[]{"LANDSCAPE"};
            appOrientation.locked = true;

            // Create the data binding and add data with dedault values
            var dataBinding = new DataBindingService()
                .AddDataNode<bool>("app.settings.notifications", true)
                .AddDataNode<bool>("app.settings.notifications.hasSound", true)
                .AddDataNode<bool>("app.settings.sound.music", true)
                .AddDataNode<bool>("app.settings.sound.FX", true)
                .AddDataNode<ControlData>("app.control", control)
                .AddDataNode<AppOrientationData>("app.settings.orientation", appOrientation);

            Console.WriteLine(dataBinding.ToString());
            Console.WriteLine("End of program");
        }
    }

    public class ControlData
    {
        public string controlType;
        public string[] supportedSensors;

        public override string ToString()
        {
            return $"{base.ToString()}, control type: {controlType}, supportedSensors: {supportedSensors.Length}";
        }
    }

    public class AppOrientationData
    {
        public string[] supported;
        public bool locked;

        public override string ToString()
        {
            return $"{base.ToString()}, supported: {supported.Length}, locked: {locked}";
        }
    }
    #endif
}
