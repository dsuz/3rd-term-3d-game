using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 右クリックを押しながらマウス操作で視点を操作するためのコンポーネント
/// </summary>
[RequireComponent(typeof(Camera))]
public class ThirdPersonViewController : MonoBehaviour
{
    // カメラによる画角の中心になるオブジェクト
    [SerializeField] Transform m_target;
    // カメラの回転速度
    [SerializeField] float m_rotationSpeed = 5f;
    // ズームの速度
    [SerializeField] float m_zoomSpeed = 5f;
    // 直前のフレームでの target の座標
    Vector3 m_lastTargetPosition;

    void Start()
    {
        m_lastTargetPosition = m_target.position;   // 現在のターゲットの座標を保存する
    }

    void Update()
    {
        transform.position += m_target.position - m_lastTargetPosition; // ターゲットが動いた分だけ自分も動く
        float mouseX = 0f;  // mouseX はマウスが横方向に動いた距離

        // マウスの入力を受け付ける
        if (Input.GetMouseButton(1))    // 右クリックが押されている時
        {
            mouseX = Input.GetAxis("Mouse X");
        }

        transform.LookAt(m_target); // ターゲットの方を向く
        transform.RotateAround(m_target.position, Vector3.up, mouseX * m_rotationSpeed);    // ターゲットを中心に Y 軸の周りを回転する

        // ホイール回転でズームを変える処理
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        GetComponent<Camera>().fieldOfView -= scrollWheel * m_zoomSpeed;

        m_lastTargetPosition = m_target.position;   // 現在のターゲットの座標を保存する
    }
}
