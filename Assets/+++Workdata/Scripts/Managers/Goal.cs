using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private CanvasGroup winPanel;
    [SerializeField] private bool unlockLevel;
    
    private GameManager gameManager;
    private MusicManager musicManager;
    
    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        musicManager = FindAnyObjectByType<MusicManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            musicManager.PlaySFX(musicManager.winGame);
            if (unlockLevel)
            {
                gameManager.unlockedLevel++;
            }
            winPanel.ShowCanvasGroup();
            Time.timeScale = 0f;
        }
    }
}
