using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToNextSceneDialog_NPC : Dialog_Trigger
{
    [SerializeField]private Vector2 _TeleportPosition;
    [SerializeField]private SceneRefenceSO Scene;
    public void ToNextScene()
    {
        SceneLaodManager.LoadScene(Scene, _TeleportPosition);
    }

    protected override void PlayDialog()
    {
        DialogManager.GetInstance().BindExternalFunction("ToNextScene",  ToNextScene);
        base.PlayDialog();
    }
}
