using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator animation;
    public bool open;
    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animator>();  //Look inside of Door's animator component
    }

    // Update is called once per frame
    void Update()
    {
        if(open)
        {
            //Show open state
            animation.SetBool("open", true);

        }
        else if(!open)
        {
            //Show closed state
            animation.SetBool("open", false);
        }
    }
}
