using UnityEngine;
using UnityEngine.AI;  // for NavMeshAgent (Unity's pathfinding)

// =====================================================================
//  ENEMY AI — controls the animatronic
// =====================================================================
//  Simple state machine. The AI is in ONE state at a time:
//
//    HIDING    — walking to a random hiding spot
//    WAITING   — standing still for a few seconds
//    ATTACKING — walking to a door
//    BREAKING_IN — rushing the player to kill them
//
//  Each frame, we check the current state and switch to the next one
//  when the right thing happens (arrived, time elapsed, etc.).
// =====================================================================
public class EnemyAI : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 3f;            // walking speed
    public float aggressionGrowth = 0.04f;  // how fast door-attack chance grows

    [Header("References")]
    public GameManager gameManager;
    public DoorController leftDoor;
    public DoorController rightDoor;

    // All the spots the AI walks to (set automatically in Start)
    public Transform corridor;
    public Transform leftAlcove;
    public Transform rightAlcove;
    public Transform stage;
    public Transform classroom;
    public Transform bathroom;
    public Transform leftDoorSpot;
    public Transform rightDoorSpot;
    public Transform officeCenter;

    // The 4 possible states
    enum State { Hiding, Waiting, Attacking, BreakingIn }

    NavMeshAgent agent;       // Unity's pathfinding component
    State state;              // current state
    float waitTimer;          // counts down while waiting
    float aggression;         // 0 to 0.7 — chance of attacking
    bool goingLeft;           // which door we're attacking
    bool gameEnded;           // true once the AI catches the player

    // =================================================================
    //  Start() — runs once when the AI spawns
    // =================================================================
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        FindAllSpots();   // grab references to scene objects

        // Begin by hiding
        PickHidingSpot();
    }

    // =================================================================
    //  Update() — runs every frame
    //  This is the main loop. Handles each state.
    // =================================================================
    void Update()
    {
        if (gameEnded) return;
        if (gameManager != null && !gameManager.IsGameActive()) return;

        if      (state == State.Hiding)     UpdateHiding();
        else if (state == State.Waiting)    UpdateWaiting();
        else if (state == State.Attacking)  UpdateAttacking();
        else if (state == State.BreakingIn) UpdateBreakingIn();

        PlayFootsteps();
        PreventOfficeEntry();
    }

    // -----------------------------------------------------------------
    //  HIDING: walk to a hiding spot. When we arrive, switch to Waiting.
    // -----------------------------------------------------------------
    void UpdateHiding()
    {
        if (HasArrived())
        {
            state = State.Waiting;
            waitTimer = Random.Range(1f, 5f);  // wait 1-5 seconds
        }
    }

    // -----------------------------------------------------------------
    //  WAITING: stand still while the timer counts down.
    //  When time's up, decide: hide again or attack a door?
    // -----------------------------------------------------------------
    void UpdateWaiting()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer > 0f) return;

        // Aggression grows each cycle, capped at 70%
        aggression += aggressionGrowth;
        bool shouldAttack = Random.value < Mathf.Min(aggression, 0.7f);

        if (shouldAttack) PickDoor();
        else              PickHidingSpot();
    }

    // -----------------------------------------------------------------
    //  ATTACKING: walk to a door. When we arrive, wait + check the door.
    // -----------------------------------------------------------------
    void UpdateAttacking()
    {
        if (!HasArrived()) return;

        // We're at the door — count down the camping timer
        waitTimer -= Time.deltaTime;
        if (waitTimer > 0f) return;

        // Time's up. Is the door open?
        DoorController door = goingLeft ? leftDoor : rightDoor;
        if (door != null && !door.isClosed)
        {
            StartBreakIn();   // door open → attack the player
        }
        else
        {
            PickHidingSpot(); // door closed → give up, go hide
        }
    }

    // -----------------------------------------------------------------
    //  BREAKING_IN: rush the player. Kill on contact.
    // -----------------------------------------------------------------
    void UpdateBreakingIn()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Distance check (XZ — ignore height)
        Vector3 me = transform.position;
        Vector3 player = cam.transform.position;
        float dx = me.x - player.x;
        float dz = me.z - player.z;
        float distance = Mathf.Sqrt(dx * dx + dz * dz);

        if (distance < 2f)
        {
            // CAUGHT — game over
            gameEnded = true;
            agent.isStopped = true;
            FaceThePlayer();
            if (gameManager != null) gameManager.TriggerGameOver();
        }
    }

    // =================================================================
    //  STATE TRANSITIONS — methods that switch the AI between states
    // =================================================================

    // Pick a random hiding spot and start walking there
    void PickHidingSpot()
    {
        Transform[] spots = { corridor, leftAlcove, rightAlcove, stage, classroom, bathroom };
        Transform spot = spots[Random.Range(0, spots.Length)];
        if (spot != null) agent.SetDestination(spot.position);
        state = State.Hiding;
    }

    // Pick a random door and start walking there
    void PickDoor()
    {
        goingLeft = Random.value < 0.5f;
        Transform doorSpot = goingLeft ? leftDoorSpot : rightDoorSpot;
        if (doorSpot != null) agent.SetDestination(doorSpot.position);
        state = State.Attacking;
        waitTimer = Random.Range(1f, 5f);  // camping time at door
    }

    // Start the kill rush
    void StartBreakIn()
    {
        // Force both doors open so we can't be trapped
        if (leftDoor != null) leftDoor.OpenDoor();
        if (rightDoor != null) rightDoor.OpenDoor();

        // Sprint at the player
        agent.speed = moveSpeed * 1.5f;

        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 target = cam.transform.position;
            target.y = 0.5f;   // floor level
            agent.SetDestination(target);
        }

        state = State.BreakingIn;
    }

    // =================================================================
    //  HELPERS
    // =================================================================

    // True if the agent reached its destination
    bool HasArrived()
    {
        if (agent.pathPending) return false;
        return agent.remainingDistance < 1.5f;
    }

    // Turn to face the player camera (for the death pose)
    void FaceThePlayer()
    {
        Camera cam = Camera.main;
        if (cam == null) return;
        Vector3 dir = cam.transform.position - transform.position;
        dir.y = 0;
        if (dir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dir);
    }

    // If we're not breaking in, we shouldn't be in the office. Push back out.
    void PreventOfficeEntry()
    {
        if (state == State.BreakingIn) return;  // breaking in is allowed

        Vector3 p = transform.position;
        bool inOffice = p.x > -4f && p.x < 4f && p.z > -2f && p.z < 2f;
        if (inOffice)
        {
            // Teleport back to the closest hallway
            Vector3 push = p;
            if (Mathf.Abs(p.x) > Mathf.Abs(p.z * 2)) push.x = Mathf.Sign(p.x) * 5.5f;
            else                                     push.z = Mathf.Sign(p.z) * 2.5f;
            agent.Warp(push);
        }
    }

    // Footstep timer
    float footstepTimer;
    void PlayFootsteps()
    {
        // Only when moving and AudioManager exists
        if (agent.velocity.sqrMagnitude < 0.1f) return;
        if (AudioManager.Instance == null) return;

        Camera cam = Camera.main;
        if (cam == null) return;

        // Distance to player
        float dist = Vector3.Distance(transform.position, cam.transform.position);
        if (dist > 15f) return;

        // Closer = faster footsteps (Mathf.Lerp blends two values)
        float interval = Mathf.Lerp(0.4f, 1.2f, Mathf.InverseLerp(2f, 15f, dist));

        footstepTimer += Time.deltaTime;
        if (footstepTimer >= interval)
        {
            footstepTimer = 0f;
            AudioManager.Instance.PlayFootstep();
        }
    }

    // Find all the scene objects we need by name
    void FindAllSpots()
    {
        if (corridor == null)      corridor      = FindByName("Spot_Corridor");
        if (leftAlcove == null)    leftAlcove    = FindByName("Spot_LeftAlcove");
        if (rightAlcove == null)   rightAlcove   = FindByName("Spot_RightAlcove");
        if (stage == null)         stage         = FindByName("Spot_Stage");
        if (classroom == null)     classroom     = FindByName("Spot_Classroom");
        if (bathroom == null)      bathroom      = FindByName("Spot_Bathroom");
        if (leftDoorSpot == null)  leftDoorSpot  = FindByName("Spot_LeftDoor");
        if (rightDoorSpot == null) rightDoorSpot = FindByName("Spot_RightDoor");
        if (officeCenter == null)  officeCenter  = FindByName("Spot_OfficeCenter");

        if (gameManager == null) gameManager = Object.FindFirstObjectByType<GameManager>();
        if (leftDoor == null || rightDoor == null)
        {
            DoorController[] doors = Object.FindObjectsByType<DoorController>(FindObjectsSortMode.None);
            foreach (DoorController d in doors)
            {
                if (leftDoor == null && d.transform.position.x < 0) leftDoor = d;
                if (rightDoor == null && d.transform.position.x > 0) rightDoor = d;
            }
        }
    }

    Transform FindByName(string name)
    {
        GameObject obj = GameObject.Find(name);
        if (obj != null) return obj.transform;
        return null;
    }
}
