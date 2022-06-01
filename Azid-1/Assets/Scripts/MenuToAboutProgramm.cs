using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuToAboutProgramm : MonoBehaviour
{
  // Методы для перехода между сценами:
    public void JumpToScene(int _sceneNumber)
    {
       SceneManager.LoadScene(_sceneNumber);
    }
}
