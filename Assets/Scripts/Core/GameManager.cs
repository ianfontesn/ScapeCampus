using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static IEnumerator LoadSceneAsync(int sceneBuildIndex)
    {
        var task = SceneManager.LoadSceneAsync(sceneBuildIndex);

        while(!task.isDone)
        {
            yield return null;
        }
    }

}
