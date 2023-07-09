using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    [BoxGroup("Settings")]
    public int health;
    [BoxGroup("Settings")]
    public int speed;


    [Foldout("Stats")]
    public int health1;
    [Foldout("Stats")]
    public int speed1;

    [MinMaxSlider(0, 100), BoxGroup("Settings")]

    public Vector2 minMaxValue = new Vector2(25, 75);
    [ShowAssetPreview]
    public GameObject myPrefab;

    [HorizontalLine]


    [Dropdown("GetDropdownValues")]
    public string dropdownValue;

    private string[] GetDropdownValues()
    {
        return new string[] { "Option 1", "Option 2", "Option 3" };
    }

    [HorizontalLine]


    [ResizableTextArea]
    public string myText;


    [ValidateInput("ValidateMyVariable")]
    public int MoreThan0;

    private bool ValidateMyVariable(int value)
    {
        return value > 0; // Only allow values greater than 0
    }

    [HorizontalLine]

    [OnValueChanged("OnMyVariableChanged")]
    public int onChangeVariable;

    private void OnMyVariableChanged()
    {
        Debug.Log("Value changed to: " + onChangeVariable);
    }

    [ReadOnly]
    public string justRead = "This is a weeping song";

    [Required]
    public GameObject myRequiredObject;

    [HorizontalLine]

    [Scene]
    public string myScene;
    [Tag]
    public string myTag;
    [Layer]
    public int myLayer;

    [HorizontalLine]

    [EnumFlags]
    public MyEnum myEnumFlags;

    [System.Flags]
    public enum MyEnum
    {
        None = 0,
        Option1 = 1 << 0,
        Option2 = 1 << 1,
        Option3 = 1 << 2
    }

    [ShowNativeProperty]
    public int NativeProperty { get { return 10; } }

    [ShowNonSerializedField]
    private int myNonSerializedField = 10;


    [HorizontalLine]

    [ProgressBar("Health", 100)]
    public int Health = 50;


    [HorizontalLine]

    [MinValue(0), MaxValue(100)]
    public int clampedValue0_100;

    [HorizontalLine]

    public bool showField;
    [ShowIf("showField")]
    public int showMe = 12;

    public bool enableField;
    [EnableIf("enableField")]
    public int enableMe;

    public int myVariable;

    private void MySpecialCaseDrawer(string label, ref int value)
    {
        value = EditorGUILayout.IntSlider(label, value, 0, 100);
    }

    [Button]
    public void PrintSa()
    {

    }
    
}
