using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    private void OnTriggerEnter(Collider hit)
    {
        // 碰撞怪物
        if (hit.GetComponent<Collider>().tag == "NPC")
        {
            //NPC扣血
            hit.GetComponent<NPC>().HitNpc(1000f, 10f);
        }
        if (hit.GetComponent<Collider>().tag == "King")
        {
            //King扣血
            hit.GetComponent<NPC>().HitNpc(1000f, 5f);
        }
        if (hit.GetComponent<Collider>().name == "floor")
        {
            //大法術物件消失
            Destroy(transform.parent.gameObject);
            //狀態復原
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CanCreateMagic = false;
            GameObject.Find("GM").GetComponent<GM>().MagicTimeScript = 0;
        }
    }
}
