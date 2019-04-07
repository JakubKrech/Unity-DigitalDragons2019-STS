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
    public Text leftTextField;
    public Text rightTextField;
    public List<Sprite> artContainer;

    [Header ("Script variables")]
    public int currentSceneStage = 0;
    public bool textOnLeft = false;

    // intro dialogs

    private string textStart = "So here we are.\n\n" +
        "People been dying, but now its ok.\n" +
        "Because kids are on the mission\n" +
        "Lets hope its gud.\n\n" +
        "Cya.";
    private string text0 = "text0";
    private string text1 = "text1";
    private string text2 = "text2";
    private string text3 = "text3";
    private string text4 = "text4";
    private string text5 = "text5";
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

        middleTextField.text = textStart;
        StartCoroutine(fadeIN(middleTextField, 5));
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
            StartCoroutine (fadeOUT (middleTextField));
            middleTextField.gameObject.SetActive(false);
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

}