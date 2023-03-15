using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickManager : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("TestScene");
    }
    public void OnClickExitButton()
    {
        Application.Quit();
    }
}
