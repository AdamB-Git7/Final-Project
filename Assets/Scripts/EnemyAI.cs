using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Navigation Points")]
    public Transform leftDoorPoint;
    public Transform rightDoorPoint;
    public Transform centerPoint;

    [Header("Movement Settings")]
    public float moveSpeed = 3.5f;
    public float doorWaitTime = 1.5f;
    public float centerWaitTime = 3f;

    [Header("References")]
    public GameManager gameManager;
    public DoorController leftDoor;
    public DoorController rightDoor;

    [Header("Office Target")]
    public Transform officeTarget;

    private const float ArrivalThreshold = 1.5f;
    private const float OfficeArrivalThreshold = 2f;
    private const float StuckCheckInterval = 2f;
    private const float StuckMoveThreshold = 0.1f;
    private const float NavMeshSampleRadius = 4f;
    private const int MaxDoorRetries = 2;

    NavMeshAgent agent;
    float timer;
    float stuckTimer;
    int stuckDoorRetries;
    Vector3 lastPosition;
    bool gameEnded;

    enum State { WalkingToCenter, WaitingAtCenter, WalkingToDoor, WaitingAtDoor, Retreating, EnteringOffice }
    State state;
    bool goingLeft;

    Vector3 OfficeCenter => officeTarget != null
        ? officeTarget.position
        : new Vector3(0, 0.5f, -5f);

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        gameEnded = false;
        timer = 0f;
        stuckTimer = 0f;
        stuckDoorRetries = 0;
        lastPosition = transform.position;
        state = State.WalkingToCenter;

        if (centerPoint != null)
            SetDestinationSafe(centerPoint.position);
    }

    void Update()
    {
        if (gameEnded) return;
        if (gameManager != null && !gameManager.IsGameActive()) return;

        CheckIfStuck();

        switch (state)
        {
            case State.WalkingToCenter:
                if (HasArrived(centerPoint.position))
                {
                    agent.isStopped = true;
                    state = State.WaitingAtCenter;
                    timer = 0f;
                }
                break;

            case State.WaitingAtCenter:
                timer += Time.deltaTime;
                if (timer >= centerWaitTime)
                    ChooseDoor();
                break;

            case State.WalkingToDoor:
                Vector3 doorPos = goingLeft ? leftDoorPoint.position : rightDoorPoint.position;
                if (HasArrived(doorPos))
                {
                    agent.isStopped = true;
                    state = State.WaitingAtDoor;
                    timer = 0f;
                }
                break;

            case State.WaitingAtDoor:
                DoorController doorToCheck = goingLeft ? leftDoor : rightDoor;

                if (doorToCheck == null)
                {
                    Retreat();
                    break;
                }

                bool isOpen = !doorToCheck.isClosed;

                if (isOpen)
                {
                    timer += Time.deltaTime;
                    if (timer >= doorWaitTime)
                    {
                        state = State.EnteringOffice;
                        agent.isStopped = false;
                        agent.speed = moveSpeed * 1.5f;
                        SetDestinationSafe(OfficeCenter);
                    }
                }
                else
                {
                    Retreat();
                }
                break;

            case State.EnteringOffice:
                if (HasArrivedOffice(OfficeCenter))
                {
                    gameEnded = true;
                    agent.isStopped = true;

                    Camera cam = Camera.main;
                    if (cam != null)
                    {
                        Vector3 dir = cam.transform.position - transform.position;
                        dir.y = 0;
                        if (dir != Vector3.zero)
                            transform.rotation = Quaternion.LookRotation(dir);
                    }

                    if (gameManager != null)
                        gameManager.TriggerGameOver();
                }
                break;

            case State.Retreating:
                if (HasArrived(centerPoint.position))
                {
                    agent.isStopped = true;
                    state = State.WaitingAtCenter;
                    timer = 0f;
                }
                break;
        }
    }

    /// <summary>
    /// Detects if the agent hasn't moved while it should be moving, and reissues the destination.
    /// </summary>
    void CheckIfStuck()
    {
        bool shouldBeMoving = state == State.WalkingToCenter
            || state == State.WalkingToDoor
            || state == State.Retreating
            || state == State.EnteringOffice;

        if (!shouldBeMoving)
        {
            stuckTimer = 0f;
            lastPosition = transform.position;
            return;
        }

        stuckTimer += Time.deltaTime;
        if (stuckTimer >= StuckCheckInterval)
        {
            if (Vector3.Distance(transform.position, lastPosition) < StuckMoveThreshold)
                RefreshDestination();

            lastPosition = transform.position;
            stuckTimer = 0f;
        }
    }

    /// <summary>
    /// Reissues the current state's destination to un-stick the agent.
    /// If stuck at a door too many times, switches to the other door.
    /// </summary>
    void RefreshDestination()
    {
        switch (state)
        {
            case State.WalkingToCenter:
            case State.Retreating:
                SetDestinationSafe(centerPoint.position);
                break;
            case State.WalkingToDoor:
                stuckDoorRetries++;
                if (stuckDoorRetries >= MaxDoorRetries)
                {
                    // This door side seems unreachable — flip to the other
                    stuckDoorRetries = 0;
                    goingLeft = !goingLeft;
                }
                SetDestinationSafe(goingLeft ? leftDoorPoint.position : rightDoorPoint.position);
                break;
            case State.EnteringOffice:
                SetDestinationSafe(OfficeCenter);
                break;
        }
    }

    /// <summary>
    /// Sets the agent destination. Falls back to nearest NavMesh point if target is off-mesh.
    /// </summary>
    bool SetDestinationSafe(Vector3 target)
    {
        agent.isStopped = false;
        // Try floor-level first (Y=0.5) in case target is above the NavMesh
        Vector3 floorTarget = new Vector3(target.x, 0.5f, target.z);
        if (NavMesh.SamplePosition(floorTarget, out NavMeshHit hit, NavMeshSampleRadius, NavMesh.AllAreas))
            return agent.SetDestination(hit.position);

        return agent.SetDestination(target);
    }

    /// <summary>
    /// Returns true when the agent has reached the target position.
    /// </summary>
    bool HasArrived(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) < ArrivalThreshold)
            return true;

        if (!agent.pathPending && agent.hasPath && agent.remainingDistance <= agent.stoppingDistance + 0.5f)
            return true;

        return false;
    }

    /// <summary>
    /// Looser arrival check used when entering the office — desk geometry can block the last few units.
    /// </summary>
    bool HasArrivedOffice(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) < OfficeArrivalThreshold)
            return true;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.5f)
            return true;

        // Path could not be completed — trigger game over anyway
        if (!agent.pathPending && (agent.pathStatus == NavMeshPathStatus.PathInvalid
            || agent.pathStatus == NavMeshPathStatus.PathPartial))
            return true;

        return false;
    }

    void ChooseDoor()
    {
        // Track how many times each door has been blocked
        bool leftBlocked = leftDoor != null && leftDoor.isClosed;
        bool rightBlocked = rightDoor != null && rightDoor.isClosed;

        if (leftBlocked && rightBlocked)
        {
            // Both closed — wait longer at center and try again
            timer = -centerWaitTime * 0.5f;
            state = State.WaitingAtCenter;
            return;
        }

        if (leftBlocked)
        {
            // Left is closed — always go right
            goingLeft = false;
        }
        else if (rightBlocked)
        {
            // Right is closed — always go left
            goingLeft = true;
        }
        else
        {
            // Both open — avoid the door we just came from, with a bias toward it
            // to keep the player guessing (30% chance to retry same side)
            bool avoidLast = Random.value > 0.3f;
            if (avoidLast)
                goingLeft = !goingLeft; // switch sides
            // else keep goingLeft as-is (feint)
        }

        Transform target = goingLeft ? leftDoorPoint : rightDoorPoint;
        agent.speed = moveSpeed;
        SetDestinationSafe(target.position);
        state = State.WalkingToDoor;
        stuckDoorRetries = 0;
    }

    void Retreat()
    {
        // Check if the other door is open — if so, go straight there instead of returning to center
        bool otherDoorOpen = goingLeft
            ? (rightDoor != null && !rightDoor.isClosed)
            : (leftDoor != null && !leftDoor.isClosed);

        if (otherDoorOpen)
        {
            goingLeft = !goingLeft;
            Transform target = goingLeft ? leftDoorPoint : rightDoorPoint;
            agent.speed = moveSpeed;
            SetDestinationSafe(target.position);
            state = State.WalkingToDoor;
        }
        else
        {
            state = State.Retreating;
            agent.speed = moveSpeed;
            SetDestinationSafe(centerPoint.position);
        }
    }

    public void SetDifficulty(int night)
    {
        switch (night)
        {
            case 1:
                moveSpeed = 2.5f;
                doorWaitTime = 2f;
                centerWaitTime = 4f;
                break;
            case 2:
                moveSpeed = 3.5f;
                doorWaitTime = 1.2f;
                centerWaitTime = 2f;
                break;
            case 3:
                moveSpeed = 5f;
                doorWaitTime = 0.7f;
                centerWaitTime = 1f;
                break;
        }

        if (agent != null)
            agent.speed = moveSpeed;
    }
}
