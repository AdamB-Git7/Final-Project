using UnityEngine;

// =====================================================================
//  AUDIO MANAGER — plays background music and sound effects
// =====================================================================
//  Other scripts call AudioManager.Instance.PlayDoorClose() etc.
//  Uses AudioSource (Unity's built-in component for sound).
// =====================================================================
public class AudioManager : MonoBehaviour
{
    // "Instance" is a public reference so any script can reach this one
    // without having to find it. Set in Awake() once and used everywhere.
    public static AudioManager Instance;

    [Header("Audio Clips")]
    public AudioClip ambientLoop;   // background music
    public AudioClip doorSlam;      // door close SFX
    public AudioClip jumpscare;     // death scream
    public AudioClip footstep;      // animatronic walking
    public AudioClip cameraClick;   // camera UI open/close

    [Header("Volumes")]
    public float ambientVolume = 0.3f;
    public float sfxVolume = 0.7f;

    AudioSource ambientSource;   // plays the looping music
    AudioSource sfxSource;       // plays one-shot effects

    void Awake()
    {
        Instance = this;   // any script can now find us via AudioManager.Instance
    }

    void Start()
    {
        // Load each clip from the Resources folder if not set in Inspector
        if (ambientLoop == null) ambientLoop = Resources.Load<AudioClip>("ambient_drone");
        if (doorSlam == null)    doorSlam    = Resources.Load<AudioClip>("door_slam");
        if (jumpscare == null)   jumpscare   = Resources.Load<AudioClip>("jumpscare");
        if (footstep == null)    footstep    = Resources.Load<AudioClip>("footstep");
        if (cameraClick == null) cameraClick = Resources.Load<AudioClip>("camera_click");

        // First AudioSource: looping background music
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.clip = ambientLoop;
        ambientSource.loop = true;
        ambientSource.volume = ambientVolume;
        if (ambientLoop != null) ambientSource.Play();

        // Second AudioSource: one-shot sound effects
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.volume = sfxVolume;
    }

    // PlayOneShot plays a sound without interrupting other sounds.
    // Multiple SFX can overlap (door slam + footstep at the same time).
    public void PlayDoorClose()   { if (doorSlam != null)    sfxSource.PlayOneShot(doorSlam); }
    public void PlayJumpscare()   { if (jumpscare != null)   sfxSource.PlayOneShot(jumpscare); }
    public void PlayFootstep()    { if (footstep != null)    sfxSource.PlayOneShot(footstep); }
    public void PlayCameraClick() { if (cameraClick != null) sfxSource.PlayOneShot(cameraClick); }
}
