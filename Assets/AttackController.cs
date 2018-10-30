using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の攻撃を制御するコンポーネント
/// 攻撃の有効範囲を表す Trigger と同じ GameObject に追加して使う
/// </summary>
public class AttackController : MonoBehaviour
{
    [SerializeField] float m_attackPower = 15f;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // Player が攻撃の有効範囲にいたら攻撃の影響を加える
        if (other.gameObject.tag == "Player")
        {
            Attack(other.gameObject);
        }
    }

    /// <summary>
    /// 攻撃の影響を加える
    /// </summary>
    /// <param name="target">影響を加える相手</param>
    void Attack(GameObject target)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        // もし相手に Rigidbody が追加されていたら、斜め上に力を加える
        if (rb)
        {
            rb.AddForce((transform.forward + Vector3.up).normalized * m_attackPower, ForceMode.Impulse);
        }
    }
}
