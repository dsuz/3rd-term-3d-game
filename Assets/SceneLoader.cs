using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Color m_fadeColor = Color.black;
    [SerializeField] float m_fadeMultiplier = 1f;

    /// <summary>
    /// フェードしながらシーンを切り替える
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        Initiate.Fade(sceneName, m_fadeColor, m_fadeMultiplier);
    }
}
