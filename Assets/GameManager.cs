using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UI (uGUI) を使うために追加した

/// <summary>
/// ゲーム全体をコントロールする
/// </summary>
[RequireComponent(typeof(SoundManager))]
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
    // サウンドマネージャー
    SoundManager m_soundManager;
    // タイムを表示する Text
    [SerializeField] Text m_timeText;
    // スコアを表示する Text
    [SerializeField] Text m_scoreText;
    // ゲームスタートを表示する Animation
    [SerializeField] Animation m_startAnim;
    // ゴールを表示する Animation
    [SerializeField] Animation m_goalAnim;

    void Start()
    {
        m_soundManager = GetComponent<SoundManager>();
        StartGame();
    }

    void Update()
    {
        // 経過時間を測る
        m_elapsedTime += Time.deltaTime;

        if (m_isInGame)
        {
            // タイムの表示を更新する
            string timeString = "Time: " + m_elapsedTime.ToString("F3") + " / " + m_timeLimit.ToString("F3");   // 書式指定文字列については「C# ToString 書式」で検索するとよいでしょう
            m_timeText.text = timeString;

            // タイムオーバーを判定する
            if (m_elapsedTime > m_timeLimit)
            {
                TimeOver();
            }
        }
    }

    /// <summary>
    /// ゲーム開始時に呼ぶ
    /// </summary>
    void StartGame()
    {
        StartCoroutine(StartGameImpl());
    }

    /// <summary>
    /// ゲーム開始時の処理
    /// </summary>
    /// <returns></returns>
    IEnumerator StartGameImpl()
    {
        Debug.Log("Start Game.");
        // Goal の時を隠す
        m_goalAnim.gameObject.SetActive(false);
        // アニメーションを再生する
        m_startAnim.Play();
        // 再生中は待つ
        while (m_startAnim.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        // 再生が終わったら初期化してゲームを始める
        m_score = 0;
        m_scoreText.text = "Score: " + m_score; // スコアの表示を初期化する
        m_elapsedTime = 0f;
        m_isInGame = true;
        m_soundManager.PlayBgm();
        yield return null;
    }

    /// <summary>
    /// ゴールした時に呼ぶ
    /// </summary>
    public void Goal()
    {
        Debug.Log("Goal.");
        m_isInGame = false;
        m_soundManager.StopBgm();
        m_soundManager.PlaySeGoal();
        m_goalAnim.gameObject.SetActive(true);
        m_goalAnim.Play();
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
            m_scoreText.text = "Score: " + m_score;
            m_soundManager.PlaySeGetItem();
        }
    }

    /// <summary>
    /// タイムオーバーした時に呼ぶ
    /// </summary>
    void TimeOver()
    {
        Debug.Log("Time over.");
        m_isInGame = false;
        m_soundManager.StopBgm();
        m_timeText.color = Color.red;   // タイムオーバーしたらタイム表示を赤くする
        m_soundManager.PlaySeTimeOver();
    }
}
