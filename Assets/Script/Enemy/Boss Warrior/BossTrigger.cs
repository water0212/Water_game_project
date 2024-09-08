using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : PlayerTrigger
{
    public BossWarriorEnemy Boss;
    // Start is called before the first frame update
    protected override void IsFindPlayer(GameObject player){
        Boss.Player = player;
        Boss.BossStart();
        DestoryGB();
    }
}
