using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStore : MonoBehaviour
{
    public static GameStore Instance { get; private set; }
    public static event Action OnGameOver;

    public string PlayerName     { get; set; } = "Player";
    public float  LevelTime      { get; private set; }
    public int    Collisions     { get; private set; }
    public int    CoinsCollected { get; private set; }
    public int    Lives          { get; private set; } = 3;
    public bool   IsGameOver     { get; private set; }
    public bool   IsGameStarted  { get; set; } = false;

    [Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public float  timeTaken;
        public int    coinsCollected;
        public int    collisions;
    }

    public List<LeaderboardEntry> Leaderboard { get; private set; } = new List<LeaderboardEntry>();
    private const string LBKey = "LB_v1";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        string raw = PlayerPrefs.GetString(LBKey, "");
        if (!string.IsNullOrEmpty(raw))
        {
            var w = JsonUtility.FromJson<LBWrap>(raw);
            if (w != null && w.entries != null) Leaderboard = w.entries;
        }
    }

    void Update() { if (!IsGameStarted || IsGameOver) return; LevelTime += Time.deltaTime; }

    public void RegisterCollision() { if (!IsGameOver) Collisions++; }
    public void CollectCoin()       { if (!IsGameOver) CoinsCollected++; }
    public void LoseLife()
    {
        if (IsGameOver) return;
        Lives--;
        if (Lives <= 0) { Lives = 0; EndGame(true); }
    }
    public void OnLevelComplete() { if (!IsGameOver) EndGame(false); }
    public void TriggerGameOver() { if (!IsGameOver) EndGame(true); }

    private void EndGame(bool lost)
    {
        IsGameOver = true;
        Time.timeScale = 0f;
        Leaderboard.Add(new LeaderboardEntry
        {
            playerName = PlayerName, timeTaken = LevelTime,
            coinsCollected = CoinsCollected, collisions = Collisions
        });
        Leaderboard.Sort((a, b) =>
        {
            int c = a.collisions.CompareTo(b.collisions);
            return c != 0 ? c : a.timeTaken.CompareTo(b.timeTaken);
        });
        if (Leaderboard.Count > 10) Leaderboard.RemoveAt(Leaderboard.Count - 1);
        PlayerPrefs.SetString(LBKey, JsonUtility.ToJson(new LBWrap { entries = Leaderboard }));
        PlayerPrefs.Save();
        if (lost) OnGameOver?.Invoke();
    }

    public void ResetSession()
    {
        LevelTime = 0f; Collisions = 0; CoinsCollected = 0;
        Lives = 3; IsGameOver = false; IsGameStarted = false;
        Time.timeScale = 1f;
    }

    [Serializable]
    private class LBWrap { public List<LeaderboardEntry> entries; }
}
