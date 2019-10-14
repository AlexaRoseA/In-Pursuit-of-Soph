using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject player;
    private GameObject spawnPoint;
    public Vector3 pPosition;
    public Vector3 pDirection;
    public Vector3 cursorPosition;
    public Vector3 pAcceleration;
    public Vector3 pVelocity;
    private GameObject Arm;
    //Maybe take in a "temperance" value, starting angle
    private float temperance; //Will be taken from player
    private float accelRate;
    float maxSpeed;
    float startTime; //Time at object's creation
    public float elapsedTime; //Time since then
    float angleOfRotation; //This is used to determine the angle of the sprite as well as the pDirection vector which controls its physical rotation.
    public bool alive; //Determines when to play death animation
    //bool dieThisFrame; //Helps figure out timing on death animation
    private int currentFrame;
    float lifespan; //determines how long the projectile lasts based on temperance

    private Sprite two, three, four;

    // Start is called before the first frame update
    void Start()
    {
       
        alive = true;
        currentFrame = 0;
        lifespan = 8-temperance; 
        if(lifespan < 3)
        {
            lifespan = 3; //Makes sure that it doesn't last less than 3 seconds
        }
        two = Resources.Load<Sprite>("bullet_2");
        three = Resources.Load<Sprite>("bullet_3");
        four = Resources.Load<Sprite>("bullet_4");
    }

    void Awake()
    {
        Arm = GameObject.Find("arm");
        spawnPoint = GameObject.Find("Hand"); //An empty created so that the projectile can fire from a 
        Arm.GetComponent<MoveArm>().count++; //??
        temperance = Arm.GetComponent<MoveArm>().temperance; //Value grabbed from player
        startTime = Time.time; //Captures start time
        pPosition = spawnPoint.transform.position; //Sets position to the tip of the arm
        pDirection = new Vector3(1, 0, 0);
        pVelocity = new Vector3(0, 0, 0); //Starts off slower
        maxSpeed = (temperance)*.1f; //This should also vary with temperance
        angleOfRotation = 0;
        accelRate = .1f;
        //Makes the projectile harder to control at higher temperance values
        /*if(temperance >= 3)
        {
            accelRate = accelRate * .5f;
        }
        */
        Move();
        
    }

    // Update is called once per frame
    void Update()
    {    
        //Remove the projectile after a certain amount of time based on temperance
        if (elapsedTime > lifespan)
        {
            Die(currentFrame);
        }

        //If the projectile dies (either by remaining for more than 5 seconds or by colliding with something)
        if (!alive)
        {
            Die(currentFrame);
        }
        if (alive)
        {
            //Update Values for different vectors
            UpdateValues();
            //Projectile movement   
            Move();
        }
  
    }

    void FixedUpdate()
    {
        elapsedTime = (startTime - Time.time) * -1f; //Updates current time
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Arm.GetComponent<MoveArm>().count = 0;
            alive = false;
        }
        if(other.gameObject.tag == "Button")
        {
            Debug.Log("Colbutton");
            other.gameObject.GetComponent<PressButton>().isActive = true; //If the projectile collides with a button, turn the button on.
        }
        //Put other collisions here
    }


    void UpdateValues()
    {

        //Okay, so you still need to figure out why it continues to go +x even if I change mouse cursor

        CalcRotations();

        CalcMovement();

    }

            void CalcRotations()
            {
                float prevAngle = angleOfRotation;

                //Update position of mouse cursor
                cursorPosition = Input.mousePosition;
                Vector3 screenPositionOfProjectile = Camera.main.WorldToScreenPoint(pPosition);
                
                 Debug.Log(cursorPosition);
        //Update angleOfRotation (for sprite and physics) to face mouse cursor
            angleOfRotation = Mathf.Atan2(pDirection.y, pDirection.x) * Mathf.Rad2Deg;

        //Update direction to face mouse cursor. This changes actual physics rotation, not just sprite.
        //pDirection = Quaternion.Euler(0, 0, angleOfRotation - prevAngle) * pDirection; //Keep in mind this should make pdirection.z = 0 if the mouse cursor hasn't moved since the previous frame
            
            pDirection = cursorPosition - screenPositionOfProjectile;
            pDirection.Normalize();
            }

            void CalcMovement()
            {
                pAcceleration = accelRate * pDirection;

                pVelocity += pAcceleration;

                pVelocity = Vector3.ClampMagnitude(pVelocity, maxSpeed);

                pPosition += pVelocity;
            }

    void Move() 
    {
        //Rotates sprite
        transform.rotation = Quaternion.Euler(0, 0, angleOfRotation);
        //Updates position and physical rotation
        transform.position = pPosition;

    }

    void Die(int _currentFrame) //Underscore in front to differentiate from the field itself
    {
        currentFrame++;
        if (currentFrame == 10)
        {
            this.GetComponent<SpriteRenderer>().sprite = two;
        }
        if(currentFrame == 15)
        {
            this.GetComponent<SpriteRenderer>().sprite = three;
           
        }
        if (currentFrame == 20)
        {
            this.GetComponent<SpriteRenderer>().sprite = four;
        }
        if (currentFrame == 25)
        {
            Arm.GetComponent<MoveArm>().count = 0;
            Destroy(gameObject);
        }
    }

}
