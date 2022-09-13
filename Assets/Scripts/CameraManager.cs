using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public RectTransform logoRect; // para encontrar los pixeles necesarios para recorte

    public GameObject mainPanelObject;
    public GameObject hudButtonHud;
    public GameObject waterMarkObject;

    public MainCanvas_Controller canvasControllerComponent;

    public Camera arCameraComponent;
    public Camera hudCameraComponent;

    public bool optimizeForManyScreenshots = true;

    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenShot;

    private IEnumerator screenshotRutine;

    public void CaptureScreenshot()
    {
        if (screenshotRutine != null) { StopCoroutine(screenshotRutine); };
        screenshotRutine = ScreenshotEnumerator();
        StartCoroutine(screenshotRutine);
    }

    private IEnumerator ScreenshotEnumerator()
    {
        int captureWidth = Screen.width;
        int captureHeight = Screen.height;

        waterMarkObject.SetActive(true);
        hudButtonHud.SetActive(false);
        mainPanelObject.SetActive(false);


        int recorte = Mathf.FloorToInt(Mathf.Abs(hudCameraComponent.WorldToScreenPoint(logoRect.position).y));

        if (renderTexture == null)
        {
            rect = new Rect(0, 0, captureWidth, captureHeight);
            renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
            screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);

        };

        arCameraComponent.targetTexture = renderTexture;
        arCameraComponent.Render();
        hudCameraComponent.targetTexture = renderTexture;
        hudCameraComponent.Render();
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        yield return null;

        int fixedWidth = captureWidth - 2; // extra para laterales
        int fixedHeight = captureHeight - recorte; // tomar solo porcion de pantalla

        float imageRatio = (float)fixedWidth / (float)fixedHeight;

        Color[] c = screenShot.GetPixels(2, recorte, fixedWidth, fixedHeight);
        
        Texture2D m2Texture = new Texture2D(fixedWidth, fixedHeight);
        m2Texture.SetPixels(c);
        m2Texture.Apply();

        waterMarkObject.SetActive(false);
        hudButtonHud.SetActive(true);
        mainPanelObject.SetActive(true);


        // canvasControllerComponent.TriggerShutterLight();
        // aca me manda los datos para el codigo main
        canvasControllerComponent.SetPreviewPhoto(m2Texture, imageRatio);

        //arCameraComponent.targetTexture = null; // restaurar segunda camara...
        hudCameraComponent.targetTexture = null;
        RenderTexture.active = null;

        if (optimizeForManyScreenshots == false)
        {
            Destroy(renderTexture);
            renderTexture = null;
            screenShot = null;
        };
    }

    public void RestoreArCameraSystem()
    {
        // recuperar camara luego de modo preview
        arCameraComponent.targetTexture = null; // restaurar segunda camara...
    }
}
