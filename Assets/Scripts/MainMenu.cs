using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject authorsTab;
    public GameObject artSpotlight;
    public Text authorsButtonText;

    [Header("Art Spotlight Components")]
    public Text artSpotlightText;
    public Image artSpotlightImage;
    public Sprite[] artArray;
    int currentArtNumber = 0;

    public void PlayGameButton()
    {
        SceneManager.LoadScene("Battle");
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

    public void AuthorsButton()
    {
        if(authorsTab.activeSelf == false){
            authorsTab.SetActive(true);
            artSpotlight.SetActive(false);
            authorsButtonText.text = "Art Spotlight";
        }
        else {
            authorsTab.SetActive(false);
            artSpotlight.SetActive(true);
            authorsButtonText.text = "Authors";
        }
    }

    public void artSpotlightPrev()
    {
        if(currentArtNumber - 1 < 0)
            currentArtNumber = artArray.Length  - 1;
        else
            --currentArtNumber;

        updateArtSpotlightText();
    }

    public void artSpotlightNext()
    {
        if(currentArtNumber + 1 == artArray.Length)
            currentArtNumber = 0;
        else
            ++currentArtNumber;

        updateArtSpotlightText();
    }

    void updateArtSpotlightText()
    {
        artSpotlightImage.sprite = artArray[currentArtNumber];
        artSpotlightText.text = (currentArtNumber + 1) +  " / " + artArray.Length;
    }
}

