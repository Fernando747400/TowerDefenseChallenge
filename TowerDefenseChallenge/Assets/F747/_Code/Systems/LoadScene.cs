using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [Header("Settings")]
    [Scene]
    [SerializeField] private string _sceneToLoad;

    public void Load()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
}
