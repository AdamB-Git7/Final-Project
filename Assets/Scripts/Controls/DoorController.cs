using UnityEngine;

// =====================================================================
//  DOOR CONTROLLER — handles one office door
// =====================================================================
//  How it works:
//    - Player presses the door's key (Q for left, E for right)
//    - The door panel slides UP (opening) or back DOWN (closing)
//    - When closed, a NavMeshObstacle blocks the AI from passing through
// =====================================================================
public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public KeyCode toggleKey = KeyCode.Q;  // which key opens/closes this door
    public float doorSpeed = 5f;           // how fast the door slides

    [Header("State")]
    public bool isClosed = true;           // current state (visible in Inspector)

    [Header("References")]
    // NavMeshObstacle = blocks the AI's pathfinding. When enabled it
    // "carves" a hole in the NavMesh so the AI must walk around
    public UnityEngine.AI.NavMeshObstacle doorBlocker;

    // Internal — tracks the door panel's position
    Transform doorPanel;     // the visual door mesh
    Vector3 closedPos;       // resting (down) position
    Vector3 openPos;         // raised position

    // =================================================================
    //  Start() — runs once when the door spawns
    // =================================================================
    void Start()
    {
        // Find the child GameObject named "DoorPanel" (the visible door)
        doorPanel = transform.Find("DoorPanel");

        if (doorPanel != null)
        {
            // Remember its starting position (closed = down)
            closedPos = doorPanel.localPosition;
            // Open position = 2.5 units higher (slides up out of the way)
            openPos = closedPos + new Vector3(0, 2.5f, 0);
        }

        // Sync the obstacle: enabled when closed, disabled when open
        if (doorBlocker != null)
            doorBlocker.enabled = isClosed;
    }

    // =================================================================
    //  Update() — runs every frame
    // =================================================================
    void Update()
    {
        // Did the player press the toggle key this frame?
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleDoor();
        }

        // Smoothly slide the door panel toward its target position
        if (doorPanel != null)
        {
            // Pick the target based on state (closed = closedPos, open = openPos)
            Vector3 target = isClosed ? closedPos : openPos;

            // Lerp = linear interpolation. Move a fraction toward target each frame.
            // Time.deltaTime * doorSpeed makes it speed-independent of frame rate.
            doorPanel.localPosition = Vector3.Lerp(doorPanel.localPosition, target, Time.deltaTime * doorSpeed);
        }
    }

    // =================================================================
    //  ToggleDoor() — flip the door state (closed ↔ open)
    //  Called by player input AND by other scripts
    // =================================================================
    public void ToggleDoor()
    {
        isClosed = !isClosed;  // flip the bool

        // Enable or disable the AI blocker to match
        if (doorBlocker != null)
            doorBlocker.enabled = isClosed;

        // Play the door slam sound effect
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayDoorClose();
    }

    // =================================================================
    //  CloseDoor() — force-close (used by AI logic if needed)
    // =================================================================
    public void CloseDoor()
    {
        isClosed = true;
        if (doorBlocker != null)
            doorBlocker.enabled = true;
    }

    // =================================================================
    //  OpenDoor() — force-open (the AI calls this when breaking in)
    // =================================================================
    public void OpenDoor()
    {
        isClosed = false;
        if (doorBlocker != null)
            doorBlocker.enabled = false;
    }
}
