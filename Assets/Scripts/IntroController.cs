using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroController : MonoBehaviour {
    [Header ("Scene elements")]
    public Image leftImage;
    public Image rightImage;
    public Image middleTextArea;
    public Image leftTextArea;
    public Image rightTextArea;
    public Text middleTextField;
    public Image gameLogo;
    public Text leftTextField;
    public Text rightTextField;
    public List<Sprite> artContainer;

    [Header ("Script variables")]
    public int currentSceneStage = 0;
    public bool textOnLeft = false;

    // intro dialogs

    private string textStart = "<game studio> presents\n" + 
        "Fenrir’s Legacy";
    private string text0 = "We are the children of The Wolf God Fenrir.\n" +
        "Our pack, initially weak, expanded and grew in power.";
    private string text1 = "Our people settled on an island that was given to us by the Norse gods.\n" + 
        "This is our home.";
    private string text2 = "With time our people divided into clans,\n" + 
        "settling into different areas of an island,\n" + 
        "yet with our combined might and power,\n" +
        "We were determined to keep enemies at bay.";
    private string text3 = "Recently, Thornspeakers Clan druids found corruption within their forests.\n" +
        "Angered by this, they brought it to the Council of three, seeking help.";
    private string text4 = "The Council of Three decided to set their strongest sons and daughters\n" + 
        "to investigate the strange corruption,\n" +
        "hoping it will strengthen the bond between clans.";
    private string text5 = "It is up to these heroes\n" + 
        "to venture through the wilds and bring desired peace.";
    // private string text6 = "text6";

    private List<string> textContainer;

    // Start is called before the first frame update
    void Start () {
        textContainer = new List<string> ();

        textContainer.Add (text0);
        textContainer.Add (text1);
        textContainer.Add (text2);
        textContainer.Add(text3);
        textContainer.Add(text4);
        textContainer.Add(text5);

        //middleTextField.text = textStart;
        StartCoroutine(fadeIN(middleTextField, 5));
        StartCoroutine(fadeINGameLogo(gameLogo, 3));
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown (0))  {
            if (currentSceneStage + 1 > textContainer.Count)
                SceneManager.LoadScene ("Battle");
            else
                nextIntroStep (currentSceneStage++);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene ("Battle");
        }
    }

    void nextIntroStep (int step) {
        StopAllCoroutines();

        if (middleTextArea.color.a > 0.0f) {
            //StartCoroutine (fadeOUT (middleTextField));
            //middleTextField.gameObject.SetActive(false);
            middleTextArea.gameObject.SetActive(false);
        }

        if (textOnLeft) {
            StartCoroutine (fadeOUT (rightImage));
            rightTextField.text = textContainer[step];
            StartCoroutine (fadeIN (rightTextField));

            StartCoroutine (fadeOUT (leftTextField));
            leftImage.sprite = artContainer[step];
            StartCoroutine (fadeIN (leftImage));
        } else {
            StartCoroutine (fadeOUT (leftImage));
            leftTextField.text = textContainer[step];
            StartCoroutine (fadeIN (leftTextField));

            StartCoroutine (fadeOUT (rightTextField));
            rightImage.sprite = artContainer[step];
            StartCoroutine (fadeIN (rightImage));
        }

        if (textOnLeft) textOnLeft = false;
        else textOnLeft = true;
    }

    public IEnumerator fadeIN (Text a) {
        int fadeSpeed = 2;

        while (a.color.a < 1.0f) {
            a.color = new Color (a.color.r, a.color.g,
                a.color.b, a.color.a + (Time.deltaTime * fadeSpeed));

            yield return null;
        }
    }

    public IEnumerator fadeIN (Image a) {
        int fadeSpeed = 2;

        while (a.color.a < 1.0f) {
            a.color = new Color (a.color.r, a.color.g,
                a.color.b, a.color.a + (Time.deltaTime / fadeSpeed));

            yield return null;
        }
    }

    public IEnumerator fadeOUT (Text a) {
        int fadeSpeed = 2;

        while (a.color.a > 0.0f) {
            a.color = new Color (a.color.r, a.color.g,
                a.color.b, a.color.a - (Time.deltaTime * fadeSpeed));

            yield return null;
        }
    }
    public IEnumerator fadeOUT (Image a) {
        int fadeSpeed = 2;

        while (a.color.a > 0.0f) {
            a.color = new Color (a.color.r, a.color.g,
                a.color.b, a.color.a - (Time.deltaTime / fadeSpeed));

            yield return null;
        }
    }

    public IEnumerator fadeIN (Text a, float fadeSpeed) {
        while (a.color.a < 1.0f) {
            a.color = new Color (a.color.r, a.color.g,
                a.color.b, a.color.a + (Time.deltaTime / fadeSpeed));

            yield return null;
        }
    }

    IEnumerator SleepForSeconds (int sec) {
        yield return new WaitForSeconds (sec);
    }

    public IEnumerator fadeINGameLogo (Image a, float fadeSpeed) {

        yield return new WaitForSeconds (2);

        while (a.color.a < 1.0f) {
            a.color = new Color (a.color.r, a.color.g,
                a.color.b, a.color.a + (Time.deltaTime / fadeSpeed));

            yield return null;
        }
    }

}