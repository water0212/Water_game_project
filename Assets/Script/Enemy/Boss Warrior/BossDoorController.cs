using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorController : MonoBehaviour
{
    // Start is called before the first frame update
    public VoidEventSO BossStart;
    private BoxCollider2D coll;
    public GameObject[] DoorImage;
    private void Awake() {
        coll = GetComponent<BoxCollider2D>();
    }
    private void Start() {
        coll.enabled = false;
        foreach (GameObject obj in DoorImage)
        {
            if (obj.TryGetComponent(out SpriteRenderer sr))
            {
                Color color = sr.color;
                color.a = 0; // 設置 alpha 為 0
                sr.color = color;
            }
        }
    }
    private void OnEnable() {
        BossStart.OnEventRaised += bossDoorEnable;
    }
    private void OnDisable() {
        BossStart.OnEventRaised -= bossDoorEnable;
    }

    private void bossDoorEnable()
    {
        if (coll != null) {
            coll.enabled = true;
        }
         foreach (GameObject obj in DoorImage)
        {
            if (obj.TryGetComponent(out SpriteRenderer sr))
            {
                StartCoroutine(FadeIn(sr));
            }
        }
    }
    private IEnumerator FadeIn(SpriteRenderer spriteRenderer)
    {
        float duration = 1.0f; // 漸變持續時間
        float elapsedTime = 0f;

        Color color = spriteRenderer.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / duration); // 計算 alpha 值
            spriteRenderer.color = color;
            yield return null; // 等待下一幀
        }

        // 確保 alpha 值最終為 1
        color.a = 1;
        spriteRenderer.color = color;
    }
}
