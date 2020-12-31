using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public UIController uiController;
    public void Continue()
    {
        uiController.ExitMenuExternal();
    }
    
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
