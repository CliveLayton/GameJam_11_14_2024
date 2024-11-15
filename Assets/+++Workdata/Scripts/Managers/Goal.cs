using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private CanvasGroup winPanel;
    [SerializeField] private string addLevelName;
    
    private GameManager gameManager;
    private MusicManager musicManager;
    
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        musicManager = FindObjectOfType<MusicManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            musicManager.PlaySFX(musicManager.winGame);
            if (addLevelName != null)
            {
                gameManager.unlockedLevel.Add(addLevelName);
            }
            winPanel.ShowCanvasGroup();
            Time.timeScale = 0f;
        }
    }
}
