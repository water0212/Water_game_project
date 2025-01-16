using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AttackNumber : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public float floatSpeed = 2f;
    public float fadeDuration = 1f;
    private float timer = 0f;
    private int nowDamageNumber;
    private Vector2 originPosition;
    private void OnEnable() {
        originPosition = transform.position;
        damageText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;


        timer += Time.deltaTime;
        if (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            Color color = damageText.color;
            color.a = alpha;
            damageText.color = color;
        }
        else
        {
            Destroy(gameObject); // 刪除數值物件
        }
    }

    public void SetDamage(int damageAmount)
    {
        nowDamageNumber = damageAmount;
        damageText.text = nowDamageNumber.ToString();
    }
    public void AddDamage(int damageAmount){
        nowDamageNumber+=damageAmount;
        damageText.text = nowDamageNumber.ToString();
        timer = 0;
        transform.position = originPosition;
    }
    public void ResetPosition(Vector2 position){
        originPosition = position;
    }
}
