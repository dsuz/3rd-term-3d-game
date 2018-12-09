using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Photon 用の名前空間を参照する
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Check = OX の一つひとつの枠内にあるOXをコントロールする
/// </summary>
[RequireComponent(typeof(Button))]
public class CheckController : MonoBehaviourPunCallbacks
{
    /// <summary>ターンを管理しているマネージャー</summary>
    TTTManager m_tttManager;
    /// <summary>ボタンの Text</summary>
    Text m_buttonText;
    PhotonView m_photonView;

    /// <summary>
    /// Text つまり O か X か空白かを返す
    /// </summary>
    public string Text
    {
        get { return m_buttonText.text; }
    }

    /// <summary>
    /// クリックした時に動作するかを表すプロパティ
    /// false の時は押しても何も起きない
    /// </summary>
    public bool Enabled
    {
        get { return GetComponent<Button>().enabled; }
        set { GetComponent<Button>().enabled = value; }
    }

    /// <summary>
    /// ターンを管理しているマネージャーを設定する
    /// これをしないとターン管理できない
    /// </summary>
    /// <param name="manager"></param>
    public void SetManager(TTTManager manager)
    {
        m_tttManager = manager;
    }

    void Start()
    {
        m_buttonText = GetComponentInChildren<Text>();
        Enabled = false;    // 初期状態ではクリックできないようにする
        m_photonView = PhotonView.Get(this);
    }

    void Update()
    {

    }

    /// <summary>
    /// 初期化する
    /// </summary>
    public void Initialize()
    {
        m_buttonText.text = ""; // 表示を空にする
    }

    /// <summary>
    /// クリックイベントのハンドラー
    /// </summary>
    public void OnClick()
    {
        m_photonView.RPC("SetCheck", RpcTarget.All);
    }

    [PunRPC]
    void SetCheck()
    {
        // どちらのターンかに応じて表示を変える
        if (m_tttManager.Status == TTTManager.TttStatus.Player1)
        {
            m_buttonText.text = "O";
        }
        else if (m_tttManager.Status == TTTManager.TttStatus.Player2)
        {
            m_buttonText.text = "X";
        }

        this.Enabled = false;   // 一度クリックしたらもう押せないようにする
        m_tttManager.EndTurn(); // ターンが終わったことを通知する
    }
}
