using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームの管理、ターン管理を行うコンポーネント
/// </summary>
public class TTTManager : MonoBehaviour
{
    /// <summary>Check をすべて格納した配列</summary>
    [SerializeField] CheckController[] m_checkArray;
    /// <summary>プレイヤー1 を表示するテキスト</summary>
    [SerializeField] Text m_txtP1;
    /// <summary>プレイヤー2 を表示するテキスト</summary>
    [SerializeField] Text m_txtP2;
    /// <summary>ゲームスタートするためのボタン</summary>
    [SerializeField] Button m_startButton;
    /// <summary>ターンステート</summary>
    TttStatus m_status;

    /// <summary>
    /// どちらのターンかを返す
    /// </summary>
    public TttStatus Status
    {
        get { return m_status; }
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    /// <summary>
    /// ゲームをスタートさせる
    /// </summary>
    public void StartGame()
    {
        m_status = TttStatus.Player1;   // ターンをプレイヤー1 から始める
        HighlightPlayerText();  // 現在ターン中のプレイヤー側の表示を赤くする
        InitializeAllChecks();  // Check を初期化する
        m_startButton.gameObject.SetActive(false);  // スタートボタンは消す
    }

    /// <summary>
    /// 各 Check をすべて初期化する
    /// </summary>
    void InitializeAllChecks()
    {
        foreach (var check in m_checkArray)
        {
            check.SetManager(this); // ステート管理できるように参照を渡す
            check.Enabled = true;   // クリック可能にする
            check.Initialize(); // OXを消す
        }
    }

    /// <summary>
    /// ターンを切り替える
    /// </summary>
    void SwitchTurn()
    {
        if (m_status == TttStatus.Player1)
        {
            m_status = TttStatus.Player2;
        }
        else if (m_status == TttStatus.Player2)
        {
            m_status = TttStatus.Player1;
        }
        HighlightPlayerText();  // プレイヤー表示のハイライトを切り替える
    }

    /// <summary>
    /// プレイヤー表示のハイライトを切り替える
    /// </summary>
    void HighlightPlayerText()
    {
        if (m_status == TttStatus.Player1)
        {
            m_txtP1.color = Color.red;
            m_txtP2.color = Color.black;
        }
        else if (m_status == TttStatus.Player2)
        {
            m_txtP1.color = Color.black;
            m_txtP2.color = Color.red;
        }
    }

    /// <summary>
    /// ターンが終わった時に呼び出される
    /// </summary>
    public void EndTurn()
    {
        // 勝利条件チェック
        if ((m_checkArray[0].Text == m_checkArray[1].Text && m_checkArray[1].Text == m_checkArray[2].Text && m_checkArray[0].Text.Length > 0) ||
            (m_checkArray[3].Text == m_checkArray[4].Text && m_checkArray[4].Text == m_checkArray[5].Text && m_checkArray[3].Text.Length > 0) ||
            (m_checkArray[6].Text == m_checkArray[7].Text && m_checkArray[7].Text == m_checkArray[8].Text && m_checkArray[6].Text.Length > 0) ||
            (m_checkArray[0].Text == m_checkArray[3].Text && m_checkArray[3].Text == m_checkArray[6].Text && m_checkArray[0].Text.Length > 0) ||
            (m_checkArray[1].Text == m_checkArray[4].Text && m_checkArray[4].Text == m_checkArray[7].Text && m_checkArray[1].Text.Length > 0) ||
            (m_checkArray[2].Text == m_checkArray[5].Text && m_checkArray[5].Text == m_checkArray[8].Text && m_checkArray[2].Text.Length > 0) ||
            (m_checkArray[0].Text == m_checkArray[4].Text && m_checkArray[4].Text == m_checkArray[8].Text && m_checkArray[0].Text.Length > 0) ||
            (m_checkArray[2].Text == m_checkArray[4].Text && m_checkArray[4].Text == m_checkArray[6].Text && m_checkArray[2].Text.Length > 0))
        {
            Debug.Log("Game End");
            EndGame(m_status);  // 現在ターンを持っているプレイヤーの勝ち
        }
        else
        {
            // 引き分けをチェックする
            bool isDraw = true;

            foreach(var check in m_checkArray)
            {
                isDraw = isDraw && check.Text.Length > 0;
            }

            if (isDraw)
            {
                EndGame(TttStatus.None);    // 引き分け
            }
        }

        // ターンを入れ替える
        SwitchTurn();
    }

    /// <summary>
    /// 全てのチェックを操作可能/操作不可能にする
    /// </summary>
    /// <param name="flag"></param>
    void SetEnableToAllChecks(bool flag)
    {
        foreach (var check in m_checkArray)
        {
            check.Enabled = flag;
        }
    }

    /// <summary>
    /// ゲーム終了時に呼び出す
    /// </summary>
    /// <param name="player">勝ったプレイヤー</param>
    void EndGame(TttStatus player)
    {
        SetEnableToAllChecks(false);    // これ以上 Check を操作できないようにする

        if (player == TttStatus.Player1)
        {
            Debug.Log("Player1 wins.");
        }
        else if (player == TttStatus.Player2)
        {
            Debug.Log("Player2 wins.");
        }
        else
        {
            Debug.Log("Draw");
        }

        m_startButton.gameObject.SetActive(true);   // スタートボタンを表示する
    }

    /// <summary>
    /// ターン状況を表す enum
    /// </summary>
    public enum TttStatus
    {
        None,
        Player1,
        Player2,
    }
}
