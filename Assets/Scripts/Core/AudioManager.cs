
// Imports the UnityEngine namespace.
using UnityEngine;








// Declares the class named AudioManager.
public class AudioManager : MonoBehaviour

// Opens a new code block.
{



    // Declares the variable Instance.
    public static AudioManager Instance;


    // Applies the Header("Audio Clips") attribute.
    [Header("Audio Clips")]

    // Declares the variable ambientLoop.
    public AudioClip ambientLoop;

    // Declares the variable doorSlam.
    public AudioClip doorSlam;

    // Declares the variable jumpscare.
    public AudioClip jumpscare;

    // Declares the variable footstep.
    public AudioClip footstep;

    // Declares the variable cameraClick.
    public AudioClip cameraClick;


    // Applies the Header("Volumes") attribute.
    [Header("Volumes")]

    // Declares the variable ambientVolume and initializes it.
    public float ambientVolume = 0.3f;

    // Declares the variable sfxVolume and initializes it.
    public float sfxVolume = 0.7f;


    // Declares the variable ambientSource.
    AudioSource ambientSource;

    // Declares the variable sfxSource.
    AudioSource sfxSource;


    // Declares the method named Awake.
    void Awake()

    // Opens a new code block.
    {

        // Updates an existing value.
        Instance = this;

    // Closes the current code block.
    }


    // Declares the method named Start.
    void Start()

    // Opens a new code block.
    {


        // Checks the condition and runs the inline statement when it is true.
        if (ambientLoop == null) ambientLoop = Resources.Load<AudioClip>("ambient_drone");

        // Checks the condition and runs the inline statement when it is true.
        if (doorSlam == null)    doorSlam    = Resources.Load<AudioClip>("door_slam");

        // Checks the condition and runs the inline statement when it is true.
        if (jumpscare == null)   jumpscare   = Resources.Load<AudioClip>("jumpscare");

        // Checks the condition and runs the inline statement when it is true.
        if (footstep == null)    footstep    = Resources.Load<AudioClip>("footstep");

        // Checks the condition and runs the inline statement when it is true.
        if (cameraClick == null) cameraClick = Resources.Load<AudioClip>("camera_click");



        // Updates an existing value.
        ambientSource = gameObject.AddComponent<AudioSource>();

        // Updates an existing value.
        ambientSource.clip = ambientLoop;

        // Updates an existing value.
        ambientSource.loop = true;

        // Updates an existing value.
        ambientSource.volume = ambientVolume;

        // Checks the condition and runs the inline statement when it is true.
        if (ambientLoop != null) ambientSource.Play();



        // Updates an existing value.
        sfxSource = gameObject.AddComponent<AudioSource>();

        // Updates an existing value.
        sfxSource.volume = sfxVolume;

    // Closes the current code block.
    }




    // Executes this statement.
    public void PlayDoorClose()   { if (doorSlam != null)    sfxSource.PlayOneShot(doorSlam); }

    // Executes this statement.
    public void PlayJumpscare()   { if (jumpscare != null)   sfxSource.PlayOneShot(jumpscare); }

    // Executes this statement.
    public void PlayFootstep()    { if (footstep != null)    sfxSource.PlayOneShot(footstep); }

    // Executes this statement.
    public void PlayCameraClick() { if (cameraClick != null) sfxSource.PlayOneShot(cameraClick); }

// Closes the current code block.
}
