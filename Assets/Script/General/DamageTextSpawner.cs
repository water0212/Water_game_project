using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject damageTextPrefab;
    private GameObject damageTextInstance;
    private AttackNumber damageText;
    public Vector2 TextOffset;

    public void SpawnDamageText(int damageAmount, Vector2 worldPosition)
    {
        Vector2 generatePos = worldPosition+TextOffset; 
        // 生成傷害數值物件
        if(damageTextInstance == null){
            damageTextInstance = Instantiate(damageTextPrefab, generatePos, Quaternion.identity,this.transform);
            damageText = damageTextInstance.GetComponent<AttackNumber>();
            damageText.SetDamage(damageAmount);
        }else{
            //重設傷害數值
            Debug.Log("test");
            damageText.AddDamage(damageAmount);
            damageText.ResetPosition(generatePos);
        }
    }
}
