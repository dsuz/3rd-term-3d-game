using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コイン（プレーヤーが取ると点になるアイテム）に追加するコンポーネント
/// </summary>
public class CoinController : MonoBehaviour
{
    // 爆発エフェクトのプレハブ
    [SerializeField] GameObject m_effectPrefab;

    void Start()
    {
    }

    void Update()
    {

    }

    /// <summary>
    /// コインがプレーヤーに取られた時に呼ばれる
    /// </summary>
    void Taken()
    {
        Debug.Log("coin taken.");
        GameObject effect = Instantiate(m_effectPrefab);    // 爆発エフェクトを生成する
        effect.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Taken();
        }
    }
}
