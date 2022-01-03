using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class AchievementManager : MonoBehaviour
{
    DataManager dm;

    public GameObject achieve;
    List<GameObject> achieveList = new List<GameObject>();

    GameObject getPop;
    //GameObject blur;

    string title;
    string content;
    // Start is called before the first frame update
    void Start()
    {
        dm = GameObject.Find("DataManager").GetComponent<DataManager>();
        getPop = GameObject.Find("GetPointPop");
        getPop.SetActive(false);
        //blur = GameObject.Find("Blur");
        //blur.SetActive(false);
        GetAchievements();
    }

    void Update()
    {
        // 뒤로가기 버튼을 두번 누를 시 MainScene으로 돌아간다.
        if (Input.GetKey(KeyCode.Escape))
        {
            LoadingSceneManager.Instance.LoadScene("Home");
        }
    }

    public void GetAchievements()
    {
        for (int i = 0; i < dm.data.acList.Count; i++)
        {
            achieve = Instantiate(Resources.Load("Prefab/Achievement"),
                GameObject.Find("AchiView/Viewport/Content").transform) as GameObject;
            int temp = i;

            achieve.transform.Find("ClearBtn").GetComponent<Button>().onClick.AddListener(() => RewardButtonClick(temp));

            achieve.transform.Find("ClearBtn").GetComponent<Button>().interactable = false;

            bool isCanGetReward = false;
            int require = 0;
            int ct = 0;
            string title = "[" + dm.data.acList[i].achievementTitle + "] ";
            string content1 = "";
            int content2 = 0;
            string content3 = "";
            Debug.Log(i+i + "번째 업적");
            switch (i)
            {
                case 0:
                    ct = 0;
                    for(int j=0;j<dm.data.ptList.Count;j++)
                    {
                        if(dm.data.ptList[j].contents == "출석 보상")
                        {
                            ct++;
                        }
                    }
                    require = ct;
                    content1 = "누적 접속 ";
                    content3 = "일";
                    break;
                case 1:
                    require = dm.data.pdList.Count;
                    content1 = "등록한 상품 ";
                    content3 = "개 이상";
                    break;
                case 2:
                    require = dm.data.safe;
                    content1 = "유통기한 준수 상품 ";
                    content3 = "개 이상";
                    break;
                case 3:
                    ct = 0;
                    for (int j = 0; j < 12; j++)
                    {
                        if (dm.data.have_deco_bg[j])
                        {
                            ct++;
                        }
                        if (dm.data.have_deco_col[j])
                        {
                            ct++;
                        }
                        if (dm.data.have_deco_cos[j])
                        {
                            ct++;
                        }
                        if (dm.data.have_deco_nbg[j])
                        {
                            ct++;
                        }
                        if (dm.data.have_deco_pet[j])
                        {
                            ct++;
                        }
                    }
                    require = ct-5;
                    content1 = "뽑기 ";
                    content3 = "번 하기";
                    break;
            }

            content2 = dm.data.acList[i].require_5;
            if (!dm.data.acList[i].isReward_5)
            {
                if (!dm.data.acList[i].isAchieve_5)
                {
                    if (dm.data.acList[i].require_5 <= require)
                    {
                        achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = 1;
                        dm.data.acList[i].isAchieve_5 = true;
                        isCanGetReward = true;
                    }
                    else
                    {
                        achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = (float)require / (float)dm.data.acList[i].require_5;
                    }
                }
                else
                {
                    achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = 1;
                    isCanGetReward = true;
                }
                achieve.transform.Find("Proceed/Text").GetComponent<Text>().text = require + "/" + dm.data.acList[i].require_5;
            }
            else
            {
                achieve.transform.Find("Achi5").GetComponent<Image>().sprite = Resources.Load("Image/Achievement/clear", typeof(Sprite)) as Sprite;
                achieve.transform.Find("Proceed/Text").GetComponent<Text>().text = "완료!";
                achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = 1;
            }

            if (!dm.data.acList[i].isReward_4)
            {
                if (!dm.data.acList[i].isAchieve_4)
                {
                    if (dm.data.acList[i].require_4 <= require)
                    {
                        achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = 1;
                        dm.data.acList[i].isAchieve_4 = true;
                        isCanGetReward = true;
                    }
                    else
                    {
                        achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = (float)require / (float)dm.data.acList[i].require_4;
                    }
                }
                else
                {
                    achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = 1;
                    isCanGetReward = true;
                }
                achieve.transform.Find("Proceed/Text").GetComponent<Text>().text = require + "/" + dm.data.acList[i].require_4;
                content2 = dm.data.acList[i].require_4;
            }
            else
            {
                achieve.transform.Find("Achi4").GetComponent<Image>().sprite = Resources.Load("Image/Achievement/clear", typeof(Sprite)) as Sprite;
            }

            if (!dm.data.acList[i].isReward_3)
            {
                if (!dm.data.acList[i].isAchieve_3)
                {
                    if (dm.data.acList[i].require_3 <= require)
                    {
                        achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = 1;
                        dm.data.acList[i].isAchieve_3 = true;
                        isCanGetReward = true;
                    }
                    else
                    {
                        achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = (float)require / (float)dm.data.acList[i].require_3;
                    }
                }
                else
                {
                    achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = 1;
                    isCanGetReward = true;
                }
                achieve.transform.Find("Proceed/Text").GetComponent<Text>().text = require + "/" + dm.data.acList[i].require_3;
                content2 = dm.data.acList[i].require_3;
            }
            else
            {
                achieve.transform.Find("Achi3").GetComponent<Image>().sprite = Resources.Load("Image/Achievement/clear", typeof(Sprite)) as Sprite;
            }

            if (!dm.data.acList[i].isReward_2)
            {

                if (!dm.data.acList[i].isAchieve_2)
                {
                    if (dm.data.acList[i].require_2 <= require)
                    {
   
                        achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = 1;
                        dm.data.acList[i].isAchieve_2 = true;
                        isCanGetReward = true;
                    }
                    else
                    {
                        achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = (float)require / (float)dm.data.acList[i].require_2;
                    }
                }
                else
                {
                    achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = 1;
                    isCanGetReward = true;
                }
                achieve.transform.Find("Proceed/Text").GetComponent<Text>().text = require + "/" + dm.data.acList[i].require_2;
                content2 = dm.data.acList[i].require_2;
            }
            else
            {
                achieve.transform.Find("Achi2").GetComponent<Image>().sprite = Resources.Load("Image/Achievement/clear", typeof(Sprite)) as Sprite;
            }

            if (!dm.data.acList[i].isReward_1)
            {
                if (!dm.data.acList[i].isAchieve_1)
                {
                    if (dm.data.acList[i].require_1 <= require)
                    {
                        achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = 1;
                        dm.data.acList[i].isAchieve_1 = true;
                        isCanGetReward = true;
                    }
                    else
                    {
                        achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = (float)require / (float)dm.data.acList[i].require_1;
                    }
                }
                else
                {
                    achieve.transform.Find("Proceed/Fill").GetComponent<Image>().fillAmount = 1;
                    isCanGetReward = true;
                }
                achieve.transform.Find("Proceed/Text").GetComponent<Text>().text = require + "/" + dm.data.acList[i].require_1;
                content2 = dm.data.acList[i].require_1;
            }
            else
            {
                achieve.transform.Find("Achi1").GetComponent<Image>().sprite = Resources.Load("Image/Achievement/clear", typeof(Sprite)) as Sprite;
            }
            achieve.transform.Find("AchiContent").GetComponent<Text>().text = title + content1 + content2 + content3;

            if (isCanGetReward)
            {
                achieve.transform.Find("ClearBtn").GetComponent<Button>().interactable = true;
            }

            achieveList.Add(achieve);
        }
    }

    public void RewardButtonClick(int no)
    {
        if(!dm.data.acList[no].isReward_1)
        {
            dm.data.point += 30;
            dm.data.acList[no].isReward_1 = true;
        }
        else if (!dm.data.acList[no].isReward_2)
        {
            dm.data.point += 30;
            dm.data.acList[no].isReward_2 = true;
        }
        else if (!dm.data.acList[no].isReward_3)
        {
            dm.data.point += 30;
            dm.data.acList[no].isReward_3 = true;
        }
        else if (!dm.data.acList[no].isReward_4)
        {
            dm.data.point += 30;
            dm.data.acList[no].isReward_4 = true;
        }
        else if (!dm.data.acList[no].isReward_5)
        {
            dm.data.point += 30;
            dm.data.acList[no].isReward_5 = true;
        }
        PointData np = new PointData(DateTime.Now, "업적 보상", 30);
        dm.data.ptList.Add(np);
        DataManager.SaveIngameData(dm.data);

        //blur.SetActive(true);
        getPop.SetActive(true);
    }

    public void PopupOkButtonClick()
    {
        getPop.SetActive(false);
        //blur.SetActive(false);
        for (int i = 0; i < achieveList.Count; i++)
        {
            Destroy(achieveList[i]);
        }
        GetAchievements();
    }
}
