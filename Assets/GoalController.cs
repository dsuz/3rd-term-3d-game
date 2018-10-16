using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゴール（プレイヤーが衝突するとステージクリアとなるオブジェクト）に追加するコンポーネント
/// </summary>
public class GoalController : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// ゴールした時に呼び出す
    /// </summary>
    void Goal()
    {
        Debug.Log("Goal.");
        GameObject go = GameObject.Find("Manager");
        GameManager manager = go.GetComponent<GameManager>();
        manager.Goal();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Goal();
        }
    }
}
