using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : UIBinder
{
    public void Button_Lobby()
    {
        SceneManager.LoadScene(2);
    }
}
