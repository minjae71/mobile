using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class ProductData // ��ǰ������
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

    // ������
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
    // �ش� areaNo ���� �� ��ǰ �������� �ش� areaNo�� �ٷ� �� �浹 ���ɼ��� �����Ƿ� ����
    public int areaNo;
    public string areaName;
    public bool isDeleted;

    // ������
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
    // �ش� areaNo ���� �� ��ǰ �������� �ش� areaNo�� �ٷ� �� �浹 ���ɼ��� �����Ƿ� ����
    public DateTime getDate;
    public string contents;
    public int getPoint;

    // ������
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
    // �ش� areaNo ���� �� ��ǰ �������� �ش� areaNo�� �ٷ� �� �浹 ���ɼ��� �����Ƿ� ����
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

    // ������
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
public class SaveData // ����� ������
{
    public string version;

    public List<ProductData> pdList;
    public List<AreaData> adList;
    public List<PointData> ptList;
    public List<AchievementData> acList;

    public string cname; // ĳ���� �̸�

    //�⼮�� ���� ��¥ ���� ����
    public string attendance;
    public int countAttendance;

    public string setPopupDate;
    public bool isNoPopup;

    //������� �ؼ��� �Ǵ�
    public int safe; // pdList���� ������ ��ǰ�� �߿� ������� �ؼ��� ��ǰ ����
    public int total; // pdList���� ������ ��ǰ�� �� ����
    public int safe_price; // pdList���� ������ ��ǰ�� �߿� ��������� ���Ѽ� ������ ��ǰ�� ���� ����
    public int total_price; // pdList���� ������ ��ǰ�� ���� ����

    public int point; // ���� ����Ʈ

    // �ٹ̱�� ������(�̸��� ��Ģ�� �ο�)
    public int deco_col; // ĳ���� ����
    public int deco_pet; // ��
    public int deco_cos; // �ڽ�Ƭ
    public int deco_bg; // ���
    public int deco_nbg; // �̸��� ���

    // ������ �ٹ̱� ��� ������
    public bool[] have_deco_col;
    public bool[] have_deco_pet;
    public bool[] have_deco_cos;
    public bool[] have_deco_bg;
    public bool[] have_deco_nbg;

    // ȯ�漳�� ����
    public bool isCompleteProductDelete;
    public bool isPopup;
    public bool isSilent;
    // �� * 60 + ��, ����� ���� (silenctStart / 60)���� �ð��� ���ϰ� (silentStart % 60)���� ���� ���Ѵ�.
    public int silentStart;
    public int silentEnd;

    public int updateProductNo = 0;

    // ������
    public SaveData()
    {
        version = "v.0";

        pdList = new List<ProductData>(); // ����ִ� ����Ʈ �Ҵ�
        adList = new List<AreaData>();
        ptList = new List<PointData>();
        acList = new List<AchievementData>();

        cname = "������"; // ����Ʈ ����
        attendance = "20210909";
        countAttendance = 0;

        setPopupDate = "20211012";
        isNoPopup = false;

        safe = 0;
        total = 0;
        safe_price = 0;
        total_price = 0;

        point = 0;

        // �⺻ ����
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

        //����, ���, �̸��� ����� �ϳ��� �⺻ ����, ��� �ڽ�Ƭ�� ���� �̹����� �⺻ �̹���
        have_deco_col[0] = true;
        have_deco_pet[0] = true;
        have_deco_cos[0] = true;
        have_deco_bg[0] = true;
        have_deco_nbg[0] = true;

        // �ٹ̱� ���� �Ǵ� ����
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
        silentStart = 21 * 60 + 0; // pm 9�� 00��
        silentEnd = 6 * 60 + 30; // am 6�� 30��
    }

    // ������� �ؼ��� ��� �޼ҵ�
    public int CalRate()
    {
        int child = safe;
        int mother = total;

        // ���� ��ǰ��� �� ��������� ���� ǰ���� �ִ� ��� �ؼ����� ����߸���.
        foreach (ProductData pd in pdList)
        {
            if (pd.expDate.Date < DateTime.Now.Date) mother++;
        }

        if (mother == 0) return 100; // �ؼ��� ����� �ȵ� �� 100%�� ����
        else return child * 100 / mother;
    }

    // ĳ���� ���¸� �����ϴ� �޼ҵ�
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

    // ù ȭ�� �ε� slider�� �ؽ�Ʈ
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
        // AsyncOperation ���·� ������ �����ؾ� MainScene�� ����ǵ��� �Ѵ�.
        AsyncOperation operation = SceneManager.LoadSceneAsync("Home");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;

            // slider�� 90% ä�������� (100%�� �Ǿ �����Ͱ� �ε��� �ȵǾ� �ȳѾ� ���� ���� �����ϱ� ����)
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

            // slider�� 100% �̻� ä������ Touch to Start��� ������ ���´�.
            if (progressbar.value >= 1f)
            {
                loadtext.text = "ȭ���� ��ġ�ϼ���!";
            }

            // ȭ���� ��ġ(��ǻ�Ϳ����� ���콺 ���� Ŭ��)�� ���� �Ǹ� allowSceneActivation�� true�� �ٲ����ν� Scene �̵��� �ǵ��� �Ѵ�.
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
