# Tri Inspector [![Github license](https://img.shields.io/github/license/codewriter-packages/Tri-Inspector.svg?style=flat-square)](#) [![Unity 2020.3](https://img.shields.io/badge/Unity-2020.3+-2296F3.svg?style=flat-square)](#) ![GitHub package.json version](https://img.shields.io/github/package-json/v/codewriter-packages/Tri-Inspector?style=flat-square) [![openupm](https://img.shields.io/npm/v/com.codewriter.triinspector?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.codewriter.triinspector/)

_Advanced inspector attributes for Unity_

![Tri-Inspector-Demo](https://user-images.githubusercontent.com/26966368/168415510-819b4fc0-d5ea-4795-b67f-7bd20b2f6adb.png)

- [How to Install](#How-to-Install)
- [Attributes](#Attributes)
    - [Misc](#Misc)
    - [Validation](#Validation)
    - [Styling](#Styling)
    - [Collections](#Collections)
    - [Conditionals](#Conditionals)
    - [Buttons](#Buttons)
    - [Debug](#Debug)
    - [Groups](#Groups)
- [Customization](#Customization)
    - [Custom Drawers](#Custom-Drawers)
    - [Validators](#Validators)
    - [Property Processors](#Property-Processors)
- [Integrations](#Integrations)
    - [Odin Inspector](#Odin-Inspector)
    - [Odin Validator](#Odin-Validator)
- [License](#License)

## How to Install

Library distributed as git package ([How to install package from git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html))
<br>Git URL: `https://github.com/codewriter-packages/Tri-Inspector.git`

After installing the package, you need to unpack the `Installer.unitypackage` that comes with the package to fix compiler errors.

## Attributes

### Misc

#### ShowInInspector

Shows non-serialized property in the inspector.

![ShowInInspector](https://user-images.githubusercontent.com/26966368/168230693-a1a389a6-1a3b-4b94-b4b5-0764e88591f4.png)

```csharp
private float _field;

[ShowInInspector]
public float ReadOnlyProperty => _field;

[ShowInInspector]
public float EditableProperty
{
    get => _field;
    set => _field = value;
}
```

#### PropertyOrder

Changes property order in the inspector.

![PropertyOrder](https://user-images.githubusercontent.com/26966368/168231223-c6628a8d-0d0a-47c1-8850-dc4e789fa14f.png)

```csharp
public float first;

[PropertyOrder(0)]
public float second;
```

#### ReadOnly

Makes property non-editable in the inspector.

![ReadOnly](https://user-images.githubusercontent.com/26966368/168231817-948ef153-eb98-42fb-88ad-3e8d17925b43.png)

```csharp
[ReadOnly]
public Vector3 vec;
```

#### OnValueChanged

Invokes callback on property modification.

```csharp
[OnValueChanged(nameof(OnMaterialChanged))]
public Material mat; 

private void OnMaterialChanged()
{
    Debug.Log("Material changed!");
}
```

### Validation

Tri Inspector has some builtin validators such as `missing reference` and `type mismatch` error. Additionally you can mark out your code with validation attributes or even write own validators.

![Builtin-Validators](https://user-images.githubusercontent.com/26966368/168232996-04de69a5-91c2-45d8-89b9-627b498db2ce.png)

#### Required

![Required](https://user-images.githubusercontent.com/26966368/168233232-596535b4-bab8-462e-b5d8-7a1c090e5143.png)

```csharp
[Required]
public Material mat;
```

#### ValidateInput

![ValidateInput](https://user-images.githubusercontent.com/26966368/168233592-b4dcd4d4-88ec-4213-a2e5-667719feb0b8.png)

```csharp
[ValidateInput(nameof(ValidateTexture))]
public Texture tex;

private TriValidationResult ValidateTexture()
{
    if (tex == null) return TriValidationResult.Error("Tex is null");
    if (!tex.isReadable) return TriValidationResult.Warning("Tex must be readable");
    return TriValidationResult.Valid;
}

```

#### InfoBox

![InfoBox](https://user-images.githubusercontent.com/26966368/169318171-d1a02212-48f1-41d1-b0aa-e2e1b25df262.png)

```csharp
[Title("InfoBox Message Types")]
[InfoBox("Default info box")]
public int a;

[InfoBox("None info box", TriMessageType.None)]
public int b;

[InfoBox("Warning info box", TriMessageType.Warning)]
public int c;

[InfoBox("Error info box", TriMessageType.Error)]
public int d;

[InfoBox("$" + nameof(DynamicInfo), visibleIf: nameof(VisibleInEditMode))]
public Vector3 vec;

private string DynamicInfo => "Dynamic info box: " + DateTime.Now.ToLongTimeString();

private bool VisibleInEditMode => !Application.isPlaying;
```

### Styling

#### Title

![Title](https://user-images.githubusercontent.com/26966368/168528842-10ba070e-74ab-4377-8f33-7a55609494f4.png)

```csharp
[Title("My Title")]
public string val;

[Title("$" + nameof(_myTitleField))]
public Rect rect;

[Title("$" + nameof(MyTitleProperty))]
public Vector3 vec;

[Title("Button Title")]
[Button]
public void MyButton()
{
}

private string _myTitleField = "Serialized Title";

private string MyTitleProperty => DateTime.Now.ToLongTimeString();
```

#### HideLabel

![HideLabel](https://user-images.githubusercontent.com/26966368/168528934-353f2843-b6ea-4f4f-b56e-24e006eca6ae.png)

```csharp
[Title("Wide Vector")]
[HideLabel]
public Vector3 vector;

[Title("Wide String")]
[HideLabel]
public string str;
```

#### LabelText

![LabelText](https://user-images.githubusercontent.com/26966368/168529002-8fb17112-f74c-4535-b399-aefdb352f73a.png)

```csharp
[LabelText("Custom Label")]
public int val;

[LabelText("$" + nameof(DynamicLabel))]
public Vector3 vec;

public string DynamicLabel => DateTime.Now.ToShortTimeString();
```

#### LabelWidth

![LabelWidth](https://user-images.githubusercontent.com/26966368/168529051-c90bce09-92a7-4afd-b961-d19f03e826f3.png)

```csharp
public int defaultWidth;

[LabelWidth(40)]
public int thin;

[LabelWidth(300)]
public int customInspectorVeryLongPropertyName;
```

#### GUIColor

![GUIColor](https://user-images.githubusercontent.com/26966368/168529122-048cd964-358c-453b-ab3a-aa7137bab4f7.png)

```csharp
[GUIColor(0.8f, 1.0f, 0.6f)]
public Vector3 vec;

[GUIColor(0.6f, 0.9f, 1.0f)]
[Button]
public void BlueButton() { }

[GUIColor(1.0f, 0.6f, 0.6f)]
[Button]
public void RedButton() { }
```

#### Indent

![Indent](https://user-images.githubusercontent.com/26966368/168528565-2972221d-2cb3-49f1-8000-a425e4ff6cea.png)

```csharp
[Title("Custom Indent")]
[Indent]
public int a;

[Indent(2)]
public int b;

[Indent(3)]
public int c;

[Indent(4)]
public int d;
```

#### PropertySpace

![PropertySpace](https://user-images.githubusercontent.com/26966368/168529641-ee61c950-cb15-4a4e-986b-c9fa8c82dd4d.png)

```csharp
[Space, PropertyOrder(0)]
public Vector3 vecField;

[ShowInInspector, PropertyOrder(1)]
[PropertySpace(SpaceBefore = 10, SpaceAfter = 30)]
public Rect RectProperty { get; set; }

[PropertyOrder(2)]
public bool b;
```

#### PropertyTooltip

![PropertyTooltip](https://user-images.githubusercontent.com/26966368/168530124-95609470-a495-4eb3-9059-f6203ead995f.png)

```csharp
[PropertyTooltip("This is tooltip")]
public Rect rect;

[PropertyTooltip("$" + nameof(DynamicTooltip))]
public Vector3 vec;

public string DynamicTooltip => DateTime.Now.ToShortTimeString();
```

#### InlineEditor

![InlineEditor](https://user-images.githubusercontent.com/26966368/168234617-86a7f500-e635-46f8-90f2-5696e5ae7e63.png)

```csharp
[InlineEditor]
public Material mat;
```

#### InlineProperty

![InlineProperty](https://user-images.githubusercontent.com/26966368/168234909-1e6bec90-18ed-4d56-91ca-fe09118e1b72.png)

```csharp
public MinMax rangeFoldout;

[InlineProperty(LabelWidth = 40)]
public MinMax rangeInline;

[Serializable]
public class MinMax
{
    public int min;
    public int max;
}
```

### Collections

#### ListDrawerSettings

![ListDrawerSettings](https://user-images.githubusercontent.com/26966368/168235372-1a460037-672c-424f-b2f0-6bf4641c0119.png)

```csharp
[ListDrawerSettings(Draggable = true,
                    HideAddButton = false,
                    HideRemoveButton = false,
                    AlwaysExpanded = false)]
public List<Material> list;
```

#### TableList

![TableList](https://user-images.githubusercontent.com/26966368/171021981-e0aa5d4b-96b2-40dd-96b3-3cd6b3af01e3.png)

```csharp
[TableList(Draggable = true,
           HideAddButton = false,
           HideRemoveButton = false,
           AlwaysExpanded = false)]
public List<TableItem> table;

[Serializable]
public class TableItem
{
    public Texture icon;
    public string description;

    [Group("Combined"), LabelWidth(16)]
    public string A, B, C;

    [Button, Group("Actions")]
    public void Test1() { }

    [Button, Group("Actions")]
    public void Test2() { }
}
```

### Conditionals

#### ShowIf

![ShowIf](https://user-images.githubusercontent.com/26966368/168531065-af5dad6a-8aea-4ca9-9730-da5feac0099a.png)

```csharp
public Material material;
public bool toggle;
public SomeEnum someEnum;

[ShowIf(nameof(material), null)]
public Vector3 showWhenMaterialIsNull;

[ShowIf(nameof(toggle))]
public Vector3 showWhenToggleIsTrue;

[ShowIf(nameof(toggle), false)]
public Vector3 showWhenToggleIsFalse;

[ShowIf(nameof(someEnum), SomeEnum.Two)]
public Vector3 showWhenSomeEnumIsTwo;

public enum SomeEnum { One, Two, Three }
```

#### HideIf

```csharp
public bool visible;

[HideIf(nameof(visible))]
public float val;
```

#### EnableIf

```csharp
public bool visible;

[EnableIf(nameof(visible))]
public float val;
```

#### DisableIf

```csharp
public bool visible;

[DisableIf(nameof(visible))]
public float val;
```

#### HideInPlayMode / ShowInPlayMode

```csharp
[HideInPlayMode] [ShowInPlayMode]
```

#### DisableInPlayMode / EnableInPlayMode

```csharp
[DisableInPlayMode] [EnableInPlayMode]
```

#### HideInEditMode / ShowInEditMode

```csharp
[HideInEditMode] [ShowInEditMode]
```

#### DisableInEditMode / EnableInEditMode

```csharp
[DisableInEditMode] [EnableInEditMode]
```

### Buttons

#### Button

![Button](https://user-images.githubusercontent.com/26966368/168235907-2b5ed6d4-d00b-4cd6-999c-432abd0a2230.png)

```csharp
[Button("Click me!")]
private void DoButton()
{
    Debug.Log("Button clicked!");
}
```

#### EnumToggleButtons

![EnumToggleButtons](https://user-images.githubusercontent.com/26966368/170864400-fe76d356-2a80-479a-90de-bac9619bd7d1.png)

```csharp
[EnumToggleButtons] public SomeEnum someEnum;
[EnumToggleButtons] public SomeFlags someFlags;

public enum SomeEnum { One, Two, Three }

[Flags] public enum SomeFlags
{
    A = 1 << 0,
    B = 1 << 1,
    C = 1 << 2,
    AB = A | B,
    BC = B | C,
}
```

### Debug

#### ShowDrawerChain

![ShowDrawerChain](https://user-images.githubusercontent.com/26966368/168531723-5f2b2d7a-a4c1-4727-8ab5-e7b82a52182e.png)

```csharp
[ShowDrawerChain]
[Indent]
[PropertySpace]
[Title("Custom Title")]
[GUIColor(1.0f, 0.8f, 0.8f)]
public Vector3 vec;
```

### Groups

![Groups](https://user-images.githubusercontent.com/26966368/168236396-b28eba4a-7fe7-4a5c-b185-55fabf1aabf5.png)

```csharp
[DeclareHorizontalGroup("header")]
[DeclareBoxGroup("header/left", Title = "My Left Box")]
[DeclareVerticalGroup("header/right")]
[DeclareBoxGroup("header/right/top", Title = "My Right Box")]
[DeclareTabGroup("header/right/tabs")]
[DeclareBoxGroup("body")]
public class GroupDemo : MonoBehaviour
{
    [Group("header/left")] public bool prop1;
    [Group("header/left")] public int prop2;
    [Group("header/left")] public string prop3;
    [Group("header/left")] public Vector3 prop4;

    [Group("header/right/top")] public string rightProp;

    [Group("body")] public string body1;
    [Group("body")] public string body2;

    [Group("header/right/tabs"), Tab("One")] public float tabOne;
    [Group("header/right/tabs"), Tab("Two")] public float tabTwo;
    [Group("header/right/tabs"), Tab("Three")] public float tabThree;

    [Group("header/right"), Button("Click me!")]
    public void MyButton()
    {
    }
}
```

### Customization

#### Custom Drawers

<details>
  <summary>Custom Value Drawer</summary>

```csharp
using TriInspector;
using UnityEditor;
using UnityEngine;

[assembly: RegisterTriValueDrawer(typeof(BoolDrawer), TriDrawerOrder.Fallback)]

public class BoolDrawer : TriValueDrawer<bool>
{
    public override float GetHeight(float width, TriValue<bool> propertyValue, TriElement next)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, TriValue<bool> propertyValue, TriElement next)
    {
        var value = propertyValue.Value;

        EditorGUI.BeginChangeCheck();

        value = EditorGUI.Toggle(position, propertyValue.Property.DisplayNameContent, value);

        if (EditorGUI.EndChangeCheck())
        {
            propertyValue.Value = value;
        }
    }
}
```

</details>

<details>
  <summary>Custom Attribute Drawer</summary>

```csharp
using TriInspector;
using UnityEditor;
using UnityEngine;

[assembly: RegisterTriAttributeDrawer(typeof(LabelWidthDrawer), TriDrawerOrder.Decorator)]

public class LabelWidthDrawer : TriAttributeDrawer<LabelWidthAttribute>
{
    public override void OnGUI(Rect position, TriProperty property, TriElement next)
    {
        var oldLabelWidth = EditorGUIUtility.labelWidth;

        EditorGUIUtility.labelWidth = Attribute.Width;
        next.OnGUI(position);
        EditorGUIUtility.labelWidth = oldLabelWidth;
    }
}
```

</details>

<details>
  <summary>Custom Group Drawer</summary>

```csharp
using TriInspector;
using TriInspector.Elements;

[assembly: RegisterTriGroupDrawer(typeof(TriBoxGroupDrawer))]

public class TriBoxGroupDrawer : TriGroupDrawer<DeclareBoxGroupAttribute>
{
    public override TriPropertyCollectionBaseElement CreateElement(DeclareBoxGroupAttribute attribute)
    {
        // ...
    }
}
```

</details>

#### Validators

<details>
  <summary>Custom Value Validator</summary>

```csharp
using TriInspector;

[assembly: RegisterTriValueValidator(typeof(MissingReferenceValidator<>))]

public class MissingReferenceValidator<T> : TriValueValidator<T>
    where T : UnityEngine.Object
{
    public override TriValidationResult Validate(TriValue<T> propertyValue)
    {
        // ...
    }
}
```

</details>

<details>
  <summary>Custom Attribute Validators</summary>

```csharp
using TriInspector;

[assembly: RegisterTriAttributeValidator(typeof(RequiredValidator), ApplyOnArrayElement = true)]

public class RequiredValidator : TriAttributeValidator<RequiredAttribute>
{
    public override TriValidationResult Validate(TriProperty property)
    {
        // ...
    }
}
```

</details>

#### Property Processors

<details>
  <summary>Custom Property Hide Processor</summary>

```csharp
using TriInspector;
using UnityEngine;

[assembly: RegisterTriPropertyHideProcessor(typeof(HideInPlayModeProcessor))]

public class HideInPlayModeProcessor : TriPropertyHideProcessor<HideInPlayModeAttribute>
{
    public override bool IsHidden(TriProperty property)
    {
        return Application.isPlaying;
    }
}
```

</details>

<details>
  <summary>Custom Property Disable Processor</summary>

```csharp
using TriInspector;
using UnityEngine;

[assembly: RegisterTriPropertyDisableProcessor(typeof(DisableInPlayModeProcessor))]

public class DisableInPlayModeProcessor : TriPropertyDisableProcessor<DisableInPlayModeAttribute>
{
    public override bool IsDisabled(TriProperty property)
    {
        return Application.isPlaying;
    }
}
```

</details>

## Integrations

### Odin Inspector

Tri Inspector is able to work in compatibility mode with Odin Inspector. 
In this mode, the primary interface will be drawn by the Odin Inspector. However, 
parts of the interface can be rendered by the Tri Inspector.

In order for the interface to be rendered by Tri instead of Odin, 
it is necessary to mark classes with `[DrawWithTriInspector]` attribute.

Alternatively, you can mark the entire assembly with an attribute `[assembly:DrawWithTriInspector]`
to draw all types in the assembly using the Tri Inspector.

### Odin Validator

Tri Inspector is integrated with the Odin Validator
so all validation results from Tri attributes will be shown 
in the Odin Validator window.

![Odin-Validator-Integration](https://user-images.githubusercontent.com/26966368/169645537-d8f0b50f-46af-4804-95e8-337ff3b5ae83.png)

## License

Tri-Inspector is [MIT licensed](./LICENSE.md).