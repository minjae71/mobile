using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class HomeManager : MonoBehaviour
{
    DataManager dm;

    Animator charAnim;
    Animator cosAnim;
    Animator pet_motionAnim;
    Animator pet_stateAnim;

    Image background;
    Image color;
    Image costume;
    Image pet_motion;
    Image pet_state;
    Image nameBackground;

    GameObject attendancePop;
    GameObject changeNamePop;
    GameObject pointHistory;
    GameObject imminentPop;
    GameObject deadlineMsg;
    GameObject stockMsg;
    GameObject resultPop;
    GameObject quitPop;

    DateTime now;

    int todayDeadline = 0;
    int todayStock = 0;
    int finish = 0;
    int over = 0;

    int today = 0;

    string checkVersion;

    string checkAttendance = "";

    List<ProductData> sortDeadlineList = new List<ProductData>();
    List<ProductData> sortStockList = new List<ProductData>();

    // Start is called before the first frame update
    void Start()
    {
        dm = GameObject.Find("DataManager").GetComponent<DataManager>();
        if (dm.data == null) // 앱이 처음 실행된 경우
        {
            dm.data = new SaveData();

            // 디폴트 구역으로 냉장고, 냉동고 생성
            dm.data.adList.Add(new AreaData(0, "냉장고", false));
            dm.data.adList.Add(new AreaData(1, "냉동고", false));
        }

        // 데이터에 추가할 항목이 생기면 이 변수 수정
        checkVersion = "v.20191031_2";
        if (dm.data.version != checkVersion)
        {
            UpdateVersion();
        }

        int total = 0;
        int safeCount = 0;
        for(int i=0;i<dm.data.pdList.Count;i++)
        {
            if(dm.data.pdList[i].stock == 0)
            {
                dm.data.pdList[i].isSpend = true;
                if(dm.data.isCompleteProductDelete)
                {
                    dm.data.pdList[i].isDeleted = true;
                }
            }
            if(dm.data.pdList[i].expDate < DateTime.Now)
            {
                total++;
            }
            if(dm.data.pdList[i].isSpend)
            {
                safeCount++;
            }
        }

        dm.data.total = total;
        dm.data.safe = safeCount;
        DataManager.SaveIngameData(dm.data);

        GameObject.Find("MainTxt").GetComponent<Text>().text = "'" + dm.data.cname + "'님의 유통기한 준수율";
        GameObject.Find("Rate").GetComponent<Text>().text = dm.data.CalRate() + "%";

        sortDeadlineList = dm.data.pdList;
        sortStockList = dm.data.pdList;

        int stateTemp = dm.data.CharState();

        background = GameObject.Find("Background/UserBackground").GetComponent<Image>();
        background.sprite = Resources.Load("Image/Deco/Background/bg_" + dm.data.deco_bg, typeof(Sprite)) as Sprite;

        color = GameObject.Find("Character/Char").GetComponent<Image>();
        //color.sprite = Resources.Load("Image/Deco/Character/Color/" + dm.data.CharState() + "/col_" + dm.data.deco_col, typeof(Sprite)) as Sprite;
        color.sprite = Resources.Load("Animation/" + stateTemp + "/Character/" + dm.data.deco_col + "/0", typeof(Sprite)) as Sprite;
        charAnim = GameObject.Find("Character/Char").GetComponent<Animator>();
        charAnim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animation/" + stateTemp + "/Character/" + dm.data.deco_col + "/Character_" + stateTemp + "_" + dm.data.deco_col, typeof(RuntimeAnimatorController));

        costume = GameObject.Find("Character/Costume").GetComponent<Image>();
        costume.sprite = Resources.Load("Animation/" + stateTemp + "/Costume/" + dm.data.deco_cos + "/0", typeof(Sprite)) as Sprite;
        cosAnim = GameObject.Find("Character/Costume").GetComponent<Animator>();
        if(dm.data.deco_cos != 0 && dm.data.deco_cos != 2 && dm.data.deco_cos != 5)
        {
            cosAnim = GameObject.Find("Character/Costume").GetComponent<Animator>();
            cosAnim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animation/" + stateTemp + "/Costume/" + dm.data.deco_cos + "/Costume_" + stateTemp + "_" + dm.data.deco_cos, typeof(RuntimeAnimatorController));
        }

        pet_motion = GameObject.Find("Character/Pet_Motion").GetComponent<Image>();
        pet_motion.sprite = Resources.Load("Animation/Pet/" + dm.data.deco_pet + "/0", typeof(Sprite)) as Sprite;
        pet_motionAnim = GameObject.Find("Character/Pet_Motion").GetComponent<Animator>();
        if (dm.data.deco_pet != 0)
        {
            pet_motionAnim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animation/Pet/" + dm.data.deco_pet + "/Pet_Motion_" + dm.data.deco_pet, typeof(RuntimeAnimatorController));
        }
        /*pet = GameObject.Find("Character/Pet").GetComponent<Image>();
        pet.sprite = Resources.Load("Image/Deco/Pet/" + dm.data.CharState() + "/pet_" + dm.data.deco_pet, typeof(Sprite)) as Sprite;*/
        
        if (dm.data.deco_pet != 0)
        {
            pet_state = GameObject.Find("Character/Pet_State").GetComponent<Image>();
            pet_state.sprite = Resources.Load("Animation/" + dm.data.CharState() + "/Pet/0", typeof(Sprite)) as Sprite;
            pet_stateAnim = GameObject.Find("Character/Pet_State").GetComponent<Animator>();
            pet_stateAnim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animation/" + stateTemp + "/Pet/Pet_State_" + stateTemp, typeof(RuntimeAnimatorController));
        }

        nameBackground = GameObject.Find("Character/Name").GetComponent<Image>();
        nameBackground.sprite = Resources.Load("Image/Deco/NameBackground/nbg_" + dm.data.deco_nbg, typeof(Sprite)) as Sprite;

        deadlineMsg = GameObject.Find("TodayImminent/TodayBackground/NoDeadlineProduct");
        stockMsg = GameObject.Find("TodayImminent/TodayBackground/NoStockProduct");


        now = DateTime.Now;
        switch (stateTemp)
        {
            case 1:
                GameObject.Find("Character/State/Text").GetComponent<Text>().text = "기분이 좋아요!";
                break;
            case 2:
                GameObject.Find("Character/State/Text").GetComponent<Text>().text = "조금 더 힘내요!";
                break;
            case 3:
                GameObject.Find("Character/State/Text").GetComponent<Text>().text = "조금 힘들어요..";
                break;
            case 4:
                GameObject.Find("Character/State/Text").GetComponent<Text>().text = "아야 하는 중..";
                break;
        }

        //당일에 처음 접속했으면 출석 보상 10p 지급
        attendancePop = GameObject.Find("AttendancePopup");
        checkAttendance = now.Year.ToString() + now.Month.ToString() + now.Day.ToString();
        if(checkAttendance != dm.data.attendance)
        {
            dm.data.point += 10;
            dm.data.attendance = checkAttendance;
            dm.data.countAttendance++;

            PointData np = new PointData(now, "출석 보상", 10);
            dm.data.ptList.Add(np);
        }
        else
        {
            attendancePop.SetActive(false);
        }

        //팝업 오늘 하루 열지 않기에서 오늘이 지났는지
        if(dm.data.isNoPopup && checkAttendance!=dm.data.setPopupDate)
        {
            dm.data.isNoPopup = false;
        }

        GameObject.Find("PointTxt").GetComponent<Text>().text = "" + dm.data.point;

        changeNamePop = GameObject.Find("ChangeNamePopup");
        GameObject.Find("Character/Name/NameBtn/NameTxt").GetComponent<Text>().text = dm.data.cname;
        changeNamePop.SetActive(false);

        imminentPop = GameObject.Find("ImminentPopup");
        resultPop = GameObject.Find("ResultPopup");
        quitPop = GameObject.Find("QuitPopup");

        GetTodayProduct();

       

        pointHistory = GameObject.Find("PointHistory");
        int start = dm.data.ptList.Count - 1;
        int last = 0;
        if (start > 30)
            last = start - 30;
        for(int i = start;i >= last;i--)
        {
            GameObject getPt = Instantiate(Resources.Load("Prefab/PointHistory"), GameObject.Find("PointHistory/HistoryBox/Viewport/Content").transform) as GameObject;
            getPt.name = "getPt" + i;
            if (dm.data.ptList[i].getPoint<0)
            {
                getPt.transform.Find("Date").GetComponent<Text>().text = "<color=#C8613C>" + dm.data.ptList[i].getDate.ToString("yyyy/MM/dd  HH:mm") + "</color>";
                getPt.transform.Find("Contents").GetComponent<Text>().text = "<color=#C8613C>" + dm.data.ptList[i].contents + "</color>";
                getPt.transform.Find("GetPoint").GetComponent<Text>().text = "<color=#C8613C>" + dm.data.ptList[i].getPoint + "P</color>";
            } else
            {
                getPt.transform.Find("Date").GetComponent<Text>().text = dm.data.ptList[i].getDate.ToString("yyyy/MM/dd  HH:mm");
                getPt.transform.Find("Contents").GetComponent<Text>().text = dm.data.ptList[i].contents;
                getPt.transform.Find("GetPoint").GetComponent<Text>().text = dm.data.ptList[i].getPoint + "P";
            }
            
            
        }
        pointHistory.SetActive(false);
        quitPop.SetActive(false);

        DataManager.SaveIngameData(dm.data);
    }

    public void UpdateVersion()
    {
        dm.data.acList.RemoveRange(0, dm.data.acList.Count);
        AchievementData acd1 = new AchievementData("", "", 1, 2, 3, 4, 5, false, false, false, false, false, false, false, false, false, false);
        acd1.achievementTitle = "접속";
        acd1.achievementContent = "누적 접속 1일";
        dm.data.acList.Add(acd1);

        AchievementData acd2 = new AchievementData("", "", 2, 4, 6, 8, 10, false, false, false, false, false, false, false, false, false, false);
        acd2.achievementTitle = "상품";
        acd2.achievementContent = "등록한 상품 2개 이상";
        dm.data.acList.Add(acd2);

        AchievementData acd3 = new AchievementData("", "", 2, 4, 6, 8, 10, false, false, false, false, false, false, false, false, false, false);
        acd3.achievementTitle = "상품";
        acd3.achievementContent = "유통기한 준수 상품 2개 이상";
        dm.data.acList.Add(acd3);

        AchievementData acd4 = new AchievementData("", "", 2, 4, 6, 8, 10, false, false, false, false, false, false, false, false, false, false);
        acd4.achievementTitle = "꾸미기";
        acd4.achievementContent = "뽑기 2번 하기";
        dm.data.acList.Add(acd4);

        dm.data.version = checkVersion;
        DataManager.SaveIngameData(dm.data);
    }

    void Update()
    {
        // 뒤로가기 버튼을 두번 누를 시 MainScene으로 돌아간다.
        if (Input.GetKey(KeyCode.Escape))
        {
            quitPop.SetActive(true);
        }
    }

    public void GetTodayProduct()
    {
        int pdSize = sortDeadlineList.Count;

        ProductSort();
        if (pdSize != 0)
        {
            for (int i = 0; i < pdSize; i++)
            {
                if (!sortDeadlineList[i].isDeleted)
                {
                    if ((sortDeadlineList[i].expDate - now).Days < 1)
                    {
                        today++;
                        todayDeadline++;
                        GameObject todayDatePd = Instantiate(Resources.Load("Prefab/TodayDeadlineColumn"),
                            GameObject.Find("TodayImminent/TodayBackground/TodayDeadline/DeadlineBox/Viewport/Content").transform) as GameObject;

                        todayDatePd.name = "D" + i;

                        string temp = todayDatePd.name;
                        todayDatePd.transform.Find("IsSpend").GetComponent<Button>().onClick.AddListener(() => CheckProuct(temp));
                        if (sortDeadlineList[i].isSpend)
                        {
                            todayDatePd.transform.Find("IsSpend/Check").GetComponent<Image>().sprite = Resources.Load("Image/Home/checked", typeof(Sprite)) as Sprite;
                        }
                        else
                        {
                            todayDatePd.transform.Find("IsSpend/Check").GetComponent<Image>().sprite = Resources.Load("Image/Home/check_none", typeof(Sprite)) as Sprite;
                        }
                        
                        String dday = "D0";
                        if ((sortDeadlineList[i].expDate - now).Days == 0)
                        {
                            finish++;
                        }
                        else if ((sortDeadlineList[i].expDate - now).Days < 0)
                        {
                            over++;
                            dday = "D+";
                        }
                        todayDatePd.transform.Find("DDay").GetComponent<Text>().text = dday;
                        todayDatePd.transform.Find("ProductName").GetComponent<Text>().text = sortStockList[i].productName;
                    }
                }
            }
            for (int i = 0; i < pdSize; i++)
            {
                if (!sortStockList[i].isDeleted)
                {
                    if (sortStockList[i].stock < 5)
                    {
                        todayStock++;
                        GameObject todayStockPd = Instantiate(Resources.Load("Prefab/TodayStockColumn"),
                            GameObject.Find("TodayImminent/TodayBackground/TodayStock/StockBox/Viewport/Content").transform) as GameObject;


                        todayStockPd.name = "S" + i;

                        string temp = todayStockPd.name;
                        todayStockPd.transform.Find("IsSpend").GetComponent<Button>().onClick.AddListener(() => CheckProuct(temp));
                        if (sortStockList[i].isSpend)
                        {
                            todayStockPd.transform.Find("IsSpend/Check").GetComponent<Image>().sprite = Resources.Load("Image/Home/checked", typeof(Sprite)) as Sprite;
                            if(sortStockList[i].stock == 0)
                            {
                                todayStockPd.transform.Find("IsSpend").GetComponent<Button>().interactable = false;
                            }
                        }
                        else
                        {
                            todayStockPd.transform.Find("IsSpend/Check").GetComponent<Image>().sprite = Resources.Load("Image/Home/check_none", typeof(Sprite)) as Sprite;
                        }
                        todayStockPd.transform.Find("Stock").GetComponent<Text>().text = sortStockList[i].stock + "";
                        todayStockPd.transform.Find("ProductName").GetComponent<Text>().text = sortStockList[i].productName;
                    }
                }
            }
            if (todayDeadline != 0)
            {
                deadlineMsg.SetActive(false);
            }
            if (todayStock != 0)
            {
                stockMsg.SetActive(false);
            }
        }

        if(dm.data.isNoPopup)
        {
            resultPop.SetActive(false);
        }
        else if (finish == 0 && over == 0)
        {
            resultPop.SetActive(false);
        }
        else
        {
            if (finish != 0 && over == 0)
            {
                GameObject.Find("ResultPopup/ResultTxt").GetComponent<Text>().text = "유통기한이 24시간 이내인 상품이 " + finish + "개 있습니다.\n보관함을 확인해 주세요!";
            }
            else if (finish == 0 && over != 0)
            {
                GameObject.Find("ResultPopup/ResultTxt").GetComponent<Text>().text = "유통기한이 지난 상품이 " + over + "개 있습니다.\n보관함을 확인해 주세요!";
            }
            else
            {
                GameObject.Find("ResultPopup/ResultTxt").GetComponent<Text>().text = "오늘의 마감 상품이 " + finish + "개,\n유통기한이 지난 상품이 " + over + "개 있습니다.\n보관함을 확인해 주세요!";
            }
        }
        imminentPop.SetActive(false);
        GameObject.Find("Imminent/ImminentBtn/DeadLineTxt").GetComponent<Text>().text = todayDeadline + "";
        GameObject.Find("Imminent/ImminentBtn/StockTxt").GetComponent<Text>().text = todayStock + "";
    }

    public void CheckNoPopup()
    {
        if(!dm.data.isNoPopup)
        {
            dm.data.setPopupDate = checkAttendance;
            dm.data.isNoPopup = true;
            GameObject.Find("ResultPopup/Today").GetComponent<Image>().sprite = Resources.Load("Image/Home/checked", typeof(Sprite)) as Sprite;
        }
        else
        {
            dm.data.setPopupDate = "";
            dm.data.isNoPopup = false;
            GameObject.Find("ResultPopup/Today").GetComponent<Image>().sprite = Resources.Load("Image/Home/check_b", typeof(Sprite)) as Sprite;
        }
        DataManager.SaveIngameData(dm.data);
    }

    public void IsQuit(bool isQuit)
    {
        if(isQuit)
        {
            DataManager.SaveIngameData(dm.data);
            Application.Quit();
        }
        else
        {
            quitPop.SetActive(false);
        }
    }

    public void ProductSort()
    {
        sortDeadlineList.Sort(delegate (ProductData A, ProductData B)
        {
            if (A.expDate > B.expDate) return 1;
            else if (A.expDate < B.expDate) return -1;
            return 0;
        });
        sortStockList.Sort(delegate (ProductData A, ProductData B)
        {
            if (A.stock > B.stock) return 1;
            else if (A.stock < B.stock) return -1;
            return 0;
        });
    }

    public void CheckProuct(string str)
    {
        string type = str.Substring(0, 1);
        int no = int.Parse(str.Substring(1));

        if(type=="D")
        {
            if (!dm.data.pdList[no].isSpend)
            {
                GameObject.Find("TodayImminent/TodayBackground/TodayDeadline/DeadlineBox/Viewport/Content/" + type + no + "/IsSpend/Check").GetComponent<Image>().sprite = Resources.Load("Image/Home/checked", typeof(Sprite)) as Sprite;
                
                if (dm.data.pdList[no].expDate.Date >= DateTime.Now.Date) // 유통기한을 지킨 경우
                {
                    dm.data.pdList[no].isSpend = true;
                }
                if(dm.data.isCompleteProductDelete)
                {
                    dm.data.pdList[no].isDeleted = true;
                }
            }
            else
            {
                GameObject.Find("TodayImminent/TodayBackground/TodayDeadline/DeadlineBox/Viewport/Content/" + type + no + "/IsSpend/Check").GetComponent<Image>().sprite = Resources.Load("Image/Home/check_none", typeof(Sprite)) as Sprite;
                dm.data.pdList[no].isSpend = false;
                dm.data.pdList[no].isDeleted = false;
            }
        }
        else
        {
            if (!dm.data.pdList[no].isSpend)
            {
                GameObject.Find("TodayImminent/TodayBackground/TodayStock/StockBox/Viewport/Content/" + type + no + "/IsSpend/Check").GetComponent<Image>().sprite = Resources.Load("Image/Home/checked", typeof(Sprite)) as Sprite;

                if (dm.data.pdList[no].expDate.Date >= DateTime.Now.Date) // 유통기한을 지킨 경우
                {
                    dm.data.pdList[no].isSpend = true;
                }
                if (dm.data.isCompleteProductDelete)
                {
                    dm.data.pdList[no].isDeleted = true;
                }
            }
            else
            {
                GameObject.Find("TodayImminent/TodayBackground/TodayStock/StockBox/Viewport/Content/" + type + no + "/IsSpend/Check").GetComponent<Image>().sprite = Resources.Load("Image/Home/check_none", typeof(Sprite)) as Sprite;
                dm.data.pdList[no].isSpend = false;
                dm.data.pdList[no].isDeleted = false;
            }
        }
        DataManager.SaveIngameData(dm.data);
    }

    public void ChangeNameBtnClick()
    {
        changeNamePop.SetActive(true);
        GameObject.Find("ChangeNamePopup/NameInputField").GetComponent<InputField>().text = dm.data.cname;
    }

    public void ChangeNameCloseBtnClick()
    {
        dm.data.cname = GameObject.Find("ChangeNamePopup/NameInputField/Text").GetComponent<Text>().text;
        DataManager.SaveIngameData(dm.data);
        changeNamePop.SetActive(false);
        GameObject.Find("Character/Name/NameBtn/NameTxt").GetComponent<Text>().text = dm.data.cname;
        GameObject.Find("MainTxt").GetComponent<Text>().text = "'" + dm.data.cname + "'님의 유통기한 준수율";
    }

    public void PointBtnClick()
    {
        pointHistory.SetActive(true);
    }

    public void PointCloseBtnClick()
    {
        pointHistory.SetActive(false);
    }

    public void ImminentBtnClick()
    {
        imminentPop.SetActive(true);
    }

    public void ImminentCloseBtnClick()
    {
        imminentPop.SetActive(false);
    }

    public void AttendanceCloseBtnClick()
    {
        attendancePop.SetActive(false);
    }

    public void ResultCloseBtnClick()
    {
        resultPop.SetActive(false);
    }

    public void GoProductLocker()
    {
        SceneManager.LoadScene("Locker");
    }
}
