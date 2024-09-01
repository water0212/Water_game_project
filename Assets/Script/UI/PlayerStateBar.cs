using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    public Image HealthImage;
    public Image HealthDelayImage;
    public Image RollingTimesImage;
    public Image TenacityImage;
    public Image ExperienceImage;
    public float HealthDelayRate;
    public float ExperienceDelayRate;
    private float ExperienceHide = 0;
    private void Update() {
        if(HealthDelayImage.fillAmount>HealthImage.fillAmount){
            HealthDelayImage.fillAmount-=Time.deltaTime*HealthDelayRate;
        }
        if(ExperienceImage.fillAmount<ExperienceHide){
            ExperienceImage.fillAmount += Time.deltaTime*ExperienceDelayRate;
        }
    }
    /// <summary>
    /// 調整血量接收的百分比
    /// </summary>
    /// <param name="persentage"></param>
    public void OnHealthChange(float persentage){
        HealthImage.fillAmount = persentage;
    }
    public void OnRollingTimesChange(float persentage){
        RollingTimesImage.fillAmount = persentage;
    }
    public void OnExperienceChange(float persentage){
        ExperienceHide += persentage;
    }
    public void OnTenacityChange(float persentage){
        TenacityImage.fillAmount = persentage;
    }
}
