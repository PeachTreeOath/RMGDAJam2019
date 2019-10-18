using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : Singleton<SceneSwitch>
{
    public void GotoGame()
    {
        SceneManager.LoadScene("Game");
    }
}
