using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanLoad : MonoBehaviour
{
    private static SceanLoad instance;

    public static SceanLoad Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    public void NewStartScean()
    {
        Scene scene = SceneManager.GetActiveScene();

        if(scene.name == "StartScean")
        {

            GameManager.instance.Delay = true;
            SceanLoaderManager.Instance.LoadScean("MainScean");
            return;
        }

        if(scene.name == "MainScean")
        {
            SceanLoaderManager.Instance.LoadScean("StartScean");
            return;
        }
    }
    public void LoadStartSCean()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "StartScean")
        {
            SaveLoadManager.instance.IsLoad = true;
            SaveLoadManager.instance.GameLoadSetting();
            GameManager.instance.Delay = true;
            SceanLoaderManager.Instance.LoadScean("MainScean");
            return;
        }

        if (scene.name == "MainScean")
        {
            SceanLoaderManager.Instance.LoadScean("StartScean");
            return;
        }
    }
    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }



}
