using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHealthUIManager : MonoBehaviour
{
    public VoidEventSO BossStart;
    public FloatFloatEventSO BossHealthChangeEvent;
    public FloatFloatEventSO BossTenacityChangeEvent;
    public VoidEventSO BossDeadEvent;
    //public VoidEventSO BossEnd;
    public CanvasGroup BossHealthBar;
    public Image BossHealthImage;
    public Image BossHealthReduceImage;
    public Image BossTenacityImage;
    public float fadeDuration = 1.0f;
    private bool ReduceHealth; 
    public float ReduceHealthSpeed;
    public void OnEnable(){
        BossStart.OnEventRaised += BossShowHealthUI;
        BossHealthChangeEvent.OnEventRaised += BossHealthChangeUI;
        BossTenacityChangeEvent.OnEventRaised += BossTenacityChangeUI;
        BossDeadEvent.OnEventRaised += BossDead;
      //  BossEnd.OnEventRaised += BossHideHealthUI;
        BossHideHealthUI();
    } 
    public void OnDisable(){
        BossStart.OnEventRaised -= BossShowHealthUI;
        BossHealthChangeEvent.OnEventRaised -= BossHealthChangeUI;
        BossTenacityChangeEvent.OnEventRaised -= BossTenacityChangeUI;
        BossDeadEvent.OnEventRaised -= BossDead;
        //BossEnd.OnEventRaised -= BossHideHealthUI;
    }
    public void Update(){
        if(ReduceHealth)
        HealthReduce();
    }
     public void BossShowHealthUI()
    {
        StartCoroutine(FadeInBossHealthBar());
        BossHealthChangeUI(1,1);
    }
    private IEnumerator FadeInBossHealthBar()
    {
        BossHealthBar.gameObject.SetActive(true); // 確保 UI 顯示
        BossHealthBar.alpha = 0; // 初始化 alpha
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            BossHealthBar.alpha = Mathf.Clamp01(elapsedTime / fadeDuration); // 計算當前 alpha
            yield return null; // 等待下一幀
        }

        BossHealthBar.alpha = 1; // 確保最終 alpha 為 1
    }
    private IEnumerator FadeOutBossHealthBar()
    {
        BossHealthBar.gameObject.SetActive(true); // 確保 UI 顯示
        BossHealthBar.alpha = 1; // 初始化 alpha
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            BossHealthBar.alpha = Mathf.Clamp01((fadeDuration-elapsedTime) / fadeDuration); // 計算當前 alpha
            yield return null; // 等待下一幀
        }

        BossHealthBar.alpha = 0; // 確保最終 alpha 為 0
    }
    public void BossHideHealthUI(){
        BossHealthBar.gameObject.SetActive(false);
    }
    public void BossHealthChangeUI(float MaxHealth,float Health){
        BossHealthImage.fillAmount = Health/MaxHealth;
        ReduceHealth = true;
    }

    public void BossTenacityChangeUI(float maxTenacity,float tenacityPoint){
        BossTenacityImage.fillAmount = tenacityPoint/maxTenacity;
    }
    public void BossDead(){
        StartCoroutine(FadeOutBossHealthBar());
    }
    public void HealthReduce(){
        if(BossHealthImage.fillAmount < BossHealthReduceImage.fillAmount){
            BossHealthReduceImage.fillAmount -= (Time.deltaTime*ReduceHealthSpeed)/60;
        }else ReduceHealth = false;
    }
}
