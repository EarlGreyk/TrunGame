using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceanLoaderManager : MonoBehaviour
{
    private static SceanLoaderManager _instance;

    public static SceanLoaderManager Instance
    {
        get
        {
            
            if (_instance == null)
            {
                var obj = FindObjectOfType<SceanLoaderManager>();
                if(obj != null)
                {
                    _instance = obj;
                }else
                {
                    _instance = Create();
                }
                

            }
            return _instance;


        }

        set
        {
            _instance = value;
        }
    }


    [SerializeField]
    private CanvasGroup SceneLoaderCanvasGroup;
    [SerializeField]
    private Image LoadingBar;


    private string loadSceneName;

    //생산자
    private static SceanLoaderManager Create()
    {
        var SceneLoaderPrefab = Resources.Load<SceanLoaderManager>("UI/Loading/SceanLoader");
        return Instantiate(SceneLoaderPrefab);
    
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

    }


    //로딩
    public void LoadScean(string SceanName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += LoadSceneEnd;
        loadSceneName = SceanName;
        StartCoroutine(Load(SceanName));

    }
    private IEnumerator Load(string SceneName)
    {
        LoadingBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneName);
        op.allowSceneActivation = false;

        float timer = 0f;

        while(!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                LoadingBar.fillAmount = Mathf.Lerp(LoadingBar.fillAmount, op.progress, timer);
            }
            else
            {
                timer += Time.deltaTime;
                LoadingBar.fillAmount = Mathf.Lerp(LoadingBar.fillAmount, 1f, timer);

                if (LoadingBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }

    }

    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == loadSceneName)
        {
            SceneManager.sceneLoaded -= LoadSceneEnd;
            if (scene.name == "MainScean")
            {
                GameManager.instance.GameStart();
                UImanager.Instance.UIStart();
            }
            StartCoroutine(Fade(false));
            
        }
       
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float time = 0f;
        if(isFadeIn == false)
        {
            while(GameManager.instance.Delay)
            {
                yield return null;
            }
        }
        while(time <=1f)
        {
            yield return null;
            time += Time.unscaledDeltaTime*2f;
            SceneLoaderCanvasGroup.alpha = Mathf.Lerp(isFadeIn ? 0 : 1, isFadeIn ? 1 : 0, time);
        }

        if(!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }


    
}
