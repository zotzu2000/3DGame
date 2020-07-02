using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // 移動速度
    public float Speed;
    float ScriptSpeed;
    // NPC總血量
    public float TotalHP;
    float ScriptHP;

    // Start is called before the first frame update
    void Start()
    {
        ScriptSpeed = Speed;
        ScriptHP = TotalHP;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * ScriptSpeed);
    }

    // 碰到藍色圍牆開始攻擊
    private void OnTriggerEnter(Collider hit)
    {
        if(hit.GetComponent<Collider>().name == "mazu_wall")
        {
            ScriptSpeed = 0;
            GetComponent<Animator>().SetBool("Att", true);
        }
    }

    // 離開藍色圍牆回到Walk
    private void OnTriggerExit(Collider hit)
    {
        if(hit.GetComponent<Collider>().name == "mazu_wall")
        {
            ScriptSpeed = Speed;
            GetComponent<Animator>().SetBool("Att", false);
        }
    }

    // 法術物件打到怪物扣血與死亡
    public void HitNpc(float Hurt,float dis)
    {
        ScriptHP -= Hurt;
        ScriptHP = Mathf.Clamp(ScriptHP, 0, TotalHP);
        transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z + dis);
        if (ScriptHP <= 0)
        {
            GetComponent<Animator>().SetTrigger("Dead");
            ScriptSpeed = 0;
            GetComponent<Collider>().enabled = false;
            GameObject.Find("GM").GetComponent<GM>().DeadCount();
        }
    }

    public void AttackPlayer()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().HurtPlayerHP(10f);
    }
}
