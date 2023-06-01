using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManager : MonoBehaviour
{
    public Material material;
    public SpriteRenderer shaderObject;

    private void Start()
    {
        material = shaderObject.material;
    }

    void PrintValues()
    {
        // Retrieve the property value from the material
        float valueToPrint = material.GetFloat("_Random");

        // Print out the value to the console
        Debug.Log("Value to print: " + valueToPrint);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 700, 40), "PrintValues"))
        {
            PrintValues();
        }

    }
}
