using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionBetweenScenes : MonoBehaviour
{
  // Методы для перехода между сценами:
    public void JumpToScene(int _sceneNumber)
    {
       SceneManager.LoadScene(_sceneNumber);
    }

    public void Exit(){
      Application.Quit();
    }
}
