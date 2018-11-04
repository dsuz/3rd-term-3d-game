using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトを 3D 空間で動かしたり、ジャンプさせるコンポーネント
/// </summary>
[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // ジャンプ力を調整するパラメーター
    [SerializeField] float m_jumpPower = 10f;
    // 動く速度を調整するパラメーター
    [SerializeField] float m_moveSpeed = 5f;
    // 重力加速度を調整するパラメーター
    [SerializeField] float m_gravityMultiplier = 1f;
    // 縦方向の速度
    float m_verticalVelocity = 0f;
    // 同じオブジェクトに追加された Character Controller への参照
    CharacterController m_charCtrl;
    // 同じオブジェクトに追加された Animator への参照
    Animator m_anim;

    void Start()
    {
        m_charCtrl = GetComponent<CharacterController>();
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 方向の入力を取得する
        float v = Input.GetAxisRaw("Vertical");     // v は vertical の略。つまり垂直方向の入力を表す
        float h = Input.GetAxisRaw("Horizontal");   // h は horizontal の略。つまり水平方向の入力を表す
        Vector3 dir = Vector3.zero; // dir は direction の略。移動する方向を表す。ここでは移動する方向の速度ベクトルを表す
        
        // x-z 平面（地面と平行）の速度を求める
        dir += new Vector3(h, 0, v) * m_moveSpeed;  // 方向の入力で、x-z 平面の移動方向が決まる

        // 方向キーの入力があったら、キャラの方向をそちらに向ける
        if (dir != Vector3.zero)
        {
            dir = Camera.main.transform.TransformDirection(dir);    // カメラを基準とした方向に変換する
            Vector3 forwardDir = dir;   // forwardDir は「このオブジェクトの前方」を表すベクトル
            forwardDir.y = 0;   // カメラが傾いていてもキャラは傾いて欲しくないので、水平方向を向ける
            transform.forward = forwardDir;
        }

        // y 軸（垂直方向）の速度を求める
        if (m_charCtrl.isGrounded)  // キャラが設置しているか
        {
            // アニメーションを制御する
            m_anim.SetBool("IsGrounded", true);
            m_anim.SetFloat("Speed", dir.sqrMagnitude); // 着地している時かつ動いている時は Run に遷移する、動いていなければ Idle にとどまる

            m_verticalVelocity = 0; // 接地していれば垂直方向の速度はゼロになる

            // ジャンプの入力を受け付ける
            if (Input.GetButton("Jump"))
            {
                m_verticalVelocity += m_jumpPower;
                m_anim.SetBool("IsGrounded", false);    // Jump に遷移する
            }
        }
        else
        {
            // 接地していない時（空中にいる時）は重力に従って垂直方向の速度は減速し、マイナスになる
            m_verticalVelocity -= Physics.gravity.magnitude * m_gravityMultiplier * Time.deltaTime;
        }

        dir.y = m_verticalVelocity; // y 軸（垂直方向）の速度が決まった

        m_charCtrl.Move(dir * Time.deltaTime);  // ここでキャラクターを動かす
    }
}
