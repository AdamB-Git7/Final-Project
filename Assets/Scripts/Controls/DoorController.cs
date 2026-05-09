
// Imports the UnityEngine namespace.
using UnityEngine;










// Declares the class named DoorController.
public class DoorController : MonoBehaviour

// Opens a new code block.
{

    // Applies the Header("Door Settings") attribute.
    [Header("Door Settings")]

    // Declares the variable toggleKey and initializes it.
    public KeyCode toggleKey = KeyCode.Q;

    // Declares the variable doorSpeed and initializes it.
    public float doorSpeed = 5f;


    // Applies the Header("State") attribute.
    [Header("State")]

    // Declares the variable isClosed and initializes it.
    public bool isClosed = true;


    // Applies the Header("References") attribute.
    [Header("References")]



    // Executes this statement.
    public UnityEngine.AI.NavMeshObstacle doorBlocker;



    // Declares the variable doorPanel.
    Transform doorPanel;

    // Declares the variable closedPos.
    Vector3 closedPos;

    // Declares the variable openPos.
    Vector3 openPos;





    // Declares the method named Start.
    void Start()

    // Opens a new code block.
    {


        // Updates an existing value.
        doorPanel = transform.Find("DoorPanel");


        // Checks whether the condition is true.
        if (doorPanel != null)

        // Opens a new code block.
        {


            // Updates an existing value.
            closedPos = doorPanel.localPosition;


            // Updates an existing value.
            openPos = closedPos + new Vector3(0, 2.5f, 0);

        // Closes the current code block.
        }



        // Checks whether the condition is true.
        if (doorBlocker != null)

            // Updates an existing value.
            doorBlocker.enabled = isClosed;

    // Closes the current code block.
    }





    // Declares the method named Update.
    void Update()

    // Opens a new code block.
    {


        // Checks whether the condition is true.
        if (Input.GetKeyDown(toggleKey))

        // Opens a new code block.
        {

            // Calls a method.
            ToggleDoor();

        // Closes the current code block.
        }



        // Checks whether the condition is true.
        if (doorPanel != null)

        // Opens a new code block.
        {


            // Declares the variable target and initializes it.
            Vector3 target = isClosed ? closedPos : openPos;




            // Updates an existing value.
            doorPanel.localPosition = Vector3.Lerp(doorPanel.localPosition, target, Time.deltaTime * doorSpeed);

        // Closes the current code block.
        }

    // Closes the current code block.
    }






    // Declares the method named ToggleDoor.
    public void ToggleDoor()

    // Opens a new code block.
    {

        // Updates an existing value.
        isClosed = !isClosed;



        // Checks whether the condition is true.
        if (doorBlocker != null)

            // Updates an existing value.
            doorBlocker.enabled = isClosed;



        // Checks whether the condition is true.
        if (AudioManager.Instance != null)

            // Calls a method.
            AudioManager.Instance.PlayDoorClose();

    // Closes the current code block.
    }





    // Declares the method named CloseDoor.
    public void CloseDoor()

    // Opens a new code block.
    {

        // Updates an existing value.
        isClosed = true;

        // Checks whether the condition is true.
        if (doorBlocker != null)

            // Updates an existing value.
            doorBlocker.enabled = true;

    // Closes the current code block.
    }





    // Declares the method named OpenDoor.
    public void OpenDoor()

    // Opens a new code block.
    {

        // Updates an existing value.
        isClosed = false;

        // Checks whether the condition is true.
        if (doorBlocker != null)

            // Updates an existing value.
            doorBlocker.enabled = false;

    // Closes the current code block.
    }

// Closes the current code block.
}
