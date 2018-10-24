using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGM, SE を管理するクラス。GameManager と同じ GameObject に追加して使う。
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    // BGM
    [SerializeField] AudioClip m_bgm;
    // アイテムを取った時の SE
    [SerializeField] AudioClip m_getItem;
    // ゴールした時の SE
    [SerializeField] AudioClip m_goal;
    AudioSource m_audioSource;

    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    /// <summary>
    /// BGM を再生する
    /// </summary>
    public void PlayBgm()
    {
        m_audioSource.clip = m_bgm;
        m_audioSource.loop = true;
        m_audioSource.Play();
    }

    /// <summary>
    /// BGM を止める
    /// </summary>
    public void StopBgm()
    {
        if (m_audioSource.isPlaying)
        {
            m_audioSource.Stop();
        }
    }

    /// <summary>
    /// アイテム取得時の SE を鳴らす
    /// </summary>
    public void PlaySeGetItem()
    {
        m_audioSource.PlayOneShot(m_getItem);
    }

    /// <summary>
    /// ゴール時の SE を鳴らす
    /// </summary>
    public void PlaySeGoal()
    {
        m_audioSource.PlayOneShot(m_goal);
    }
}
