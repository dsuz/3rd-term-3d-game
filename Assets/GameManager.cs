using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム全体をコントロールする
/// </summary>
public class GameManager : MonoBehaviour
{
    // ゲーム中かどうかを判定するフラグ
    bool m_isInGame;
    // ゲーム内での経過時間
    float m_elapsedTime = 0f;
    // スコア
    int m_score = 0;
    // ステージクリアの制限時間
    [SerializeField] float m_timeLimit = 30f;

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        // 経過時間を測り、タイムオーバーを判定する
        m_elapsedTime += Time.deltaTime;
        if (m_isInGame && m_elapsedTime > m_timeLimit)
        {
            TimeOver();
        }
    }

    /// <summary>
    /// ゲーム開始時に呼ぶ
    /// </summary>
    void StartGame()
    {
        Debug.Log("Start Game.");
        m_score = 0;
        m_elapsedTime = 0f;
        m_isInGame = true;
    }

    /// <summary>
    /// ゴールした時に呼ぶ
    /// </summary>
    public void Goal()
    {
        Debug.Log("Goal.");
        m_isInGame = false;
    }

    /// <summary>
    /// スコアを追加する時に呼ぶ
    /// </summary>
    public void AddScore(int score)
    {
        if (m_isInGame)
        {
            m_score += score;
            Debug.Log("Score: " + m_score);
        }
    }

    /// <summary>
    /// タイムオーバーした時に呼ぶ
    /// </summary>
    void TimeOver()
    {
        Debug.Log("Time over.");
        m_isInGame = false;
    }
}
