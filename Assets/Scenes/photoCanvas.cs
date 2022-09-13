using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
public class photoCanvas : MonoBehaviour
{
  
    
    public static Texture2D screenshotTexture;
    public static float ratio;
    public static RawImage rawImagePreview;
    public static AspectRatioFitter ratioComponent;

    // Start is called before the first frame update

    private void Awake()
    {
        rawImagePreview = GameObject.Find("Foto").GetComponent<RawImage>();
        ratioComponent = GameObject.Find("Foto").GetComponent<AspectRatioFitter>();
        ratioComponent.aspectRatio = MainCanvas_Controller.rango ;
        rawImagePreview.texture = MainCanvas_Controller.textura;
    }
    public void ClosePreview()
    {
        MainSystem.AppIsInPreview = false;

        MainCanvas_Controller.cameratwo.RestoreArCameraSystem();
        SceneManager.UnloadSceneAsync("Photo_canvas");
        //previewHolder.SetActive(false);

    }
    public void SharePreview()
    {
        //### METODO COMPARTIR

        Debug.LogWarning("COMPARTE FOTO");


        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath,MainCanvas_Controller.currentPreviewtwo.EncodeToPNG());

        // To avoid memory leaks
        //Destroy(currentPreviewPhoto);

        new NativeShare().AddFile(filePath).SetSubject(" ").SetText(" ").Share();

        //### METODO COMPARTIR

        ClosePreview(); // cerrar panel para compartir
    }
}
