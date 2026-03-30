using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public KeyCode toggleKey = KeyCode.Q;
    public float doorSpeed = 5f;

    [Header("State")]
    public bool isClosed = true;

    [Header("References")]
    public UnityEngine.AI.NavMeshObstacle doorBlocker;

    Transform doorPanel;
    Vector3 closedPos;
    Vector3 openPos;

    void Start()
    {
        doorPanel = transform.Find("DoorPanel");

        if (doorPanel != null)
        {
            closedPos = doorPanel.localPosition;
            openPos = closedPos + new Vector3(0, 2.5f, 0); // slides up
        }

        if (doorBlocker != null)
            doorBlocker.enabled = isClosed;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleDoor();
        }

        if (doorPanel != null)
        {
            Vector3 target = isClosed ? closedPos : openPos;
            doorPanel.localPosition = Vector3.Lerp(doorPanel.localPosition, target, Time.deltaTime * doorSpeed);
        }
    }

    public void ToggleDoor()
    {
        isClosed = !isClosed;

        if (doorBlocker != null)
            doorBlocker.enabled = isClosed;
    }

    public void CloseDoor()
    {
        isClosed = true;
        if (doorBlocker != null)
            doorBlocker.enabled = true;
    }

    public void OpenDoor()
    {
        isClosed = false;
        if (doorBlocker != null)
            doorBlocker.enabled = false;
    }
}
