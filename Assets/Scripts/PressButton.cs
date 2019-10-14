using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
    private Animator animation;
    public bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        animation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //If turned on, animate
        if(isActive)
        {
            animation.SetBool("isActive", true);
            //Debug.Log("Play animation");
        }
        
        
    }
}
