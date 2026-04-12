using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

void Update()
    {
        if (GameStore.Instance == null || GameStore.Instance.IsGameOver) return;
        PlayerController pc = PlayerController.Instance;
        if (pc != null && pc.IsBoosting)
            Debug.Log("BOOST: " + pc.boostTimer.ToString("F1") + "s");
    }

public void OnFinish() { GameStore.Instance?.OnLevelComplete(); if (LevelCompleteUI.Instance != null) LevelCompleteUI.Instance.ShowVictory(); else Time.timeScale = 0f; }

public void OnRespawn() { }
}
