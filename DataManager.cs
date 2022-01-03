using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class ProductData // 상품데이터
{
    public int productNo;
    public string productName;
    public string picture;
    public bool isDisplayMinus;
    public DateTime regDate;
    public DateTime expDate;
    public int stock;
    public int areaNo;
    public int price;
    public bool isSetAlarm;
    public DateTime alarmTime;
    public bool isSpend;
    public string memo;
    public bool isDeleted;

    // 생성자
    public ProductData(int no, string pn, string pic, bool isdm, DateTime ed, int st, int an, int pr, bool isa, DateTime at, bool isd, string mm, bool del)
    {
        productNo = no;
        productName = pn;
        picture = pic;
        isDisplayMinus = isdm;
        regDate = DateTime.Now;
        expDate = ed;
        stock = st;
        areaNo = an;
        price = pr;
        isSetAlarm = isa;
        alarmTime = at;
        isSpend = isd;
        memo = mm;
        isDeleted = del;
    }
}

[Serializable]
public class AreaData
{
    // 해당 areaNo 삭제 시 상품 데이터의 해당 areaNo를 다룰 때 충돌 가능성이 있으므로 주의
    public int areaNo;
    public string areaName;
    public bool isDeleted;

    // 생성자
    public AreaData(int num, string an, bool del)
    {
        areaNo = num;
        areaName = an;
        isDeleted = del;
    }
}

[Serializable]
public class PointData
{
    // 해당 areaNo 삭제 시 상품 데이터의 해당 areaNo를 다룰 때 충돌 가능성이 있으므로 주의
    public DateTime getDate;
    public string contents;
    public int getPoint;

    // 생성자
    public PointData(DateTime gd, string ct, int gp)
    {
        getDate = gd;
        contents = ct;
        getPoint = gp;
    }
}

[Serializable]
public class AchievementData
{
    // 해당 areaNo 삭제 시 상품 데이터의 해당 areaNo를 다룰 때 충돌 가능성이 있으므로 주의
    public string achievementTitle;
    public string achievementContent;
    public int require_1;
    public int require_2;
    public int require_3;
    public int require_4;
    public int require_5;
    public bool isAchieve_1;
    public bool isAchieve_2;
    public bool isAchieve_3;
    public bool isAchieve_4;
    public bool isAchieve_5;
    public bool isReward_1;
    public bool isReward_2;
    public bool isReward_3;
    public bool isReward_4;
    public bool isReward_5;

    // 생성자
    public AchievementData(string an, string ac, int re1, int re2, int re3, int re4, int re5, bool ia1, bool ia2, bool ia3, bool ia4, bool ia5, bool ir1, bool ir2, bool ir3, bool ir4, bool ir5)
    {
        achievementTitle = an;
        achievementContent = ac;
        require_1 = re1;
        require_2 = re2;
        require_3 = re3;
        require_4 = re4;
        require_5 = re5;
        isAchieve_1 = ia1;
        isAchieve_2 = ia2;
        isAchieve_3 = ia3;
        isAchieve_4 = ia4;
        isAchieve_5 = ia5;
        isReward_1 = ir1;
        isReward_2 = ir2;
        isReward_3 = ir3;
        isReward_4 = ir4;
        isReward_5 = ir5;
    }
}

[Serializable]
public class SaveData // 저장용 데이터
{
    public string version;

    public List<ProductData> pdList;
    public List<AreaData> adList;
    public List<PointData> ptList;
    public List<AchievementData> acList;

    public string cname; // 캐릭터 이름

    //출석을 위한 날짜 저장 변수
    public string attendance;
    public int countAttendance;

    public string setPopupDate;
    public bool isNoPopup;

    //유통기한 준수율 판단
    public int safe; // pdList에서 삭제된 상품들 중에 유통기한 준수한 상품 개수
    public int total; // pdList에서 삭제된 상품의 총 개수
    public int safe_price; // pdList에서 삭제된 상품들 중에 유통기한을 지켜서 삭제한 상품의 가격 총합
    public int total_price; // pdList에서 삭제된 상품의 가격 총합

    public int point; // 보유 포인트

    // 꾸미기용 데이터(이름에 규칙성 부여)
    public int deco_col; // 캐릭터 색상
    public int deco_pet; // 펫
    public int deco_cos; // 코스튬
    public int deco_bg; // 배경
    public int deco_nbg; // 이름판 배경

    // 보유한 꾸미기 목록 데이터
    public bool[] have_deco_col;
    public bool[] have_deco_pet;
    public bool[] have_deco_cos;
    public bool[] have_deco_bg;
    public bool[] have_deco_nbg;

    // 환경설정 변수
    public bool isCompleteProductDelete;
    public bool isPopup;
    public bool isSilent;
    // 시 * 60 + 분, 사용할 때는 (silenctStart / 60)으로 시간을 구하고 (silentStart % 60)으로 분을 구한다.
    public int silentStart;
    public int silentEnd;

    public int updateProductNo = 0;

