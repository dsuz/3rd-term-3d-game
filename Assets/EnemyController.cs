using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵を制御するスクリプト
/// </summary>
[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    Animator m_anim;
    CharacterController m_charCtrl;
    GameObject m_player;

    // 移動速度
    [SerializeField] float m_speed = 2f;

    void Start()
    {
        // 各種コンポーネントを取得し、変数に格納する
        m_anim = GetComponent<Animator>();
        m_anim.SetFloat("Speed", 1f);   // これで Idle → Walk に遷移する
        m_charCtrl = GetComponent<CharacterController>();
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.LookAt(m_player.transform);   // プレイヤーの方を向く
        m_charCtrl.SimpleMove(m_speed * transform.forward); // 前進する。SimpleMove() は重力の影響を受ける。
    }

    private void OnTriggerStay(Collider other)
    {
        // Player が Trigger の中にいたら攻撃する
        if (other.gameObject.tag == "Player")
        {
            Attack();
        }
    }

    void Attack()
    {
        // プレイヤーの方を見て、アニメーションを制御する
        transform.LookAt(m_player.transform);
        m_anim.SetTrigger("Attack");
    }
}
