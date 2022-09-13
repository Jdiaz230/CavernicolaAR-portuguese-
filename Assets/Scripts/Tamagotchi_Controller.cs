using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tamagotchi_Controller : MonoBehaviour
{
    private string[] textosCarne =
    {
        "O “homem de Neandertal” foi o primeiro fóssil humano identificado.",
        "O genoma neandertal demonstrou que todos os seres humanos, com exceção dos de origem subsaariana, somos portadores de genes neandertais.",
        "A tecnologia lítica surgiu há 2.500.000 anos na África Oriental",
        "O nome “homem das cavernas”, como são comumente chamados, surgiu porque a maioria dos fósseis do Neandertal encontrados na Eurásia foram descobertos em cavernas profundas."
    };

    private string[] textosSleep =
    {
        "Os neandertais compartilham com nossa espécie, da mutação do gene FOXP2, que está especificamente relacionado ao desenvolvimento da linguagem.",
        "O cérebro dos neandertais era maior que o da nossa espécie.",
        "Os neandertais pareciam ter estatura pequena e constituição robusta; uma adaptação importante para reter o calor em um clima tão frio. ",
        "Devido às baixas temperaturas, ficavam a maior parte do tempo dentro de cavernas para que pudessem se proteger do frio, e, acredita-se que devido a essa pouca exposição ao sol, tinham a pele clara."
    };

    private string[] textosBath =
    {
        "Os neandertais tinham um crescimento relativamente rápido se comparado ao da nossa espécie.",
        "Os neandertais usavam o fogo regularmente e diversas formas",
        "Os neandertais foram a primeira espécie a cremar seus mortos há 80 mil anos.",
        "Com relação às outras características físicas, provavelmente eram ruivos, e, possivelmente sardentos e teriam narizes proeminentes."
    };



    [SerializeField] private Button button_carne = default;
    [SerializeField] private Button button_sleep = default;
    [SerializeField] private Button button_bath = default;
    [Space(30)]

    [Range(0, 100)]
    [SerializeField] private float carneCounter = 0;
    [Range(0, 100)]
    [SerializeField] private float sleepCounter = 0;
    [Range(0, 100)]
    [SerializeField] private float bathCounter = 0;
    [Space(10)]



    [SerializeField] private float decreaseValue = 0.2F; // valor de decrecimiento

    public float CarneCounter { get => carneCounter; set => carneCounter = Mathf.Clamp(value, 0, 100); }
    public float SleepCounter { get => sleepCounter; set => sleepCounter = Mathf.Clamp(value, 0, 100); }
    public float BathCounter { get => bathCounter; set => bathCounter = Mathf.Clamp(value, 0, 100); }


    private bool isOnTeraphy = false; // determina si esta en la terapia


    private bool gameIsActive = false; // determina si esta en la terapia

    [Space(30)]
    [SerializeField] private AudioClip necesidadSound = default;
    [SerializeField] private AudioClip alarmSound = default;


    public void StartGame()
    {

        buttonsCanvasGroup.alpha = 1;


        Debug.Log("inicia comprobante para tamagochi");
        gameIsActive = true;

        Debug.Log("tamagochi start");
        CarneCounter = Random.Range(90, 100);
        SleepCounter = Random.Range(90, 100);
        BathCounter = Random.Range(90, 100);

        if (isOnTeraphy)
        {
            if (requirementsRutine != null) { StopCoroutine(requirementsRutine); };
            isOnTeraphy = false;

            bubbleAnimator.SetTrigger(trigger_ForceOff);

            Debug.Log("descarvitar forcozo globo");

        };

        cavemanAnimator.SetTrigger(trigger_Reset);  // reset al player

        buttonsCanvasGroup.alpha = 1;
        buttonsCanvasGroup.alpha = 1;
        buttonsCanvasGroup.alpha = 1;



    }

    public void EndGame()
    {
        Debug.Log("TERMINA comprobante para tamagochi o cualqui");
        gameIsActive = false;

    }


    private void CheckTeraphys()
    {
        if (CarneCounter < 50)
        {
            SendNewRequirement(cavemanRequirements.Carne);
            //currentTeraphy = cavemanRequirements.Carne;
            button_carne.interactable = true;
        }
        else
        {
            button_carne.interactable = false;

        };

        if (SleepCounter < 50)
        {
            SendNewRequirement(cavemanRequirements.Sleep);
            // currentTeraphy = cavemanRequirements.Sleep;
            button_sleep.interactable = true;
        }
        else
        {
            button_sleep.interactable = false;

        };

        if (BathCounter < 50)
        {
            SendNewRequirement(cavemanRequirements.Bath);
            //currentTeraphy = cavemanRequirements.Bath;
            button_bath.interactable = true;
        }
        else
        {
            button_bath.interactable = false;

        };



    }




    private void DisableAllButons()
    {
        buttonsCanvasGroup.alpha = 0;


        button_carne.interactable = false;
        button_sleep.interactable = false;
        button_bath.interactable = false;
    }


    public CanvasGroup buttonsCanvasGroup;

    public void Update()
    {

        if (gameIsActive && !isOnTeraphy)
        {
            if (CarneCounter > 0)
            {
                CarneCounter -= decreaseValue * Time.deltaTime;

            };

            if (SleepCounter > 0)
            {
                SleepCounter -= decreaseValue * Time.deltaTime;

            };

            if (BathCounter > 0)
            {
                BathCounter -= decreaseValue * Time.deltaTime;

            };

            CheckTeraphys();

        };



    }


    public Animator cavemanAnimator;

    public Animator msjBoxAnimator;

    public Text msjHolder;



    public enum cavemanRequirements { Carne, Sleep, Bath };
    public Animator bubbleAnimator;
    public Image bubbleImage;

    public Sprite bubble_carne;
    public Sprite bubble_sleep;
    public Sprite bubble_bath;



    public Image giftImage;


    //private cavemanRequirements currentTeraphy = default;

    private void FillRequirements(cavemanRequirements currentTeraphy)
    {
        if (isOnTeraphy)
        {
            DisableAllButons();

            if (requirementsRutine != null) { StopCoroutine(requirementsRutine); };
            requirementsRutine = RequirementsEnumerator(currentTeraphy);
            StartCoroutine(requirementsRutine);
        };
    }

    private int lastUsedIndexCarne = 0;
    private int lastUsedIndexBath = 0;
    private int lastUsedIndexSleep = 0;

    private IEnumerator requirementsRutine;

    private IEnumerator RequirementsEnumerator(cavemanRequirements currentTeraphy)
    {
        string finalText = "";

        int usedIndex = -1;


        switch (currentTeraphy)
        {
            case cavemanRequirements.Carne:
                giftImage.sprite = bubble_carne;
                CarneCounter = 100;

                do
                {
                    usedIndex = Random.Range(0, textosCarne.Length);
                }
                while (usedIndex == lastUsedIndexCarne);
                lastUsedIndexCarne = usedIndex;

                finalText = textosCarne[usedIndex];


                break;
            case cavemanRequirements.Sleep:
                giftImage.sprite = bubble_sleep;

                SleepCounter = 100;

                do
                {
                    usedIndex = Random.Range(0, textosSleep.Length);
                }
                while (usedIndex == lastUsedIndexSleep);
                lastUsedIndexSleep = usedIndex;

                finalText = textosSleep[usedIndex];

                break;
            case cavemanRequirements.Bath:
                giftImage.sprite = bubble_bath;

                BathCounter = 100;

                do
                {
                    usedIndex = Random.Range(0, textosBath.Length);
                }
                while (usedIndex == lastUsedIndexBath);

                lastUsedIndexBath = usedIndex;
                finalText = textosBath[usedIndex];
                break;
        };

        MainSystem.PlayAudioclip(necesidadSound);


        cavemanAnimator.SetTrigger(trigger_Gift); // eviar regalo
        bubbleAnimator.SetTrigger(trigger_Hide); // ocultar bubble
        cavemanAnimator.SetTrigger(trigger_Reset);  // reset al player

        yield return new WaitForSeconds(1); // esperar que llegue el regalo
        cavemanAnimator.SetTrigger(trigger_Light); // trigger luz
        yield return new WaitForSeconds(0.2F); // tiempo para recibir el regalo

        float segundosExtra = 0; // segundos necesarios paracontinuar

        switch (currentTeraphy)
        {
            case cavemanRequirements.Carne:
                string randomTrigger = ((Random.value > 0.5F) ? trigger_Yes : trigger_Clap);
                cavemanAnimator.SetTrigger(randomTrigger); // trigger feliz

                segundosExtra = 3;

                break;
            case cavemanRequirements.Sleep:
                cavemanAnimator.SetTrigger(trigger_GetSleep); // trigger feliz

                segundosExtra = 4;

                break;
            case cavemanRequirements.Bath:
                cavemanAnimator.SetTrigger(trigger_GetClean); // trigger feliz

                segundosExtra = 3;

                break;
        };



        yield return new WaitForSeconds(segundosExtra);
        SetMsjScreen(finalText);
    }


    private void SendNewRequirement(cavemanRequirements requirementIndex)
    {
        MainSystem.PlayAudioclip(alarmSound);

        isOnTeraphy = true;
        switch (requirementIndex)
        {
            case cavemanRequirements.Carne:
                bubbleImage.sprite = bubble_carne;
                cavemanAnimator.SetTrigger(trigger_Hungry);
                break;
            case cavemanRequirements.Sleep:
                bubbleImage.sprite = bubble_sleep;
                cavemanAnimator.SetTrigger(trigger_Sleepy);

                break;
            case cavemanRequirements.Bath:
                bubbleImage.sprite = bubble_bath;
                cavemanAnimator.SetTrigger(trigger_Dirty);

                break;
        };
        bubbleAnimator.SetTrigger(trigger_Show);
    }

    private readonly string trigger_Hungry = "Hungry";
    private readonly string trigger_Sleepy = "Sleepy";
    private readonly string trigger_Dirty = "Dirty";

    private readonly string trigger_Reset = "Reset";
    //private readonly string trigger_Hi = "Hi"; // idle taunt


    private readonly string trigger_GetClean = "GetClean";
    private readonly string trigger_GetSleep = "GetSleep";


    private readonly string trigger_Clap = "Clap"; // felicidad
    private readonly string trigger_Yes = "Yes"; // felicidad
    private readonly string trigger_Light = "Light"; // light
    private readonly string trigger_Gift = "Gift"; // Gift


    private readonly string trigger_Show = "Show"; // Generico Show
    private readonly string trigger_Hide = "Hide"; // Generico Hide

    private readonly string trigger_ForceOff = "ForceOff"; // Generico Hide


    public void Gift_Carne()
    {
        Debug.LogWarning("Regalo carne");

        FillRequirements(cavemanRequirements.Carne);
    }


    public void Gift_Sleep()
    {
        Debug.LogWarning("Regalo sueño");

        FillRequirements(cavemanRequirements.Sleep);

    }
    public void Gift_Bath()
    {
        Debug.LogWarning("Regalo baño");

        FillRequirements(cavemanRequirements.Bath);

    }


    private void SetMsjScreen(string msj)
    {
        msjHolder.text = msj;

        msjBoxAnimator.SetTrigger(trigger_Show);
    }

    public void ButtonMsj_Atras()
    {
        Debug.LogWarning("Boton Cerrar mensaje");
        msjBoxAnimator.SetTrigger(trigger_Hide);

        isOnTeraphy = false; // finaliza terapia

        buttonsCanvasGroup.alpha = 1;

    }

}
