
# IRef

IRef is a Unity-specific library designed to simplify the handling of references to objects that implement certain interfaces, particularly within the Unity Editor's Inspector window.

## Features

- **Type-Safe References** : `IRef` and `IRefList` act as wrapper classes for `UnityEngine.Object` and `List<UnityEngine.Object>`, respectively, allowing for type-safe references in Unity projects.

- **Inspector Integration** : Easily assign objects implementing specific interfaces through the Unity Inspector, with automatic validation to ensure the assigned objects meet the required interface implementation.

- **Dynamic List Management** : IRefList supports dynamic addition and removal of objects on runtime, with methods that ensure type safety and provide clear feedback on operation success or failure.

- **Error Handling** : Automatic nullification and warning messages in the Unity console if an object fails the interface implementation validation, including edge cases.


## Usage

#### Declaring IRef and IRefList

`IRef` and `IRefList` are designed to wrap `UnityEngine.Object` and `List<UnityEngine.Object>` references, respectively. They allow you to drag and drop GameObject, Monobehaviour Component or ScriptableObject instances that implement a specific interface into the Inspector window. The list inspector also supports the functionality of dragging and dropping multiple objects all at once.

```cs
[SerializeField] private IRef<MyInterface> myRef;
[SerializeField] private IRefList<MyInterface> myRefList;
```

<img src = "https://github.com/wmkimDev/IRef/assets/156675949/c8252cea-901b-4e3b-8640-89be6ffd80e7" width="500"> 

#### Accessing Internal Objects


To access the internal object or interface, you can use the provided properties that `IRef` and `IRefList` expose. Here's how you can declare and use them:

```cs
var myObject = myRef.Object; // Access the UnityEngine.Object
var myInterface = myRef.Interface; // Access the interface
```
```cs
List<UnityEngine.Object> myObjects = myRefList.Objects // Access the List of UnityEngine.Object
List<MyInterface> myInterfaces = myRefList.Interfaces; // Access the List of interface
MyInterface myInterface = myRefList[1];
```

## Inspector Validation
When assigning an object through the Inspector, IRef automatically validates if the object implements the required interface. If validation fails, it resets the reference to null and displays a warning message in the Unity console. This ensures that only compatible objects are assigned, preventing runtime errors.

several edge cases can lead to validation failures, including:

- The object does not implement the required interface.
- The gameObject does not have any component that implement the required interface.
- The gameObject has more than one component that implement the required interface.


## Contributing
Contributions to the IRef project are welcome! If you have suggestions for improvements, bug fixes, or new features, please feel free to submit an issue or pull request. 
