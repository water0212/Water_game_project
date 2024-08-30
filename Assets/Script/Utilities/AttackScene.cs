using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScene : MonoBehaviour
{
    private static AttackScene instance;
    [Header("受傷遊戲暫停比率")]
    public int TimeAttackStopRate;
    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("Found more than one AttackScene in the Scene");
        }
        instance = this;
    }
    public static AttackScene GetInstance() {
        return instance;
    }
    public void HitPause(float duration){
        StartCoroutine(Pause(duration));
    }
    public IEnumerator Pause(float duration){
        float PauseTime = duration/TimeAttackStopRate;
        Debug.Log("暫停" + PauseTime);
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(PauseTime);
        Time.timeScale = 1;
    }
}
