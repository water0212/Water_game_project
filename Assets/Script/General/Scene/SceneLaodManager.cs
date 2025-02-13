using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SceneLaodManager : MonoBehaviour
{
    [Tooltip("是否要預先加載")]
    public bool FirstLoadingScene;
    public GameObject player;
    public SceneRefenceSO FirstLoadScene;
    public SceneRefenceSO testLoadScene;
    public static SceneLaodManager instance;
    [Header("廣播")]
    public VoidEventSO SceneLoading;
    public VoidEventSO SceneLoadFinish;
    [Header("目前狀態")]
    public AssetReference currentScene;
    public AsyncOperationHandle<SceneInstance> sceneInstance;
    public SceneInstance currentSceneInstance;
    private bool fadeComplete = false;
    [Header("test")]
    public float count;
    public float MaxCount;
    [Header("物件")]
    public Image blackBackGround;
    public Image LoadImage;
    private void Awake() {
        if(instance != null){
            Debug.LogWarning("有多個場景管理器");
            Destroy(this);
        }
        instance = this;
        if(FirstLoadingScene)sceneInstance = Addressables.LoadSceneAsync(FirstLoadScene.sceneRefence,LoadSceneMode.Additive);
        sceneInstance.Completed += OnSceneLoadComplete;
        currentScene = FirstLoadScene.sceneRefence;

    }
    private void OnEnable() {
        count = 0f;
    }
    private void Update() {
        //count+= Time.deltaTime;
        if(count > MaxCount){
            LoadScene(testLoadScene,new Vector2(-72.1999969f,-32.7000008f));
            count = -80f;
        }
    }
    public static void LoadScene(SceneRefenceSO scene, Vector2 position){
        instance.StartCoroutine(instance.blackFadeIn(2f,scene,position));
    }
    private IEnumerator LoadSceneasyns(SceneRefenceSO scene , Vector2 position){
        
        sceneInstance = Addressables.LoadSceneAsync(scene.sceneRefence, LoadSceneMode.Additive);
        SceneLoading.RaiseEvent();
        yield return sceneInstance ;
        player.transform.position = position;
        StartCoroutine(FadeOut(2f));
        currentScene = scene.sceneRefence;
        SceneLoadFinish.RaiseEvent();
        
    }

    public IEnumerator blackFadeIn(float duration,SceneRefenceSO scene, Vector2 position)
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            blackBackGround.color = new Color(0, 0, 0, t / duration);
            yield return null;
        }
            //StartCoroutine(FadeIn(LoadImage, 1f));
            LoadImage.color = new Color(LoadImage.color.r, LoadImage.color.g, LoadImage.color.b, 1);
            Addressables.UnloadSceneAsync(currentSceneInstance);
            StartCoroutine(LoadSceneasyns(scene, position));
    }
    public IEnumerator FadeIn(Image fadeImage,float duration)
    {
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, t / duration);
            yield return null;
        }
        fadeComplete = true;
    }
    public IEnumerator FadeOut(float duration)
    {
        for (float t = duration; t > 0; t -= Time.deltaTime)
        {
            blackBackGround.color = new Color(blackBackGround.color.r, blackBackGround.color.g, blackBackGround.color.b, t / duration);
            LoadImage.color = new Color(LoadImage.color.r, LoadImage.color.g, LoadImage.color.b, t / duration);
            yield return null;
        }
        blackBackGround.color = new Color(blackBackGround.color.r, blackBackGround.color.g, blackBackGround.color.b, 0);
        LoadImage.color = new Color(LoadImage.color.r, LoadImage.color.g, LoadImage.color.b, 0);
    }
    private void OnSceneLoadComplete(AsyncOperationHandle<SceneInstance> temp)
{
    currentSceneInstance = temp.Result;
}
}
