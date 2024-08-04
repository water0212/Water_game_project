using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkillUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas Skill_Canvas;
    public Canvas EnterSkillUI_icon;
    public void OpenSkillChoose()
    {
        Skill_Canvas.gameObject.SetActive(true);
        EnterSkillUI_icon.gameObject.SetActive(false);
        Debug.Log ("www");
    }
    public void CloseSkillChoose(){
        Skill_Canvas.gameObject.SetActive(false);
        EnterSkillUI_icon.gameObject.SetActive(true);
    }

    
}
