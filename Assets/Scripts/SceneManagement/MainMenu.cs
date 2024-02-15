using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private TextMeshProUGUI pressAnyKeyText;
    [SerializeField] private Image gameLogo;
    [SerializeField] private Button doorButton;
    [SerializeField] private Image fadeOverlay; // Reference to the black screen overlay
    [SerializeField] private Color textColor;
    [SerializeField] private Color logoColor;

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
        SetAlpha(pressAnyKeyText, 1f);
        SetAlpha(gameLogo, 1f);
        SetAlpha(fadeOverlay, 0f); // Initially set the fade overlay to be transparent
    }

    private void SetAlpha(Graphic graphic, float alpha)
    {
        graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }

    private IEnumerator FadeObjects()
    {
        isFading = true;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(pressAnyKeyText.color.a, targetAlpha, elapsedTime / fadeDuration);
            SetAlpha(pressAnyKeyText, newAlpha);
            SetAlpha(gameLogo, newAlpha);

            doorButton.image.color = Color.Lerp(doorButton.image.color, Color.white, elapsedTime / fadeDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameLogo.gameObject.SetActive(false);
        isFading = false;
        doorButton.interactable = true;
    }

    private IEnumerator DoorOpening()
    {
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(3.3f);
        LoadScene();

    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Fade the overlay to black
            SetAlpha(fadeOverlay, Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration));

            elapsedTime += Time.deltaTime;
            yield return null;
        }


    }

    public void ChangeImageAndLoadScene()
    {
        StartCoroutine(DoorOpening());
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
