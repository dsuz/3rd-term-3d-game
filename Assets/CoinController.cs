using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コイン（プレーヤーが取ると点になるアイテム）に追加するコンポーネント
/// </summary>
public class CoinController : MonoBehaviour
{
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
