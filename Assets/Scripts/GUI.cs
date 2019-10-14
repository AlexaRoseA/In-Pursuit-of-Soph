using UnityEngine;
using UnityEditor;
using System.Collections;

public class GUI : MonoBehaviour
{
    // Draws a texture on the screen at 10, 10 with 100 width, 100 height.

    
    public Texture Inventory;
    public Texture Controls;

    private void Awake()
    {

    }


    void OnGUI()
    {
       Graphics.DrawTexture(new Rect(0, Screen.height - 175, 335, 175), Inventory);
       Graphics.DrawTexture(new Rect(Screen.width - 315, Screen.height - 178, 335, 175), Controls);
    }
}
