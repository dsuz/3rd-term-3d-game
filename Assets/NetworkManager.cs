using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Photon 用の名前空間を参照する
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks // Photon Realtime 用のクラスを継承する
{
    /// <summary>プレイヤーのプレハブ名</summary>
    [SerializeField] string m_playerPrefabName = "PlayerPrefab";

    private void Awake()
    {
        // シーンの自動同期は無効にする
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    private void Start()
    {
        // Photon に接続する
        Connect("1.0"); // 1.0 はバージョン番号（適当）
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
            roomOptions.MaxPlayers = 20;    // 無料版の最大数まで参加できるようにする
            PhotonNetwork.CreateRoom(null, roomOptions); // ルーム名に null を指定するとランダムなルーム名を付ける
        }
    }

    /// <summary>
    /// プレイヤーを生成する
    /// </summary>
    private void SpawnPlayer()
    {
        GameObject player = PhotonNetwork.Instantiate(m_playerPrefabName, Vector3.zero, Quaternion.identity);   // プレイヤーを生成し、他のクライアントと同期する
        player.GetComponent<PlayerController>().enabled = true; // 自分だけ PlayerController を有効にする
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
        SpawnPlayer();
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
}
