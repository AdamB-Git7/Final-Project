using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecurityCamera : MonoBehaviour
{
    [Header("Cameras")]
    public Camera[] securityCameras;
    public string[] cameraNames;

    [Header("UI")]
    public RawImage monitorDisplay;
    public TMP_Text cameraNameText;

    [Header("Controls")]
    public KeyCode nextCamera = KeyCode.D;
    public KeyCode prevCamera = KeyCode.A;
    public KeyCode toggleMonitor = KeyCode.Tab;

    [Header("Monitor Settings")]
    public int renderWidth = 512;
    public int renderHeight = 384;

    int currentCamera;
    bool monitorActive;
    RenderTexture renderTexture;

    void Start()
    {
        currentCamera = 0;
        monitorActive = false;

        renderTexture = new RenderTexture(renderWidth, renderHeight, 16);

        foreach (Camera cam in securityCameras)
        {
            cam.enabled = false;
        }

        if (monitorDisplay != null)
            monitorDisplay.gameObject.SetActive(false);

        UpdateCamera();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleMonitor))
        {
            monitorActive = !monitorActive;

            if (monitorDisplay != null)
                monitorDisplay.gameObject.SetActive(monitorActive);

            UpdateCamera();
        }

        if (!monitorActive) return;

        if (Input.GetKeyDown(nextCamera))
        {
            currentCamera = (currentCamera + 1) % securityCameras.Length;
            UpdateCamera();
        }

        if (Input.GetKeyDown(prevCamera))
        {
            currentCamera--;
            if (currentCamera < 0) currentCamera = securityCameras.Length - 1;
            UpdateCamera();
        }
    }

    void UpdateCamera()
    {
        if (securityCameras.Length == 0) return;

        foreach (Camera cam in securityCameras)
        {
            cam.targetTexture = null;
            cam.enabled = false;
        }

        if (!monitorActive) return;

        Camera activeCam = securityCameras[currentCamera];
        activeCam.targetTexture = renderTexture;
        activeCam.enabled = true;

        if (monitorDisplay != null)
            monitorDisplay.texture = renderTexture;

        if (cameraNameText != null && cameraNames.Length > currentCamera)
            cameraNameText.text = cameraNames[currentCamera];
    }

    void OnDestroy()
    {
        if (renderTexture != null)
            renderTexture.Release();
    }
}
