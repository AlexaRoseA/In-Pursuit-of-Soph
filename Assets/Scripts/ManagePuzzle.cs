using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePuzzle : MonoBehaviour
{
    /*
     * NOTE: LIST AND ARRAY MUST BE THE SAME SIZE IN INSPECTOR
     * */
    public List<GameObject> puzzleObjectsList; //Stores the game objects
    int itemNumber;
    public bool[] puzzleObjectStatesToCheck; //Holds all the bools that correspond to each GameObject
    int correctStates; //Keeps track of how many object-state pairs are correct. 
    public GameObject obstacle; //This "obstacle" is usually a door that opens on completion of puzzle.
    private GameObject Arm; //Takes the arm so we can reset its temperance upon puzzle completion

    // Start is called before the first frame update
    void Start()
    {
        itemNumber = puzzleObjectsList.Count; //Stores the number of Game Objects put in the list
        puzzleObjectStatesToCheck = new bool[itemNumber]; 
    }

    void Awake()
    {
        Arm = GameObject.Find("arm");
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckPuzzle())
        {
            obstacle.GetComponent<DoorScript>().open = true; //Changes state of the obstacle so that if it's a door, it changes to its "open" state.
            Arm.GetComponent<MoveArm>().temperance = 1; //Resets player temperance on completion of puzzle
        } 
    }
    bool CheckPuzzle()
    {
        correctStates = 0; //Reset before each check
        for(int i = 0; i<itemNumber; i++)
        {
            //FOR MODULAR USE: There was a bug where it didn't work with testing true/false. So you need to check the isActive bool against the puzzleObjectStatestoCheck bool, for some reason using == didn't work.
            if(puzzleObjectsList[i].GetComponent<PressButton>().isActive==true /*&& puzzleObjectStatesToCheck[i]*/) //Checks if the state of the actual button is the same as the state it needs to be in the puzzle manager for player to complete puzzle
            {
                correctStates++; //Keeps track of how many out of the total items you have correct
                Debug.Log(correctStates);
            }
        }
        if (correctStates == itemNumber) //Be careful if itemNumber and correctStates don't add up correctly. Like if item number is one less because of the way it counts
        {
            //Debug.Log("Puzzle complete!");
            return true; //Returns true, the puzzle is complete!
        }
        return false;
    }
}
