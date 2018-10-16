using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    /// <summary>振り幅(x)</summary>
    public float m_amplitude_x = 1f;
    /// <summary>振り幅(y)</summary>
    public float m_amplitude_y = 0f;
    /// <summary>振り幅(z)</summary>
    public float m_amplitude_z = 0f;
    /// <summary>動く速さ</summary>
    public float m_speed = 2.0f;
    private float m_timer;
    private Vector3 m_initialPosition;

    void Start()
    {
        m_initialPosition = transform.position;
    }

    void Update()
    {
        // オブジェクトを回す
        m_timer += Time.deltaTime * m_speed;
        float posX = Mathf.Sin(m_timer) * m_amplitude_x;
        float posY = Mathf.Sin(m_timer) * m_amplitude_y;
        float posZ = Mathf.Cos(m_timer) * m_amplitude_z;

        Vector3 pos = m_initialPosition;
        pos = pos + new Vector3(posX, posY, posZ);
        transform.position = pos;
    }

    // プレイヤーが上に乗った時、それを子オブジェクトとすることによりプレイヤーはオブジェクトと一緒に動く
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
            collision.collider.gameObject.transform.SetParent(transform);
    }

    // プレイヤーがオブジェクトから離れた時は、親子関係を解除する
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
            collision.collider.gameObject.transform.SetParent(null);
    }
}
