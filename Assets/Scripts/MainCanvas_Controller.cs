using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class MainCanvas_Controller : MonoBehaviour
{


    [SerializeField] private GameObject TamagotchiSObject = default;
    [SerializeField] private Animator TamagotchiSAnimator = default;
    [SerializeField] private Tamagotchi_Controller TamagotchiScript = default;
    [Space(10)]
    [SerializeField] private GameObject hudHolder = default;
    [Space(10)]
    [SerializeField] private GameObject mainPanelObject = default;
    [SerializeField] private Animator mainPanelAnimator = default;
    [Space(10)]
    //se cambio a public
    [SerializeField] private CameraManager cameraComponent = default;
    [Space(10)]
    [SerializeField] private GameObject previewHolder = default;
    [SerializeField] private RawImage rawImagePreview = default;
    [SerializeField] private AspectRatioFitter ratioComponent = default;
    [Space(10)]
    [SerializeField] private GameObject photoHudHolder = default;

    private readonly string activeTriggerString = "isOn";
    private readonly string forceTriggerString = "ForceOn";
    private readonly string forceOffTriggerString = "ForceOff";

    [Space(10)]
    [SerializeField] private AudioClip shutter = default;

    //traspaso de variables para la otra scene
    public static CameraManager cameratwo;
    public static Texture2D currentPreviewtwo;
    public static Texture2D textura;
    public static float rango;

    public GameObject infoCanvas;
    //public Image imageScannerHelp;
    
    private Texture2D currentPreviewPhoto; // foto tomada en cache
	private IEnumerator hidePanelRutine;


    private void Awake()
    {
        cameratwo = cameraComponent;
        
    }

    private IEnumerator HidePanelEnumerator(GameObject mainPanelObject)
	{
		yield return new WaitForSeconds(0.5F);
		mainPanelObject.gameObject.SetActive(false);

	}

	public void SetPreviewPhoto(Texture2D screenshotTexture, float ratio)
    {
        //previewHolder.SetActive(true);
        MainSystem.AppIsInPreview = true;

        //cambiar por una scene

        ratioComponent.aspectRatio = ratio;
        rango = ratioComponent.aspectRatio;
       
        //photoCanvas.ratioComponent.aspectRatio = ratio;
        //rawImagePreview.texture = screenshotTexture;
        textura = screenshotTexture;
        currentPreviewPhoto = screenshotTexture;
        currentPreviewtwo = currentPreviewPhoto;
        SceneManager.LoadSceneAsync("Photo_canvas", LoadSceneMode.Additive);



    }

    /*public void ClosePreview()
    {
        MainSystem.AppIsInPreview = false;

        cameraComponent.RestoreArCameraSystem();

        SceneManager.UnloadSceneAsync("Photo_canvas");
        //previewHolder.SetActive(false);
        
    }

    public void SharePreview()
    {
        //### METODO COMPARTIR

        Debug.LogWarning("COMPARTE FOTO");


        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, currentPreviewPhoto.EncodeToPNG());

        // To avoid memory leaks
        //Destroy(currentPreviewPhoto);

        new NativeShare().AddFile(filePath).SetSubject(" ").SetText(" ").Share();

        //### METODO COMPARTIR

        ClosePreview(); // cerrar panel para compartir
    }*/

    public void Start()
    {
        //   Debug.Log("Inicio!");
        infoCanvas.SetActive(false);
        Button_Home(); // al iniciar carga el home
    }

    public void TomarFoto()
    {
        MainSystem.PlayAudioclip(shutter);
        Debug.LogWarning("Tomar foto");

        cameraComponent.CaptureScreenshot();


    }

	private void HideMainPanel()
	{

		TamagotchiSObject.SetActive(false);

		Debug.Log("Hide main panel");
		if (mainPanelObject.activeInHierarchy)
		{
			mainPanelAnimator.SetBool(activeTriggerString, false);


			if (hidePanelRutine != null) { StopCoroutine(hidePanelRutine); };
			hidePanelRutine = HidePanelEnumerator(mainPanelObject);
			StartCoroutine(hidePanelRutine);
		};
	}



    public void Button_Info()
    {
        MainSystem.ClickSomething();
        Debug.LogWarning("Boton INFO");
        infoCanvas.SetActive(true);
     //   imageScannerHelp.color = new Color32(255, 255, 225, 0);
    }


	public void Button_ArMode()
	{
		MainSystem.ClickSomething();

		MainSystem.AppIsInPhotoMode = false;

		Debug.LogWarning("Boton ar AR");

		photoHudHolder.SetActive(false);


		HideMainPanel();

		MainSystem.AppCanTrackTargets = true; // activar reconocimiento
	}

	public void Button_PhotoMode()
	{
		MainSystem.ClickSomething();

		MainSystem.AppIsInPhotoMode = true;

		Debug.LogWarning("Boton FOTO");

		photoHudHolder.SetActive(true);

		HideMainPanel();

		MainSystem.AppCanTrackTargets = true; // activar reconocimiento

	}


	public void Button_Home()
	{

		MainSystem.ClickSomething();


		photoHudHolder.SetActive(false);

		MainSystem.AppIsInPreview = false;


		MainSystem.AppCanTrackTargets = false;
		MainSystem.ForceHideScene();

		Debug.LogWarning("Boton HOME");

		TamagotchiScript.EndGame();
		mainPanelObject.SetActive(true);
		mainPanelAnimator.SetTrigger(forceOffTriggerString);

		mainPanelAnimator.SetBool(activeTriggerString, true);
	}




    public void Button_Tamagochi()
    {
        MainSystem.ClickSomething();


        photoHudHolder.SetActive(false);

        Debug.LogWarning("Boton TAMAGOCHI");


        MainSystem.AppCanTrackTargets = false;

        hudHolder.SetActive(true);

        HideMainPanel();
        TamagotchiSObject.SetActive(true);
        TamagotchiSAnimator.SetBool(activeTriggerString, true);
        TamagotchiSAnimator.SetTrigger(forceTriggerString);

        TamagotchiScript.StartGame();
    }




	public void Button_Info_FB()
	{
	// perdio conexion con uso
		MainSystem.ClickSomething();
		Debug.LogWarning("Boton INFO FB");
		Application.OpenURL("https://instagram.com/ram.internationalexhibitions?igshid=1dd5xmyneb015");
	}

	public void Button_Info_Close()
	{
		// perdio conexion con uso

		MainSystem.ClickSomething();
		Debug.LogWarning("Boton INFO close");
		infoCanvas.SetActive(false);
		// imageScannerHelp.color = new Color32(255, 255, 225, 100);
	}



}