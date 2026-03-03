using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] public GameObject mainMenu;
    [SerializeField] public GameObject playScreen;
    [SerializeField] public GameObject cardScreen;

    public void Play()
    {
        mainMenu.SetActive(false);
        playScreen.SetActive(true);

    }

    public void Back()
    {
        mainMenu.SetActive(true);
        playScreen.SetActive(false);
    }

    public void ShowCards()
    {
        cardScreen.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}