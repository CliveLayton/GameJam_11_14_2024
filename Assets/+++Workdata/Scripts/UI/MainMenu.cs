using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenuScreen;
    [SerializeField] private CanvasGroup levelSelectionScreen;
    [SerializeField] private CanvasGroup optionScreen;

    [SerializeField] private Texture2D cursorTexture;

    [SerializeField] private Button[] levelButtons;

    [SerializeField] private Button[] buttons;

    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        Time.timeScale = 1f;
        gameManager = FindAnyObjectByType<GameManager>();
        Cursor.SetCursor(cursorTexture,new Vector2(cursorTexture.width * 0.5f,cursorTexture.height * 0.5f), CursorMode.Auto);
    }

    private void Start()
    {

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(MusicManager.Instance.PlayButtonSFX);
        }
        
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = false;
        }
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void ShowMainMenu()
    {
        mainMenuScreen.ShowCanvasGroup();
        levelSelectionScreen.HideCanvasGroup();
        optionScreen.HideCanvasGroup();
    }

    public void ShowLevelSelection()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        for (int i = 0; i < gameManager.unlockedLevel; i++)
        {
            levelButtons[i].interactable = true;
        }

        mainMenuScreen.HideCanvasGroup();
        levelSelectionScreen.ShowCanvasGroup();
    }

    public void ShowOptions()
    {
        mainMenuScreen.HideCanvasGroup();
        optionScreen.ShowCanvasGroup();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
