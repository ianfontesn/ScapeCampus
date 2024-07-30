using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BasePuzzle : MonoBehaviour
{

    protected void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    protected virtual void SetPlayerPrefValue(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    protected virtual int GetPlayerPrefValue(string key)
    {

        return PlayerPrefs.GetInt(key);
    }

    protected virtual bool LoadSceneAsync(int buildIndex)
    {
        bool loaded = false;

        switch (buildIndex)
        {
            case 0:
                SceneManager.LoadSceneAsync(buildIndex);
                loaded = true;
                break;

            case 1:
                if (GetPlayerPrefValue("ProgrammingLogicPuzzle") == 1)
                {
                    SceneManager.LoadSceneAsync(buildIndex);
                    loaded = true;
                }
                break;

            case 2:
                if (GetPlayerPrefValue("PokemonQuizPuzzle") == 1)
                {
                    SceneManager.LoadSceneAsync(buildIndex);
                    loaded = true;
                }
                break;
        }

        return loaded;
    }


    
}