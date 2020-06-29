using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float Speed, DeleteTime;

    private void Start()
    {
        Destroy(gameObject, DeleteTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider hit)
    {
        // 如果法術碰到NPC
        if (hit.GetComponent<Collider>().tag == "NPC" )
        {
            hit.GetComponent<NPC>().HitNpc(30f, 5f);
            Destroy(gameObject);
        }
        if (hit.GetComponent<Collider>().tag == "King")
        {
            hit.GetComponent<NPC>().HitNpc(20f, 2f);
            Destroy(gameObject);
        }
    }
}
