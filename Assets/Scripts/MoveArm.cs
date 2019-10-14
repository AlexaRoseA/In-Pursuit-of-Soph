using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArm : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 mouse_pos;
    Transform target; //Assign to the object you want to rotate
    Vector3 object_pos;
    [SerializeField]
    public GameObject player;
    float angle;
    public float temperance; //Game mechanic float that keeps track of how many times you've used the projectile. Set to 0 every time you change room?

    [SerializeField]
    GameObject projectile;
    public Sprite armReg, armF;
    [SerializeField]
    GameObject point;

    public int count;
 
    void Start()
    {
        armReg = Resources.Load<Sprite>("arm");
        armF= Resources.Load<Sprite>("f");
        temperance = 1;
    }

    /// <summary>
    /// Spawns ONE projectile at a time. 
    /// </summary>
    void SpawnProjectile()
    {
        if (Input.GetKeyDown(KeyCode.Q) && projectile != null && count <= 0)
        {
            //Debug.Log("BOOM!");
            count++;
            Instantiate(projectile);
            temperance+=.25f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SpawnProjectile();
        //rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        //Debug.Log(transform.rotation);
        this.GetComponent<SpriteRenderer>().sprite = armReg;

        //if facing right
        if (player.GetComponent<CharacterController2D>().dirRight && !Input.GetKey(KeyCode.F))
        {
            if (angle >= 90)
            {
                transform.localScale = new Vector3(1, -1, 1);
            }

            if (angle >= -90 && angle < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        } 

        if (!player.GetComponent<CharacterController2D>().dirRight)
        {
            //Debug.Log(angle);
            if (angle >= -60 && angle < 85)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            } else
            {
                transform.localScale = new Vector3(-1, -1, 1);
            }
        }

        if (Input.GetKey(KeyCode.F))
        {
            this.GetComponent<SpriteRenderer>().sprite = armF;

            if (player.GetComponent<CharacterController2D>().dirRight)
            {
                if (angle >= 90)
                {
                    transform.localScale = new Vector3(1, -1, 1);
                }

                if (angle >= -90 && angle < 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
            
            if (!player.GetComponent<CharacterController2D>().dirRight)
            {
                Debug.Log(angle);
                if (angle >= -60 && angle < 85)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, -1, 1);
                }
            }
        }
    }
}