    // 생성자
    public SaveData()
    {
        version = "v.0";

        pdList = new List<ProductData>(); // 비어있는 리스트 할당
        adList = new List<AreaData>();
        ptList = new List<PointData>();
        acList = new List<AchievementData>();

        cname = "유통이"; // 디폴트 네임
        attendance = "20210909";
        countAttendance = 0;

        setPopupDate = "20211012";
        isNoPopup = false;

        safe = 0;
        total = 0;
        safe_price = 0;
        total_price = 0;

        point = 0;

        // 기본 상태
        deco_col = 0;
        deco_pet = 0;
        deco_cos = 0;
        deco_bg = 0;
        deco_nbg = 0;

        have_deco_col = new bool[12];
        have_deco_pet = new bool[12];
        have_deco_cos = new bool[12];
        have_deco_bg = new bool[12];
        have_deco_nbg = new bool[12];

        //색상, 배경, 이름판 배경은 하나씩 기본 지급, 펫과 코스튬은 없는 이미지가 기본 이미지
        have_deco_col[0] = true;
        have_deco_pet[0] = true;
        have_deco_cos[0] = true;
        have_deco_bg[0] = true;
        have_deco_nbg[0] = true;

        // 꾸미기 보유 판단 변수
        for (int i=1;i<12;i++)
        {
            have_deco_col[i] = false;
            have_deco_pet[i] = false;
            have_deco_cos[i] = false;
            have_deco_bg[i] = false;
            have_deco_nbg[i] = false;
        }

        isCompleteProductDelete = true;
        isPopup = true;
        isSilent = false;
        silentStart = 21 * 60 + 0; // pm 9시 00분
        silentEnd = 6 * 60 + 30; // am 6시 30분
    }

    // 유통기한 준수율 계산 메소드
    public int CalRate()
    {
        int child = safe;
        int mother = total;

        // 현재 상품목록 중 유통기한이 지난 품목이 있는 경우 준수율을 떨어뜨린다.
        foreach (ProductData pd in pdList)
        {
            if (pd.expDate.Date < DateTime.Now.Date) mother++;
        }

        if (mother == 0) return 100; // 준수율 계산이 안될 때 100%를 리턴
        else return child * 100 / mother;
    }

    // 캐릭터 상태를 리턴하는 메소드
    public int CharState()
    {
        int r = CalRate();
        if (r >= 75) return 1;
        else if (r >= 50) return 2;
        else if (r >= 25) return 3;
        else return 4;
    }
}



public class DataManager : MonoBehaviour
{
    public SaveData data;

    // 첫 화면 로딩 slider와 텍스트
    public Slider progressbar;
    public Text loadtext;

    public Image loadingImg1;
    public Image loadingImg2;
    public Image loadingImg3;
    public Image loadingImg4;
    public Image loadingImg5;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        data = LoadIngameData();
        StartCoroutine("NextScene");
    }

    IEnumerator NextScene()
    {
        yield return null;
        // AsyncOperation 형태로 조건이 만족해야 MainScene이 실행되도록 한다.
        AsyncOperation operation = SceneManager.LoadSceneAsync("Home");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;

            // slider를 90% 채워지도록 (100%가 되어도 데이터가 로딩이 안되어 안넘어 가는 것을 방지하기 위해)
            if (progressbar.value < 0.2f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.2f, Time.deltaTime);
                loadingImg1.fillAmount = progressbar.value*5;
            }
            else if (progressbar.value >= 0.2f && progressbar.value < 0.4f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.4f, Time.deltaTime);
                loadingImg2.fillAmount = (progressbar.value-0.2f) * 5;
            }
            else if (progressbar.value >= 0.4f && progressbar.value < 0.6f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.6f, Time.deltaTime);
                loadingImg3.fillAmount = (progressbar.value - 0.4f) * 5;
            }
            else if (progressbar.value >= 0.6f && progressbar.value < 0.8f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.8f, Time.deltaTime);
                loadingImg4.fillAmount = (progressbar.value - 0.6f) * 5;
            }
            else if (operation.progress >= 0.8f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
                loadingImg5.fillAmount = (progressbar.value - 0.8f) * 5;
            }

            // slider가 100% 이상 채워지면 Touch to Start라는 문구가 나온다.
            if (progressbar.value >= 1f)
            {
                loadtext.text = "화면을 터치하세요!";
            }

            // 화면을 터치(컴퓨터에서는 마우스 왼쪽 클릭)후 떼게 되면 allowSceneActivation을 true로 바꿈으로써 Scene 이동이 되도록 한다.
            if (Input.GetMouseButtonDown(0) && progressbar.value >= 1f && operation.progress >= 0.8f)
            {
                operation.allowSceneActivation = true;
            }
        }
    }

    public static void SaveIngameData(SaveData dt)
    {
        string path = Application.persistentDataPath + "/IngameData.dat";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Create);

        formatter.Serialize(file, dt);
        file.Close();
    }

    SaveData LoadIngameData()
    {
        string path = Application.persistentDataPath + "/IngameData.dat";
        if (!File.Exists(path)) return null;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);

        SaveData dt = (SaveData)formatter.Deserialize(file);
        file.Close();
        return dt;
    }
}
