using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Photon 用の名前空間を参照する
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// ゲームの管理、ターン管理を行うコンポーネント
/// </summary>
public class TTTManager : MonoBehaviourPunCallbacks
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
    PhotonView m_photonView;

    /// <summary>
    /// どちらのターンかを返す
    /// </summary>
    public TttStatus Status
    {
        get { return m_status; }
    }

    void Start()
    {
        m_photonView = PhotonView.Get(this);
    }

    void Update()
    {

    }

    /// <summary>
    /// Start ボタンをクリックした時に呼ばれるハンドラ
    /// </summary>
    public void OnClickStartButton()
    {
        // 勝負がついていたら切断する
        if (m_status == TttStatus.End)
        {
            PhotonNetwork.Disconnect();
        }

        Connect("1.0");
    }

    /// <summary>
    /// ゲームをスタートさせる
    /// </summary>
    [PunRPC]
    void StartGame(string p1nickname, string p2nickname)
    {
        Debug.Log("Start Game");
        m_status = TttStatus.Player1;   // ターンをプレイヤー1 から始める
        SetPlayerTexts(p1nickname, p2nickname); // プレイヤー名を表示する
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
            MaskChecks();   // クリック可能なもの以外はクリックできないようにする
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
        MaskChecks();   // クリック可能なもの以外はクリックできないようにする
    }

    /// <summary>
    /// クリック可能な check のみクリックできるようにする
    /// </summary>
    void MaskChecks()
    {
        // 自分のターンの時は、まだクリックされていない check のみクリック可能にする
        if (PhotonNetwork.IsMasterClient && m_status == TttStatus.Player1)
        {
            UnmaskEmptyChecks();
        }
        else if (!PhotonNetwork.IsMasterClient && m_status == TttStatus.Player2)
        {
            UnmaskEmptyChecks();
        }
        else
        {
            // 自分のターンではないので、すべての check をクリックできないようにする
            foreach(var check in m_checkArray)
            {
                check.Enabled = false;
            }
        }
    }

    /// <summary>
    /// まだクリックされていない check のみクリック可能にする
    /// </summary>
    void UnmaskEmptyChecks()
    {
        foreach (var check in m_checkArray)
        {
            check.Enabled = (check.Text.Length == 0);
        }
    }

    /// <summary>
    /// プレイヤー名を設定する
    /// </summary>
    /// <param name="p1nickname"></param>
    /// <param name="p2nickname"></param>
    void SetPlayerTexts(string p1nickname, string p2nickname)
    {
        m_txtP1.text = p1nickname;
        m_txtP2.text = p2nickname;
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
            m_photonView.RPC("EndGame", RpcTarget.All, m_status);   // 現在ターンを持っているプレイヤーの勝ち
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
                m_photonView.RPC("EndGame", RpcTarget.All, TttStatus.None); // 引き分け
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
    [PunRPC]
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
        m_status = TttStatus.End;
    }

    /// <summary>
    /// Photonに接続する
    /// </summary>
    private void Connect(string gameVersion)
    {
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    /// <summary>
    /// ニックネームを付ける
    /// </summary>
    private void SetMyNickName(string nickName)
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("nickName: " + nickName);
            PhotonNetwork.LocalPlayer.NickName = nickName;
        }
    }

    /// <summary>
    /// ロビーに入る
    /// </summary>
    private void JoinLobby()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    /// <summary>
    /// 既に存在する部屋に参加する
    /// </summary>
    private void JoinExistingRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    /// <summary>
    /// ランダムな名前のルームを作って参加する
    /// </summary>
    private void CreateRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = true;   // 誰でも参加できるようにする
            roomOptions.MaxPlayers = 2;     // このゲームは二人でプレイする
            PhotonNetwork.CreateRoom(null, roomOptions); // ルーム名に null を指定するとランダムなルーム名を付ける
        }
    }

    /* これ以降は Photon の Callback メソッド */

    // Photonに接続した時
    public override void OnConnected()
    {
        Debug.Log("OnConnected");

        // ニックネームを付ける
        SetMyNickName(System.Environment.UserName + "@" + System.Environment.MachineName);
    }

    // Photonから切断された時
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
    }

    // マスターサーバーに接続した時
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        JoinLobby();
    }

    // ロビーに入った時
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        JoinExistingRoom();
    }

    // ロビーから出た時
    public override void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");
    }

    // 部屋を作成した時
    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    // 部屋の作成に失敗した時
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed");
    }

    // 部屋に入室した時
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }

    // 特定の部屋への入室に失敗した時
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed");
    }

    // ランダムな部屋への入室に失敗した時
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        CreateRandomRoom();
    }

    // 部屋から退室した時
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }

    // 他のプレイヤーが入室してきた時
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom: " + newPlayer.NickName);
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            Debug.Log("RPC Call: Start Game");
            m_photonView.RPC("StartGame", RpcTarget.All, PhotonNetwork.NickName, newPlayer.NickName);
        }
    }

    // 他のプレイヤーが退室した時
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom: " + otherPlayer.NickName);
    }

    // マスタークライアントが変わった時
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("OnMasterClientSwitched to: " + newMasterClient.NickName);
    }

    // ロビーに更新があった時
    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log("OnLobbyStatisticsUpdate");
    }

    // ルームリストに更新があった時
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
    }

    // ルームプロパティが更新された時
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("OnRoomPropertiesUpdate");
    }

    // プレイヤープロパティが更新された時
    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("OnPlayerPropertiesUpdate");
    }

    // フレンドリストに更新があった時
    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        Debug.Log("OnFriendListUpdate");
    }

    // 地域リストを受け取った時
    public override void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log("OnRegionListReceived");
    }

    // WebRpcのレスポンスがあった時
    public override void OnWebRpcResponse(OperationResponse response)
    {
        Debug.Log("OnWebRpcResponse");
    }

    // カスタム認証のレスポンスがあった時
    public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse");
    }

    // カスタム認証が失敗した時
    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("OnCustomAuthenticationFailed");
    }

    /// <summary>
    /// ターン状況を表す enum
    /// </summary>
    public enum TttStatus
    {
        None,
        Player1,
        Player2,
        End,
    }
}
