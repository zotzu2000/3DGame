using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    [Header("怪物事件")]
    public GameObject NPC;
    [Header("產生怪物的方塊")]
    public GameObject CreatePos;
    [Header("Npc多久產出")]
    public float WaitTime;
    [Header("每個關卡Npc的最多數量")]
    public int MaxNum;
    // 目前在場景已經產出多少數量
    int Num;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CreateNpc", WaitTime, WaitTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateNpc()
    {
        if(Num < MaxNum)
        {
            // 抓取Collider三維座標最大值
            Vector3 MaxValue = CreatePos.GetComponent<Collider>().bounds.max;
            // 抓取Collider三維座標最小值
            Vector3 MinValue = CreatePos.GetComponent<Collider>().bounds.min;
            // 隨機抓取NPC要產生的位置點
            Vector3 RandomPox = new Vector3(Random.Range(MinValue.x, MaxValue.x), MinValue.y, MinValue.z);
            // 動態生成NPC
            Instantiate(NPC, RandomPox, CreatePos.transform.rotation);
            // 生成後數量+1
            Num++;
        }
    }
}
