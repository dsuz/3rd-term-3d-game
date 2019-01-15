using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトを 3D 空間で動かしたり、ジャンプさせるコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class PlayerControllerRigidbody : MonoBehaviour
{
    // ジャンプ力を調整するパラメーター
    [SerializeField] float m_jumpPower = 3f;
    // 動く速度を調整するパラメーター
    [SerializeField] float m_moveSpeed = 10f;
    // 重力加速度を調整するパラメーター
    //[SerializeField] float m_gravityMultiplier = 1f;
    // 縦方向の速度
    // float m_verticalVelocity = 0f;
    // 同じオブジェクトに追加された Rigidbody への参照
    Rigidbody m_rb;
    // 同じオブジェクトに追加された Animator への参照
    Animator m_anim;

    bool m_isGrounded;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 方向の入力を取得する
        float v = Input.GetAxisRaw("Vertical");     // v は vertical の略。つまり垂直方向の入力を表す
        float h = Input.GetAxisRaw("Horizontal");   // h は horizontal の略。つまり水平方向の入力を表す
        Vector3 dir = Vector3.zero; // dir は direction の略。移動する方向を表す。ここでは移動する方向の速度ベクトルを表す

        // 入力方向のベクトルを作る
        dir += new Vector3(h, 0, v);

        // 方向キーの入力があったら、キャラの方向をそちらに向ける
        if (dir != Vector3.zero)
        {
            Vector3 temp_dir = Camera.main.transform.TransformDirection(dir);    // カメラを基準とした方向に変換する
            Vector3 forwardDir = temp_dir;   // forwardDir は「このオブジェクトの前方」を表すベクトル
            forwardDir.y = 0;   // カメラが傾いていてもキャラは傾いて欲しくないので、水平方向を向ける
            transform.forward = forwardDir;
            // キャラクターを動かす
            m_rb.AddForce(transform.forward * m_moveSpeed);
        }

        if (m_isGrounded)  // キャラが設置しているか
        {
            // アニメーションを制御する
            m_anim.SetFloat("Speed", dir.sqrMagnitude); // 着地している時かつ動いている時は Run に遷移する、動いていなければ Idle にとどまる

            // ジャンプの入力を受け付ける
            if (Input.GetButton("Jump"))
            {
                m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
                m_anim.SetBool("IsGrounded", false);    // Jump に遷移する
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        m_isGrounded = true;
        m_anim.SetBool("IsGrounded", true);
    }

    private void OnTriggerStay(Collider other)
    {
        m_isGrounded = true;
        m_anim.SetBool("IsGrounded", true);
    }

    private void OnTriggerExit(Collider other)
    {
        m_isGrounded = false;
        m_anim.SetBool("IsGrounded", false);
    }
}
