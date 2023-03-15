using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    public GameObject endUI;
    private void Update()
    {
        End1();
    }
    public void End1()
    {
        if (SongManager.Instance.audioSource.time >= SongManager.Instance.audioSource.clip.length)
        {
            endUI.SetActive(true);
            EndUI.Instance.EndTexts();
        }
    }
}
