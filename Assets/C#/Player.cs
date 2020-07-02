using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 控制人物轉向
    // 將RaycastAll的射線打到的所有模型存放在此陣列中
    RaycastHit[] hits;
    // 從陣列中找尋要的物件
    RaycastHit hit;
    // 滑鼠點擊的座標點
    Vector3 targetPos;
    // 玩家要看的方向位置
    Vector3 LookPos;
    #endregion

    #region 產生小法術物件
    [Header("小法術物件")]
    public GameObject Arrow;
    [Header("小法術物件要生成的點")]
    public GameObject ArrowPos;
    #endregion

    #region 玩家血量
    [Header("設定玩家總血量")]
    public float TotalHP;
    // 程式中計算玩家血量
    float ScriptHP;
    // 玩家的血條
    public Image HPBar;
    #endregion
    #region 玩家大絕招
    [Header("大絕招的Bar條")]
    public Image MagicBar;
    // 判斷是否已經點擊大絕招按鈕
    public bool CanCreateMagic;
    [Header("大絕招的物件")]
    public GameObject MargicObject;
    // 儲存動態生成出來大絕招物件
    GameObject MagicObjectPrefab;
    #endregion

    void Start()
    {
        // 程式中的血量 = 屬性面板中設定的血量數值
        ScriptHP = TotalHP;
    }

    private void Update()
    {
        // 持續按下滑鼠左鍵，玩家會持續面向滑鼠點擊的位置
        if (Input.GetMouseButton(0)){
            // 滑鼠點擊的地方轉換成遊戲內的三維座標點後，此座標點與Main Camera兩點連為一線，成為射線
            // 遊戲內一定要有主攝影Main Camera機否則程式會出現錯誤
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // 射線從Main Camera發射，長度為100單位，將射線打到的所有3D物件都存放在hits陣列中
            hits = Physics.RaycastAll(Camera.main.transform.position, ray.direction, 100);
            // 用for迴圈檢測打到的物件是否有地板物件
            for(int i = 0; i < hits.Length; i++)
            {
                hit = hits[i];
                // 遊戲內的地板名稱要與程式內相同
                if (hit.collider.name == "floor")
                {
                    // 將射線打到的位置點帶入targetPos，只要記錄xz平面的座標值
                    targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    // 如果還沒按下大絕招的按鈕
                    if (!CanCreateMagic)
                    {
                        // 使用數學內插法，讓玩家從A點慢慢轉向至B點，如果沒有使用內插法，玩家會馬上從A點跳到B點，不會慢慢轉向
                        LookPos = Vector3.Lerp(LookPos, targetPos, Time.deltaTime * 10);
                        // 讓玩家注視內插法的座標點
                        transform.LookAt(LookPos);
                        // 玩家進行攻擊
                        GetComponent<Animator>().SetBool("Att", true);
                    }
                    // 如果按下大絕招的按鈕
                    else
                    {
                        // 目前場景上沒有任何大絕招的物件
                        if (MagicObjectPrefab == null)
                            // 動態生成大絕招的物件
                            MagicObjectPrefab = Instantiate(MargicObject) as GameObject;
                        // 如果Dragon的重力沒有被開啟 才可以移動Dragon
                        if(!MagicObjectPrefab.GetComponentInChildren<Rigidbody>().useGravity)
                            // 大絕招的物件位置=滑鼠位置
                            MagicObjectPrefab.transform.position = targetPos;
                    }
                }
            }
        }

        else
        {
            // 如果沒有按下滑鼠左鍵，玩家無法進行攻擊動作
            GetComponent<Animator>().SetBool("Att", false);
        }
        //
        if(Input.GetMouseButtonUp(0) && CanCreateMagic)
        {
            MagicObjectPrefab.GetComponentInChildren<Rigidbody>().useGravity = true;
        }
    }

    // 產生法術物件
    public void CreateArrow()
    {
        Instantiate(Arrow, ArrowPos.transform.position, ArrowPos.transform.rotation);
    }
    // 玩家被怪物攻擊扣血
    public void HurtPlayerHP(float Hurt)
    {
        ScriptHP -= Hurt;
        HPBar.fillAmount = ScriptHP / TotalHP;
    }
    // 滑鼠或是手指點下大絕招的按鈕位置
    public void PointerDownMagic()
    {
        if (MagicBar.fillAmount == 1)
        {
            CanCreateMagic = true;
        }
    }
}
