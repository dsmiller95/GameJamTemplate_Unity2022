using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// TODO: document
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public bool randomScene;
    public bool reloadThisScene;
    public int sceneNumber = -1;
    public string sceneName;
    public bool loadOnAwake = false;
    public MinMaxFloat loadAfterTime;

    [SerializeField] private bool loadNextScene;
    [SerializeField] private bool loadPrevScene;

    private float loadTime = 0;

    private float birthTime;

    private void Awake()
    {
        birthTime = Time.fixedTime;
        if(loadOnAwake)
        {
            LoadScene();
        }

        loadTime = Random.Range(loadAfterTime.min, loadAfterTime.max);

        if(loadTime > 0)
        {
            LoadAfterTime(loadTime).Forget();
        }
    }

    private async UniTask LoadAfterTime(float waitTime)
    {
        await UniTask.WaitForSeconds(waitTime);
        LoadScene();
    }

    public void LoadScene()
    {
        if(loadNextScene)
        {
            int curIndex = SceneManager.GetActiveScene().buildIndex;
            if(curIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(curIndex + 1);
            }
        }
        else if (loadPrevScene)
        {
            int curIndex = SceneManager.GetActiveScene().buildIndex;
            if (curIndex == 0)
            {
                SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
            }
            else
            {
                SceneManager.LoadScene(curIndex - 1);
            }
        }
        else if(sceneNumber != -1)
        {
            SceneManager.LoadScene(sceneNumber);
        }
        else if(sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }
        else if(randomScene)
        {
            SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCount));
        }
        else if(reloadThisScene)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void LoadScene(int value)
    {
        SceneManager.LoadScene(value);
    }

    public void LoadScene(string value)
    {
        SceneManager.LoadScene(value);
    }
}
