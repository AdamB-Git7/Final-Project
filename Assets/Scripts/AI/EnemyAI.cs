
// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the UnityEngine.AI namespace.
using UnityEngine.AI;















// Declares the class named EnemyAI.
public class EnemyAI : MonoBehaviour

// Opens a new code block.
{

    // Applies the Header("Settings") attribute.
    [Header("Settings")]

    // Declares the variable moveSpeed and initializes it.
    public float moveSpeed = 3f;

    // Declares the variable aggressionGrowth and initializes it.
    public float aggressionGrowth = 0.04f;


    // Applies the Header("References") attribute.
    [Header("References")]

    // Declares the variable gameManager.
    public GameManager gameManager;

    // Declares the variable leftDoor.
    public DoorController leftDoor;

    // Declares the variable rightDoor.
    public DoorController rightDoor;



    // Declares the variable corridor.
    public Transform corridor;

    // Declares the variable leftAlcove.
    public Transform leftAlcove;

    // Declares the variable rightAlcove.
    public Transform rightAlcove;

    // Declares the variable stage.
    public Transform stage;

    // Declares the variable classroom.
    public Transform classroom;

    // Declares the variable bathroom.
    public Transform bathroom;

    // Declares the variable leftDoorSpot.
    public Transform leftDoorSpot;

    // Declares the variable rightDoorSpot.
    public Transform rightDoorSpot;

    // Declares the variable officeCenter.
    public Transform officeCenter;



    // Declares the enum named State.
    enum State { Hiding, Waiting, Attacking, BreakingIn }


    // Declares the variable agent.
    NavMeshAgent agent;

    // Declares the variable state.
    State state;

    // Declares the variable waitTimer.
    float waitTimer;

    // Declares the variable aggression.
    float aggression;

    // Declares the variable goingLeft.
    bool goingLeft;

    // Declares the variable gameEnded.
    bool gameEnded;





    // Declares the method named Start.
    void Start()

    // Opens a new code block.
    {

        // Updates an existing value.
        agent = GetComponent<NavMeshAgent>();

        // Updates an existing value.
        agent.speed = moveSpeed;


        // Calls a method.
        FindAllSpots();



        // Calls a method.
        PickHidingSpot();

    // Closes the current code block.
    }






    // Declares the method named Update.
    void Update()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (gameEnded) return;

        // Checks the condition and runs the inline statement when it is true.
        if (gameManager != null && !gameManager.IsGameActive()) return;


        // Checks the condition and runs the inline statement when it is true.
        if      (state == State.Hiding)     UpdateHiding();

        // Checks the next condition and runs the inline statement when it is true.
        else if (state == State.Waiting)    UpdateWaiting();

        // Checks the next condition and runs the inline statement when it is true.
        else if (state == State.Attacking)  UpdateAttacking();

        // Checks the next condition and runs the inline statement when it is true.
        else if (state == State.BreakingIn) UpdateBreakingIn();


        // Calls a method.
        PlayFootsteps();

        // Calls a method.
        PreventOfficeEntry();

    // Closes the current code block.
    }





    // Declares the method named UpdateHiding.
    void UpdateHiding()

    // Opens a new code block.
    {

        // Checks whether the condition is true.
        if (HasArrived())

        // Opens a new code block.
        {

            // Updates an existing value.
            state = State.Waiting;

            // Updates an existing value.
            waitTimer = Random.Range(1f, 5f);

        // Closes the current code block.
        }

    // Closes the current code block.
    }






    // Declares the method named UpdateWaiting.
    void UpdateWaiting()

    // Opens a new code block.
    {

        // Updates an existing value.
        waitTimer -= Time.deltaTime;

        // Checks the condition and runs the inline statement when it is true.
        if (waitTimer > 0f) return;



        // Updates an existing value.
        aggression += aggressionGrowth;

        // Declares the variable shouldAttack and initializes it.
        bool shouldAttack = Random.value < Mathf.Min(aggression, 0.7f);


        // Checks the condition and runs the inline statement when it is true.
        if (shouldAttack) PickDoor();

        // Runs the fallback inline statement.
        else              PickHidingSpot();

    // Closes the current code block.
    }





    // Declares the method named UpdateAttacking.
    void UpdateAttacking()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (!HasArrived()) return;



        // Updates an existing value.
        waitTimer -= Time.deltaTime;

        // Checks the condition and runs the inline statement when it is true.
        if (waitTimer > 0f) return;



        // Declares the variable door and initializes it.
        DoorController door = goingLeft ? leftDoor : rightDoor;

        // Checks whether the condition is true.
        if (door != null && !door.isClosed)

        // Opens a new code block.
        {

            // Calls a method.
            StartBreakIn();

        // Closes the current code block.
        }

        // Runs the fallback branch when earlier conditions were false.
        else

        // Opens a new code block.
        {

            // Calls a method.
            PickHidingSpot();

        // Closes the current code block.
        }

    // Closes the current code block.
    }





    // Declares the method named UpdateBreakingIn.
    void UpdateBreakingIn()

    // Opens a new code block.
    {

        // Declares the variable cam and initializes it.
        Camera cam = Camera.main;

        // Checks the condition and runs the inline statement when it is true.
        if (cam == null) return;



        // Declares the variable me and initializes it.
        Vector3 me = transform.position;

        // Declares the variable player and initializes it.
        Vector3 player = cam.transform.position;

        // Declares the variable dx and initializes it.
        float dx = me.x - player.x;

        // Declares the variable dz and initializes it.
        float dz = me.z - player.z;

        // Declares the variable distance and initializes it.
        float distance = Mathf.Sqrt(dx * dx + dz * dz);


        // Checks whether the condition is true.
        if (distance < 2f)

        // Opens a new code block.
        {


            // Updates an existing value.
            gameEnded = true;

            // Updates an existing value.
            agent.isStopped = true;

            // Calls a method.
            FaceThePlayer();

            // Checks the condition and runs the inline statement when it is true.
            if (gameManager != null) gameManager.TriggerGameOver();

        // Closes the current code block.
        }

    // Closes the current code block.
    }







    // Declares the method named PickHidingSpot.
    void PickHidingSpot()

    // Opens a new code block.
    {

        // Declares the variable spots and initializes it.
        Transform[] spots = { corridor, leftAlcove, rightAlcove, stage, classroom, bathroom };

        // Declares the variable spot and initializes it.
        Transform spot = spots[Random.Range(0, spots.Length)];

        // Checks the condition and runs the inline statement when it is true.
        if (spot != null) agent.SetDestination(spot.position);

        // Updates an existing value.
        state = State.Hiding;

    // Closes the current code block.
    }



    // Declares the method named PickDoor.
    void PickDoor()

    // Opens a new code block.
    {

        // Updates an existing value.
        goingLeft = Random.value < 0.5f;

        // Declares the variable doorSpot and initializes it.
        Transform doorSpot = goingLeft ? leftDoorSpot : rightDoorSpot;

        // Checks the condition and runs the inline statement when it is true.
        if (doorSpot != null) agent.SetDestination(doorSpot.position);

        // Updates an existing value.
        state = State.Attacking;

        // Updates an existing value.
        waitTimer = Random.Range(1f, 5f);

    // Closes the current code block.
    }



    // Declares the method named StartBreakIn.
    void StartBreakIn()

    // Opens a new code block.
    {


        // Checks the condition and runs the inline statement when it is true.
        if (leftDoor != null) leftDoor.OpenDoor();

        // Checks the condition and runs the inline statement when it is true.
        if (rightDoor != null) rightDoor.OpenDoor();



        // Updates an existing value.
        agent.speed = moveSpeed * 1.5f;


        // Declares the variable cam and initializes it.
        Camera cam = Camera.main;

        // Checks whether the condition is true.
        if (cam != null)

        // Opens a new code block.
        {

            // Declares the variable target and initializes it.
            Vector3 target = cam.transform.position;

            // Updates an existing value.
            target.y = 0.5f;

            // Calls a method.
            agent.SetDestination(target);

        // Closes the current code block.
        }


        // Updates an existing value.
        state = State.BreakingIn;

    // Closes the current code block.
    }







    // Declares the method named HasArrived.
    bool HasArrived()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (agent.pathPending) return false;

        // Returns the specified value.
        return agent.remainingDistance < 1.5f;

    // Closes the current code block.
    }



    // Declares the method named FaceThePlayer.
    void FaceThePlayer()

    // Opens a new code block.
    {

        // Declares the variable cam and initializes it.
        Camera cam = Camera.main;

        // Checks the condition and runs the inline statement when it is true.
        if (cam == null) return;

        // Declares the variable dir and initializes it.
        Vector3 dir = cam.transform.position - transform.position;

        // Updates an existing value.
        dir.y = 0;

        // Checks the condition and runs the inline statement when it is true.
        if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);

    // Closes the current code block.
    }



    // Declares the method named PreventOfficeEntry.
    void PreventOfficeEntry()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (state == State.BreakingIn) return;


        // Declares the variable p and initializes it.
        Vector3 p = transform.position;

        // Declares the variable inOffice and initializes it.
        bool inOffice = p.x > -4f && p.x < 4f && p.z > -2f && p.z < 2f;

        // Checks whether the condition is true.
        if (inOffice)

        // Opens a new code block.
        {


            // Declares the variable push and initializes it.
            Vector3 push = p;

            // Checks the condition and runs the inline statement when it is true.
            if (Mathf.Abs(p.x) > Mathf.Abs(p.z * 2)) push.x = Mathf.Sign(p.x) * 5.5f;

            // Runs the fallback inline statement.
            else                                     push.z = Mathf.Sign(p.z) * 2.5f;

            // Calls a method.
            agent.Warp(push);

        // Closes the current code block.
        }

    // Closes the current code block.
    }



    // Declares the variable footstepTimer.
    float footstepTimer;

    // Declares the method named PlayFootsteps.
    void PlayFootsteps()

    // Opens a new code block.
    {


        // Checks the condition and runs the inline statement when it is true.
        if (agent.velocity.sqrMagnitude < 0.1f) return;

        // Checks the condition and runs the inline statement when it is true.
        if (AudioManager.Instance == null) return;


        // Declares the variable cam and initializes it.
        Camera cam = Camera.main;

        // Checks the condition and runs the inline statement when it is true.
        if (cam == null) return;



        // Declares the variable dist and initializes it.
        float dist = Vector3.Distance(transform.position, cam.transform.position);

        // Checks the condition and runs the inline statement when it is true.
        if (dist > 15f) return;



        // Declares the variable interval and initializes it.
        float interval = Mathf.Lerp(0.4f, 1.2f, Mathf.InverseLerp(2f, 15f, dist));


        // Updates an existing value.
        footstepTimer += Time.deltaTime;

        // Checks whether the condition is true.
        if (footstepTimer >= interval)

        // Opens a new code block.
        {

            // Updates an existing value.
            footstepTimer = 0f;

            // Calls a method.
            AudioManager.Instance.PlayFootstep();

        // Closes the current code block.
        }

    // Closes the current code block.
    }



    // Declares the method named FindAllSpots.
    void FindAllSpots()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (corridor == null)      corridor      = FindByName("Spot_Corridor");

        // Checks the condition and runs the inline statement when it is true.
        if (leftAlcove == null)    leftAlcove    = FindByName("Spot_LeftAlcove");

        // Checks the condition and runs the inline statement when it is true.
        if (rightAlcove == null)   rightAlcove   = FindByName("Spot_RightAlcove");

        // Checks the condition and runs the inline statement when it is true.
        if (stage == null)         stage         = FindByName("Spot_Stage");

        // Checks the condition and runs the inline statement when it is true.
        if (classroom == null)     classroom     = FindByName("Spot_Classroom");

        // Checks the condition and runs the inline statement when it is true.
        if (bathroom == null)      bathroom      = FindByName("Spot_Bathroom");

        // Checks the condition and runs the inline statement when it is true.
        if (leftDoorSpot == null)  leftDoorSpot  = FindByName("Spot_LeftDoor");

        // Checks the condition and runs the inline statement when it is true.
        if (rightDoorSpot == null) rightDoorSpot = FindByName("Spot_RightDoor");

        // Checks the condition and runs the inline statement when it is true.
        if (officeCenter == null)  officeCenter  = FindByName("Spot_OfficeCenter");


        // Checks the condition and runs the inline statement when it is true.
        if (gameManager == null) gameManager = Object.FindFirstObjectByType<GameManager>();

        // Checks whether the condition is true.
        if (leftDoor == null || rightDoor == null)

        // Opens a new code block.
        {

            // Declares the variable doors and initializes it.
            DoorController[] doors = Object.FindObjectsByType<DoorController>(FindObjectsSortMode.None);

            // Iterates through each item in the collection.
            foreach (DoorController d in doors)

            // Opens a new code block.
            {

                // Checks the condition and runs the inline statement when it is true.
                if (leftDoor == null && d.transform.position.x < 0) leftDoor = d;

                // Checks the condition and runs the inline statement when it is true.
                if (rightDoor == null && d.transform.position.x > 0) rightDoor = d;

            // Closes the current code block.
            }

        // Closes the current code block.
        }

    // Closes the current code block.
    }


    // Declares the method named FindByName.
    Transform FindByName(string name)

    // Opens a new code block.
    {

        // Declares the variable obj and initializes it.
        GameObject obj = GameObject.Find(name);

        // Checks the condition and runs the inline statement when it is true.
        if (obj != null) return obj.transform;

        // Returns the specified value.
        return null;

    // Closes the current code block.
    }

// Closes the current code block.
}
