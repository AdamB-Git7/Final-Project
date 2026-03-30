using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Navigation Points")]
    public Transform leftDoorPoint;
    public Transform rightDoorPoint;
    public Transform centerPoint;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float doorWaitTime = 1.5f;
    public float centerWaitTime = 3f;

    [Header("References")]
    public GameManager gameManager;
    public DoorController leftDoor;
    public DoorController rightDoor;

    NavMeshAgent agent;
    float timer;
    bool gameEnded;

    enum State { WalkingToCenter, WaitingAtCenter, WalkingToDoor, WaitingAtDoor, Retreating, EnteringOffice }
    State state;
    bool goingLeft;

    Vector3 officeCenter = new Vector3(0, 0.5f, 1f);

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        gameEnded = false;
        timer = 0f;
        state = State.WalkingToCenter;

        if (centerPoint != null)
            agent.SetDestination(centerPoint.position);
    }

    void Update()
    {
        if (gameEnded) return;
        if (gameManager != null && !gameManager.IsGameActive()) return;

        switch (state)
        {
            case State.WalkingToCenter:
                if (CloseEnoughTo(centerPoint.position, 2f))
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
                if (CloseEnoughTo(doorPos, 2f))
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
                        agent.SetDestination(officeCenter);
                    }
                }
                else
                {
                    Retreat();
                }
                break;

            case State.EnteringOffice:
                if (CloseEnoughTo(officeCenter, 1.5f))
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
                if (CloseEnoughTo(centerPoint.position, 2f))
                {
                    agent.isStopped = true;
                    state = State.WaitingAtCenter;
                    timer = 0f;
                }
                break;
        }
    }

    void ChooseDoor()
    {
        goingLeft = Random.value > 0.5f;
        Transform target = goingLeft ? leftDoorPoint : rightDoorPoint;

        agent.isStopped = false;
        agent.SetDestination(target.position);
        state = State.WalkingToDoor;
    }

    void Retreat()
    {
        state = State.Retreating;
        agent.isStopped = false;
        agent.SetDestination(centerPoint.position);
    }

    bool CloseEnoughTo(Vector3 target, float dist)
    {
        return Vector3.Distance(transform.position, target) < dist;
    }

    public void SetDifficulty(int night)
    {
        switch (night)
        {
            case 1:
                moveSpeed = 1.5f;
                doorWaitTime = 2f;
                centerWaitTime = 4f;
                break;
            case 2:
                moveSpeed = 2.5f;
                doorWaitTime = 1.2f;
                centerWaitTime = 2f;
                break;
            case 3:
                moveSpeed = 4f;
                doorWaitTime = 0.7f;
                centerWaitTime = 1f;
                break;
        }

        if (agent != null)
            agent.speed = moveSpeed;
    }
}
