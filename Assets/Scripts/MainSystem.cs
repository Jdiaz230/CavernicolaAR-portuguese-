using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class MainSystem : MonoBehaviour
{
    public static MainSystem instance = default;
    private AudioSource audioSource_Ambient = default;
    private AudioSource audioSource_Sfx = default;
    public CameraManager cameraController;
    [Space(15)]
    public GameObject noActiveTargetPanel;

    private bool appIsReady = false; // Control general de interacciones

    private bool appIsInPhotoMode = false; // Control general de interacciones

    private bool appIsInPreview = false; // Control general de interacciones


    private Vuforia_SceneHandler currentSceneInViewer = null;
    public static bool AppHaveActiveTarget { get => (instance.currentSceneInViewer != null);  }
    public static bool AppCanTrackTargets { get => instance.appIsReady; set => instance.appIsReady = value; }
    public static bool AppIsInPhotoMode { get => instance.appIsInPhotoMode; set => instance.appIsInPhotoMode = value; }

    public static bool AppIsInPreview { get => instance.appIsInPreview; set => instance.appIsInPreview = value; }


    public AudioClip standardClickSound = default;

    public static void ClickSomething()
    {
        PlayAudioclip(instance.standardClickSound);
    }


    private void Update()
    {
        noActiveTargetPanel.SetActive(!AppHaveActiveTarget && AppCanTrackTargets && !AppIsInPreview); // si puede reconocer pero no hay ninguno
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        };

        SetupAudioSystems();
    }

    public void SetupAudioSystems()
    {
        audioSource_Ambient = GetAudioSource(true);
        audioSource_Sfx = GetAudioSource(false);
    }

    private AudioSource GetAudioSource(bool isLoop)
    {
        AudioSource returnValue = gameObject.AddComponent<AudioSource>();
        returnValue.loop = isLoop;
        returnValue.playOnAwake = false;
        return returnValue;
    }

    public static void PlayAudioclip(AudioClip clip)
    {
        if (clip != null)
        {
            instance.audioSource_Sfx.PlayOneShot(clip, 1);
        };
    }

    public static void SetupNewTarget(Vuforia_SceneHandler sceneHandler)
    {
        instance.currentSceneInViewer = sceneHandler;
    }

    public static void DisableTarget()
    {
        instance.currentSceneInViewer = null;
    }

    public static void ForceHideScene()
    {
        if (instance.currentSceneInViewer != null)
        {
            instance.currentSceneInViewer.ForceHide();
        };
    }
}
