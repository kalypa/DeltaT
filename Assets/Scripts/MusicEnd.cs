using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicEnd : MonoBehaviour
{

    private void Update()
    {
        EndMusic();
    }

    public void EndMusic()
    {
        if(SongManager.Instance.audioSource.time >= SongManager.Instance.audioSource.clip.length)
        {
            GameManager.Instance.offset = (int)(OffsetManager.Instance.GetOffset() * 1000);
            DontDestroyOnLoad(GameManager.Instance);
            SceneManager.LoadScene("TestScene");
        }
    }
}
