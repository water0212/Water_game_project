using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : PlayerTrigger
{
    public BossWarriorEnemy Boss;
    public VoidEventSO BossStart;
    // Start is called before the first frame update
    protected override void IsFindPlayer(GameObject player){
        Boss.Player = player;
        BossStart.RaiseEvent();
        DestoryGB();
    }
}
