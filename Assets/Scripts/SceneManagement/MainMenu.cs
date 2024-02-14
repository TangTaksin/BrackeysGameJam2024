using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] TextMeshProUGUI pressAnyKeyText;
    [SerializeField] Image gameLogo;
    [SerializeField] Button doorButton;
    [SerializeField] Color textColor;
    [SerializeField] Color logoColor;

    private float targetAlpha = 0f;
    private float fadeDuration = 2f;
    private bool isFading = false;

    private void Start()
    {
        doorButton.interactable = false;
        SetInitialAlphaValues();
    }

    private void SetInitialAlphaValues()
    {
        pressAnyKeyText.color = new Color(textColor.r, textColor.g, textColor.b, 1f);
        gameLogo.color = new Color(logoColor.r, logoColor.g, logoColor.b, 1f);
    }

    private IEnumerator FadeObjects()
    {
        isFading = true;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(pressAnyKeyText.color.a, targetAlpha, elapsedTime / fadeDuration);
            pressAnyKeyText.color = new Color(textColor.r, textColor.g, textColor.b, newAlpha);
            gameLogo.color = new Color(logoColor.r, logoColor.g, logoColor.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetFinalAlphaValues();
        gameLogo.gameObject.SetActive(false);
        isFading = false;
        // Add additional logic here when the fade is complete
        if (!isFading)
        {
            doorButton.interactable = true;
        }
    }

     private IEnumerator DoorOpening()
    {
        yield return new WaitForSeconds(2);
        LoadScene();
    }


    public void ChangeImageAndLoadScene()
    {
        StartCoroutine(DoorOpening());
    }

    private void SetFinalAlphaValues()
    {
        pressAnyKeyText.color = new Color(textColor.r, textColor.g, textColor.b, targetAlpha);
        gameLogo.color = new Color(logoColor.r, logoColor.g, logoColor.b, targetAlpha);
    }

    private void Update()
    {
        if (Input.anyKey && !isFading)
        {
            StartCoroutine(FadeObjects());
        }
    }

    public void LoadScene()
    {
        Debug.Log("SceneChange");
        SceneManager.LoadScene(levelName);
    }
}
