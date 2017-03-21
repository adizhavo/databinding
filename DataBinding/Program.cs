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
            // can be loaded from json or requested from a centralized data loader
            var control = new ControlData();
            control.controlType = "touch";
            control.supportedSensors = new string[]{"Accelerometer"};

            var appOrientation = new AppOrientationData();
            appOrientation.supported = new string[]{"LANDSCAPE"};
            appOrientation.locked = true;

            // Create the data binding and add data with default values
            var dataBinding = new DataBindingService()
                .AddData<bool>("app.settings.notifications", true)
                .AddData<bool>("app.settings.notifications.hasSound", true)
                .AddData<bool>("app.settings.sound.music", true)
                .AddData<bool>("app.settings.sound.FX", true)
                .AddData<ControlData>("app.control", control)
                .AddData<AppOrientationData>("app.settings.orientation", appOrientation);

            // Create the binding component and bind it to the data
            var gameComponent = new GameComponent();
            dataBinding
                .Bind<bool>("app.settings.sound.music", gameComponent)
                .Bind<bool>("app.settings.sound.FX", gameComponent)
                .Bind<AppOrientationData>("app.settings.orientation", gameComponent);

            // the component will get the default data when binded
            Console.WriteLine(gameComponent);

            // Lets change some data
            dataBinding.GetData<bool>("app.settings.sound.music").value = false;
            dataBinding.GetData<bool>("app.settings.sound.FX").value = false;

            var appOrientationData = dataBinding.GetData<AppOrientationData>("app.settings.orientation");
            appOrientationData.value.supported = new string[]{"LANDSCAPE", "PORTRAIT"};
            appOrientationData.value.locked = false;
            appOrientationData.NotifyComponents();

            // Lets see the state of the component
            Console.WriteLine(gameComponent);
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

    public class GameComponent : BindingComponent<bool>, BindingComponent<AppOrientationData>
    {
        private bool music;
        private bool fx;
        private AppOrientationData appOrientationData;

        public void OnValueChanged(string branch, bool value)
        {
            if (string.Equals("app.settings.sound.music", branch))
                music = value;
            else if (string.Equals("app.settings.sound.FX", branch))
                fx = value;

            // Change UI at will
        }

        public void OnValueChanged(string branch, AppOrientationData value)
        {
            appOrientationData = value;
            // Change UI or settings at will
        }

        public override string ToString()
        {
            return $"[GameComponent] music: {music}, fx: {fx}, {appOrientationData}";
        }
    }
    #endif
}
