# Databinding Service
Databinding library that can be used to set application data and bind them to events when they change. Really useful for a heavy data oriented system.

## Data on a tree!

<p align="center">
  <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/c/cd/N-ary_to_binary.svg/400px-N-ary_to_binary.svg.png"/>
</p>

Each node represent a data in this service, so for example the branch "app.settings.audio.music" corresponds to the branch "A.B.H.N" with a nodeId of "music" and depth of 3.

The dot (.) is considered as a node separator in the system, can be changed to any char [here](https://github.com/adizhavo/databinding/blob/master/DataBinding/DataBindingService.cs)

## How to use it

To start the databinding service simply create an instance of it
```C#
using DataBinding;

var dataBinding = new DataBindingService();
```

#### Add data

Call the method ```AddData``` by providing the data type and the branch to hold it, new nodes will be created if nodes of the branch don't exists.

```C#
dataBinding.AddData<bool>("app.settings.notifications", true)
           .AddData<bool>("app.settings.notifications.hasSound", true)
           .AddData<bool>("app.settings.sound.music", true);
```

#### Binding components

These components bind to a data branch and get notified if the data changes in the system.
Implement the ```BindingComponent<T>``` interface by the specifying the data type which it will bind to.

```C#
public class SampleBindingComponent : BindingComponent<bool>
{
    public void OnValueChanged(string branch, bool value)
    {
        // Do stuff with the new data
    }
}
```

#### Binding data to components

Call the method ```Bind<T>(branch, instance)``` by specifying the data type, the data branch and the binding component instance.

```C#
var sampleBindingComponent = new SampleBindingComponent();

dataBinding.Bind<bool>("app.settings.sound.music", sampleBindingComponent);
```

Is possible to bind the same instance to different branches with the same or different data type.

#### Get and modify data

Call the method ```GetData<T>(string branch)``` to get an object of type ```Data<T>``` and access the its value.

```C#
var musicData = dataBinding.GetData<bool>("app.settings.sound.music");
var isMusicEnabled = musicData.value;

// Change data and reassign
isMusicEnabled = !isMusicEnabled;
musicData.value = isMusicEnabled;
```

Reassigning the value this way will notify automatically the component which are bind to this data.
To manually notify components, on the ```Data<T>``` object call the ```NotifyComponents``` method.
```C#
musicData.NotifyComponents()
```
#### Tests

To better understand the system and have a quick dive in the codebase have a look at the [unit tests](https://github.com/adizhavo/databinding/tree/master/DataBindingTest) in the project.

#### Useful methods

- ```GetData<T>(string nodeId, int treeDepth)``` if you know the node and the data depth in the tree
- ```ContainsNode(string branch)``` 
- ```ContainsNode(string nodeId, int treeDepth)``` 
- ```ExtractNode(string branch)``` will return a [node object](https://github.com/adizhavo/databinding/blob/master/DataBinding/Data.cs)
- ```ExtractNode(string nodeId, int treeDepth)```


