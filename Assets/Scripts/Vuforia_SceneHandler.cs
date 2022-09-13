using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class Vuforia_SceneHandler : DefaultTrackableEventHandler
{
    [Header("References")]
    public Animator characterAnimator = default;
    [Space(5)]
    public GameObject sceneRootObject = default;
    public GameObject sceneCharObject = default;

    // Animaciones cavernicola
    private readonly string trigger_Roar = "Roar";
    private readonly string trigger_Swipe = "Swipe";

    private IEnumerator tauntRutine;

    protected override void OnTrackingFound()
    {
        if (MainSystem.AppCanTrackTargets)
        {
            ShowScene();
        };

    }

    public void ForceHide()
    {
        HideScene();
    }

    protected override void OnTrackingLost()
    {
        HideScene();
    }

    private void ShowScene()
    {
        MainSystem.SetupNewTarget(this);
        //MainSystem.PlayAudioclip(GetSoundByID(SceneSound.char_show)); // sonido aparicion

        if (!MainSystem.AppIsInPhotoMode)
        {
            sceneRootObject.SetActive(true);

        };

        sceneCharObject.SetActive(true);

    }

    private void HideScene()
    {
        if (tauntRutine != null) { StopCoroutine(tauntRutine); }; // detener rutina

        sceneRootObject.SetActive(false);
        sceneCharObject.SetActive(false);
        MainSystem.DisableTarget();
    }

    public void StartTauntRutine()
    {
        if (tauntRutine != null) { StopCoroutine(tauntRutine); };
        tauntRutine = TauntEnumerator();
        StartCoroutine(tauntRutine);
    }

    private IEnumerator TauntEnumerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            characterAnimator.SetTrigger(trigger_Swipe);
            yield return new WaitForSeconds(2);
            characterAnimator.SetTrigger(trigger_Roar);
        };
    }
}