using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using Assets.SimpleAndroidNotifications;

public class UpdateProductManager : MonoBehaviour
{
    DataManager dm;

    //GameObject blur;

    GameObject imgPop;
    GameObject expClockPop;
    GameObject stockPop;
    GameObject areaPop;
    GameObject addAreaPop;
    GameObject pricePop;
    GameObject setAlarmPop;
    GameObject alarmClockPop;
    GameObject updatePop;
    GameObject errorPop;
    GameObject savePop;

    GameObject fadePanel;
    Image fadePop;
    Text fadeTxt;

    //GameObject cameraImg;
    //Image productCamera;
    Image productImage;
    Transform imgTrans;
    string imagePath = "none";
    //bool isCamera = true;

    Text displayPlus;
    Text displayMinus;

    bool isSetExpTime = true;

    DateTime expDay;
    int expHour = 0;
    int expMinute = 0;
    Text expAMPM;
    string expHourTxt;
    string expMinuteTxt;

    DateTime alarmDay;
    int alarmHour = 0;
    int alarmMinute = 0;
    Text alarmAMPM;
    string alarmHourTxt;
    string alarmMinuteTxt;

    public GameObject areaPrefab;

    bool isMinus = true;
    int stock = 0;
    int setAlarm = 1;
    String setAlarmTxt = "";

    bool isAlarm;

    //int imgChk = 0;
    bool areaChk = true; // ������ �����ߴ��� Ȯ���ϴ� ����
    bool priceChk = true; // ������ �Է��ߴ��� Ȯ���ϴ� ����

    int price = 0;
    int addAreaNum = 0;
    int areaCount = 0;
    int areaNum = 0;

    int pn = 0;

    // Start is called before the first frame update
    void Start()
    {
        dm = GameObject.Find("DataManager").GetComponent<DataManager>();
        areaCount = dm.data.adList.Count;
        pn = dm.data.updateProductNo;

        //blur = GameObject.Find("Blur");
        //blur.SetActive(false);
        //cameraImg = GameObject.Find("Background/Scroll/Viewport/Content/Product/Image/ProductCamera");
        //productCamera = GameObject.Find("Background/Scroll/Viewport/Content/Product/Image/ProductCamera").GetComponent<Image>();
        productImage = GameObject.Find("Background/Scroll/Viewport/Content/Product/Image/ProductImage").GetComponent<Image>();
        imgTrans = GameObject.Find("Background/Scroll/Viewport/Content/Product/Image/ProductImage").GetComponent<RectTransform>();
        imgPop = GameObject.Find("ImagePopup");
        expClockPop = GameObject.Find("ExpiryClockPopup");
        stockPop = GameObject.Find("StockPopup");
        areaPop = GameObject.Find("AreaPopup");
        addAreaPop = GameObject.Find("AddAreaPopup");
        pricePop = GameObject.Find("PricePopup");
        setAlarmPop = GameObject.Find("SetAlarmPopup");
        alarmClockPop = GameObject.Find("AlarmClockPopup");
        updatePop = GameObject.Find("UpdatePopup");
        errorPop = GameObject.Find("ErrorPopup");
        savePop = GameObject.Find("SavePopup");

        fadePanel = GameObject.Find("FadePanel");
        fadePop = GameObject.Find("FadePanel").GetComponent<Image>();
        fadeTxt = GameObject.Find("FadePanel/Text").GetComponent<Text>();

        displayMinus = GameObject.Find("Background/Scroll/Viewport/Content/ExpiryDate/Display/DisplayMinusBtn/Text").GetComponent<Text>();
        displayPlus = GameObject.Find("Background/Scroll/Viewport/Content/ExpiryDate/Display/DisplayPlusBtn/Text").GetComponent<Text>();

        for (int i = 0; i < areaCount; i++)
        {
            GameObject area = Instantiate(Resources.Load("Prefab/Area"), GameObject.Find("AreaPopup/AreaList/Viewport/Content").transform) as GameObject;
            area.name = dm.data.adList[i].areaNo.ToString();
            area.transform.Find("Text").GetComponent<Text>().text = dm.data.adList[i].areaName;
            area.GetComponent<Button>().onClick.AddListener(() => AreaClick(int.Parse(area.name)));
        }

        expAMPM = GameObject.Find("ExpiryClockPopup/Clock/AMPM").GetComponent<Text>();
        expHourTxt = GameObject.Find("ExpiryClockPopup/Clock/Hour").GetComponent<InputField>().text;
        expMinuteTxt = GameObject.Find("ExpiryClockPopup/Clock/Minute").GetComponent<InputField>().text;

        alarmAMPM = GameObject.Find("AlarmClockPopup/Clock/AMPM").GetComponent<Text>();
        alarmHourTxt = GameObject.Find("AlarmClockPopup/Clock/Hour").GetComponent<InputField>().text;
        alarmMinuteTxt = GameObject.Find("AlarmClockPopup/Clock/Minute").GetComponent<InputField>().text;

        SettingProduct();

        //cameraImg.SetActive(false);
        fadePop.color = new Color(255, 255, 255, 0);
        fadeTxt.color = new Color(255, 255, 255, 0);
        imgPop.SetActive(false);
        expClockPop.SetActive(false);
        stockPop.SetActive(false);
        areaPop.SetActive(false);
        addAreaPop.SetActive(false);
        pricePop.SetActive(false);
        setAlarmPop.SetActive(false);
        alarmClockPop.SetActive(false);
        updatePop.SetActive(false);
        errorPop.SetActive(false);
        savePop.SetActive(false);
        fadePanel.SetActive(false);

        stock = int.Parse(GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text);
    }

    void Update()
    {
        // �ڷΰ��� ��ư�� �ι� ���� �� MainScene���� ���ư���.
        if (Input.GetKey(KeyCode.Escape))
        {
            LoadingSceneManager.Instance.LoadScene("Locker");
        }
    }

    public void SettingProduct()
    {
        GameObject.Find("ProductName/ProductNameInput").GetComponent<InputField>().text = dm.data.pdList[pn].productName;
        DisplayBtnClick(dm.data.pdList[pn].isDisplayMinus);

        imagePath = dm.data.pdList[pn].picture;
        if(imagePath!="none")
        {
            UpdateImage(imagePath);
        }
        //productImage = dm.data.pdList[pn].picture;
        //GameObject.Find("Background/ProductImage").GetComponent<Image>().sprite = productImage.sprite;

        //isCamera = dm.data.pdList[pn].isTakePicture;

        expDay = dm.data.pdList[pn].expDate;
        GameObject.Find("Background/Scroll/Viewport/Content/ExpiryDate/Date/Text_Select_Date").GetComponent<Text>().text = expDay.ToString("yyyy-MM-dd");

        expHour = expDay.Hour;
        expMinute = expDay.Minute;
        if (expHour == 0)
            expHour = 12;
        if (expHour > 12)
        {
            expAMPM.text = "����";
            expHour -= 12;
            expHourTxt = expHour + "";
            expMinuteTxt = expMinute + "";
            if (expHour < 10)
                expHourTxt = "0" + expHour;
            if (expMinute < 10)
                expMinuteTxt = "0" + expMinute;
            GameObject.Find("ExpiryDate/Date/Clock/Time").GetComponent<Text>().text = "���� " + expHourTxt + ":" + expMinuteTxt;
            expHour += 12;
        }
        else
        {
            expAMPM.text = "����";
            expHourTxt = expHour + "";
            expMinuteTxt = expMinute + "";
            if (expHour < 10)
                expHourTxt = "0" + expHour;
            if (expMinute < 10)
                expMinuteTxt = "0" + expMinute;
            GameObject.Find("ExpiryDate/Date/Clock/Time").GetComponent<Text>().text = "���� " + expHourTxt + ":" + expMinuteTxt;
        }
        if (alarmHour == 12)
            alarmHour = 0;

        stock = dm.data.pdList[pn].stock;
        GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text = stock + "";

        GameObject.Find("Area/AreaBtn/Text").GetComponent<Text>().text = dm.data.adList[dm.data.pdList[pn].areaNo].areaName;
        areaNum = dm.data.pdList[pn].areaNo;

        price = dm.data.pdList[pn].price;
        GameObject.Find("Price/PriceBtn/Text").GetComponent<Text>().text = price + "";

        if (!dm.data.pdList[pn].isSetAlarm)
        {
            isAlarm = false;
            GameObject.Find("SetAlarm/SetAlarmBtn/Text").GetComponent<Text>().text = "�������� ����";
            GameObject.Find("SetAlarm/NoAlarmBtn").GetComponent<Image>().sprite = Resources.Load("Image/AlarmOff", typeof(Sprite)) as Sprite;
        }
        else
        {
            isAlarm = true;
            TimeSpan diffTime = dm.data.pdList[pn].expDate - dm.data.pdList[pn].alarmTime;
            setAlarm = diffTime.Days + 1;
            alarmHour = dm.data.pdList[pn].alarmTime.Hour;
            alarmMinute = dm.data.pdList[pn].alarmTime.Minute;
            if (alarmHour == 0)
                alarmHour = 12;
            if (alarmHour>12)
            {
                alarmHour -= 12;
                alarmHourTxt = alarmHour + "";
                alarmMinuteTxt = alarmMinute + "";
                if (alarmHour < 10)
                    alarmHourTxt = "0" + alarmHour;
                if (alarmMinute < 10)
                    alarmMinuteTxt = "0" + alarmMinute;
                GameObject.Find("SetAlarm/SetAlarmBtn/Text").GetComponent<Text>().text = (setAlarm) + "�� �� ���� " + alarmHourTxt + ":" + alarmMinuteTxt;
                alarmHour += 12;
            }
            else
            {
                alarmHourTxt = alarmHour + "";
                alarmMinuteTxt = alarmMinute + "";
                if (alarmHour < 10)
                    alarmHourTxt = "0" + alarmHour;
                if (alarmMinute < 10)
                    alarmMinuteTxt = "0" + alarmMinute;
                GameObject.Find("SetAlarm/SetAlarmBtn/Text").GetComponent<Text>().text = (setAlarm) + "�� �� ���� " + alarmHourTxt + ":" + alarmMinuteTxt;
            }
            if (alarmHour == 12)
                alarmHour = 0;
        }

        GameObject.Find("Memo/MemoInput").GetComponent<InputField>().text = dm.data.pdList[pn].memo;
    }

    public void DisplayBtnClick(bool minus)
    {
        if (minus)
        {
            isMinus = true;
            displayMinus.text = "<color=#000000>D-</color>";
            displayPlus.text = "<color=#ADAEAD>D+</color>";
        }
        else
        {
            isMinus = false;
            displayPlus.text = "<color=#000000>D+</color>";
            displayMinus.text = "<color=#ADAEAD>D-</color>";
        }
    }

    public void ImageAddBtnClick()
    {
        imgPop.SetActive(true);
    }

    public void FilmingBtnClick()
    {
#if UNITY_EDITOR
#else
        NativeCamera.TakePicture(callback);
#endif
    }

    private void callback(String path)
    {
        //isCamera = true;
        imgPop.SetActive(false);
        //cameraImg.SetActive(true);
        SaveJpgAndUpdateImage(path);
    }

    public void GalleryBtnClick()
    {
#if UNITY_EDITOR
#else
        NativeGallery.GetImageFromGallery(callbackForGallery);
#endif
    }

    private void callbackForGallery(string path)
    {
        //isCamera = false;
        imgPop.SetActive(false);
        //cameraImg.SetActive(false);
        UpdateImage(path);
    }

    public void SaveJpgAndUpdateImage(string path)
    {
        NativeCamera.ImageProperties prop = NativeCamera.GetImageProperties(path);
        Vector2 imageSize = new Vector2(prop.width, prop.height);
        Texture2D text = LoadImage(imageSize, path);
        Texture2D rotateTex;
        string rot = "";
        if (prop.width <= prop.height)
        {
            rotateTex = RotateTexture(text, true);
            text = rotateTex;
            rot = "_rotateUtongE";
        }

        string newPath = "/storage/emulated/0/DCIM/Camera/" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + rot + ".jpg";
        StartCoroutine(SaveJpg(newPath, text));

        imagePath = newPath;

        int width = text.width;
        int height = text.height;
        int spaceW = 0;
        int spaceH = 0;
        if (text.width > text.height)
        {
            width = text.width - (text.width - text.height);
            spaceW = (text.width - text.height) / 2;
        }
        else if (text.width < text.height)
        {
            height = text.height - (text.height - text.width);
            spaceH = (text.height - text.width) / 2;
        }
        productImage.sprite = Sprite.Create(text, new Rect(spaceW, spaceH, width, height), new Vector2(0, 0));

    }

    IEnumerator SaveJpg(string path, Texture2D text)
    {
        yield return new WaitForEndOfFrame();

        byte[] bytes = text.EncodeToJPG();
        File.WriteAllBytes(path, bytes);
    }

    public void UpdateImage(string path)
    {
        imgTrans.rotation = Quaternion.Euler(0, 0, 0);
        NativeGallery.ImageProperties prop = NativeGallery.GetImageProperties(path);
        Vector2 imageSize = new Vector2(prop.width, prop.height);
        Texture2D text = LoadImage(imageSize, path);

        //Texture2D rotateTex;
        string cameraPath = "/storage/emulated/0/DCIM/Camera/";
        string rot = "_rotateUtongE";
        if (path.Contains(cameraPath) && prop.width <= prop.height)
        {
            //rotateTex = RotateTexture(text, true);
            //text = rotateTex;
            if (!imagePath.Contains(rot))
            {
                imgTrans.rotation = Quaternion.Euler(0, 0, 270);
            }
        }

        imagePath = path;

        int width = text.width;
        int height = text.height;
        int spaceW = 0;
        int spaceH = 0;
        if (text.width > text.height)
        {
            width = text.width - (text.width - text.height);
            spaceW = (text.width - text.height) / 2;
        }
        else if (text.width < text.height)
        {
            height = text.height - (text.height - text.width);
            spaceH = (text.height - text.width) / 2;
        }
        productImage.sprite = Sprite.Create(text, new Rect(spaceW, spaceH, width, height), new Vector2(0, 0));
    }

    private static Texture2D LoadImage(Vector2 size, string filePath)
    {
        byte[] bytes = File.ReadAllBytes(filePath);
        Texture2D texture;

        texture = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGB24, false);

        texture.filterMode = FilterMode.Trilinear;
        texture.LoadImage(bytes);

        return texture;
    }

    public static Texture2D RotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }

    public void ExpiryTimeOnOff(bool isOn)
    {
        if (isOn)
        {
            isSetExpTime = true;
            GameObject.Find("ExpiryClockPopup/Clock/AMPM/Up").GetComponent<Button>().interactable = true;
            GameObject.Find("ExpiryClockPopup/Clock/AMPM/Down").GetComponent<Button>().interactable = true;
            GameObject.Find("ExpiryClockPopup/Clock/Hour/Up").GetComponent<Button>().interactable = true;
            GameObject.Find("ExpiryClockPopup/Clock/Hour/Down").GetComponent<Button>().interactable = true;
            GameObject.Find("ExpiryClockPopup/Clock/Minute/Up").GetComponent<Button>().interactable = true;
            GameObject.Find("ExpiryClockPopup/Clock/Minute/Down").GetComponent<Button>().interactable = true;
            GameObject.Find("ExpiryClockPopup/Clock/AMPM").GetComponent<Text>().color = new Color(24 / 255f, 97 / 255f, 52 / 255f);
            GameObject.Find("ExpiryClockPopup/Clock/Hour/Text").GetComponent<Text>().color = new Color(24 / 255f, 97 / 255f, 52 / 255f);
            GameObject.Find("ExpiryClockPopup/Clock/Minute/Text").GetComponent<Text>().color = new Color(24 / 255f, 97 / 255f, 52 / 255f);
        }
        else
        {
            isSetExpTime = false;
            GameObject.Find("ExpiryClockPopup/Clock/AMPM/Up").GetComponent<Button>().interactable = false;
            GameObject.Find("ExpiryClockPopup/Clock/AMPM/Down").GetComponent<Button>().interactable = false;
            GameObject.Find("ExpiryClockPopup/Clock/Hour/Up").GetComponent<Button>().interactable = false;
            GameObject.Find("ExpiryClockPopup/Clock/Hour/Down").GetComponent<Button>().interactable = false;
            GameObject.Find("ExpiryClockPopup/Clock/Minute/Up").GetComponent<Button>().interactable = false;
            GameObject.Find("ExpiryClockPopup/Clock/Minute/Down").GetComponent<Button>().interactable = false;
            GameObject.Find("ExpiryClockPopup/Clock/AMPM").GetComponent<Text>().color = new Color(227 / 255f, 227 / 255f, 227 / 255f);
            GameObject.Find("ExpiryClockPopup/Clock/Hour/Text").GetComponent<Text>().color = new Color(227 / 255f, 227 / 255f, 227 / 255f);
            GameObject.Find("ExpiryClockPopup/Clock/Minute/Text").GetComponent<Text>().color = new Color(227 / 255f, 227 / 255f, 227 / 255f);
        }
    }

    public void ExpiryClockButtonClick()
    {
        expClockPop.SetActive(true);
    }

    public void ExpiryAMPMBtnClick()
    {
        if (expAMPM.text == "����")
        {
            expAMPM.text = "����";
        }
        else
        {
            expAMPM.text = "����";
        }
    }

    public void ExpiryHourBtnClick(int type)
    {
        expHourTxt = GameObject.Find("ExpiryClockPopup/Clock/Hour").GetComponent<InputField>().text;
        expHour = int.Parse(expHourTxt);
        if (type == 1)
        {
            expHour++;
            if (expHour > 12)
            {
                expHour = 1;

            }
            expHourTxt = expHour + "";
            if (expHour < 10)
            {
                expHourTxt = "0" + expHour;
            }
        }
        else
        {
            expHour--;
            if (expHour < 1)
            {
                expHour = 12;

            }
            expHourTxt = expHour + "";
            if (expHour < 10)
            {
                expHourTxt = "0" + expHour;
            }
        }
        GameObject.Find("ExpiryClockPopup/Clock/Hour").GetComponent<InputField>().text = expHourTxt;
    }

    public void ExpiryMinuteBtnClick(int type)
    {
        expMinuteTxt = GameObject.Find("ExpiryClockPopup/Clock/Minute").GetComponent<InputField>().text;
        expMinute = int.Parse(expMinuteTxt);
        if (type == 1)
        {
            expMinute++;
            if (expMinute > 59)
            {
                expMinute = 0;

            }
            expMinuteTxt = expMinute + "";
            if (expMinute < 10)
            {
                expMinuteTxt = "0" + expMinute;
            }
        }
        else
        {
            expMinute--;
            if (expMinute < 0)
            {
                expMinute = 59;

            }
            expMinuteTxt = expMinute + "";
            if (expMinute < 10)
            {
                expMinuteTxt = "0" + expMinute;
            }
        }
        GameObject.Find("ExpiryClockPopup/Clock/Minute").GetComponent<InputField>().text = expMinuteTxt;
    }

    

    public void StockMinusBtnClick()
    {
        stock = int.Parse(GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text);

        if (stock <= 1)
        {
            stock = 1;
        }
        else
        {
            --stock;
        }

        GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text = stock.ToString();
    }

    public void StockPlusBtnClick()
    {
        stock = int.Parse(GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text);

        if (stock >= 99)
        {
            stock = 99;
        }
        else
        {
            ++stock;
        }

        GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text = stock.ToString();
    }

    public void StockBtnClick()
    {
        stock = int.Parse(GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text);
        stockPop.SetActive(true);
        //blur.SetActive(true);

        GameObject.Find("StockPopup/Stock").GetComponent<Text>().text = stock.ToString();
    }

    public void StockPopupMinusBtnClick()
    {
        stock = int.Parse(GameObject.Find("StockPopup/Stock").GetComponent<Text>().text);

        if (stock <= 1)
        {
            stock = 1;
        }
        else
        {
            --stock;
        }
        GameObject.Find("StockPopup/Stock").GetComponent<Text>().text = stock.ToString();
        GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text = stock.ToString();
    }

    public void StockPopupPlusBtnClick()
    {
        stock = int.Parse(GameObject.Find("StockPopup/Stock").GetComponent<Text>().text);

        if (stock >= 99)
        {
            stock = 99;
        }
        else
        {
            ++stock;
        }
        GameObject.Find("StockPopup/Stock").GetComponent<Text>().text = stock.ToString();
        GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text = stock.ToString();
    }

    public void AreaBtnClick()
    {
        areaPop.SetActive(true);
        //blur.SetActive(true);
        if (addAreaNum != 0)
        {
            for (int i = areaCount; i < areaCount + addAreaNum; i++)
            {
                if (!dm.data.adList[i].isDeleted)
                {
                    GameObject area = Instantiate(Resources.Load("Prefab/Area"), GameObject.Find("AreaPopup/AreaList/Viewport/Content").transform) as GameObject;
                    area.name = dm.data.adList[i].areaNo.ToString();
                    area.transform.Find("Text").GetComponent<Text>().text = dm.data.adList[i].areaName;
                    area.GetComponent<Button>().onClick.AddListener(() => AreaClick(int.Parse(area.name)));
                }
            }
            addAreaNum = 0;
        }
    }

    public void AreaClick(int no)
    {
        areaChk = true;
        GameObject.Find("Area/AreaBtn/Text").GetComponent<Text>().text = GameObject.Find("AreaPopup/AreaList/Viewport/Content/" + no + "/Text").GetComponent<Text>().text;
        areaNum = no;
        areaPop.SetActive(false);
        //blur.SetActive(false);
    }

    public void AddAreaBtnClick()
    {
        //blur.SetActive(true);
        addAreaPop.SetActive(true);
    }

    public void AddArea()
    {
        string newArea = GameObject.Find("AddAreaPopup/AreaInput/Text").GetComponent<Text>().text;
        addAreaNum++;
        areaNum = dm.data.adList[dm.data.adList.Count - 1].areaNo + 1;
        GameObject.Find("Area/AreaBtn/Text").GetComponent<Text>().text = newArea;
        dm.data.adList.Add(new AreaData(areaNum, newArea, false));
        DataManager.SaveIngameData(dm.data);
        areaChk = true;
        //blur.SetActive(false);
        addAreaPop.SetActive(false);
        areaPop.SetActive(false);
    }

    public void PriceBtnClick()
    {
        //blur.SetActive(true);
        if (priceChk)
        {
            price = int.Parse(GameObject.Find("Price/PriceBtn/Text").GetComponent<Text>().text);
        }
        pricePop.SetActive(true);

        if (priceChk == true)
        {
            GameObject.Find("PricePopup/PriceInput/Placeholder").GetComponent<Text>().text = GameObject.Find("Price/PriceBtn/Text").GetComponent<Text>().text;
            GameObject.Find("PricePopup/PriceInput/Text").GetComponent<Text>().text = GameObject.Find("Price/PriceBtn/Text").GetComponent<Text>().text;
        }
    }

    public void AlarmOnOff()
    {
        if (isAlarm)
        {
            GameObject.Find("SetAlarm/NoAlarmBtn").GetComponent<Image>().sprite = Resources.Load("Image/alarmOff", typeof(Sprite)) as Sprite;
            GameObject.Find("SetAlarm/SetAlarmBtn").GetComponent<Button>().interactable = false;
            isAlarm = false;
            FadeControll();
        }
        else
        {
            GameObject.Find("SetAlarm/NoAlarmBtn").GetComponent<Image>().sprite = Resources.Load("Image/alarmOn", typeof(Sprite)) as Sprite;
            GameObject.Find("SetAlarm/SetAlarmBtn").GetComponent<Button>().interactable = true;
            isAlarm = true;
            FadeControll();
        }
    }

    public void SetAlarmBtnClick()
    {
        //blur.SetActive(true);
        setAlarmPop.SetActive(true);
    }

    public void SetAlarmDay(int day)
    {
        setAlarm = day;

        GameObject.Find("SetAlarm/SetAlarmBtn/Text").GetComponent<Text>().text = day + "�� ��";
        alarmClockPop.SetActive(true);
        setAlarmPop.SetActive(false);
    }

    public void AlarmAMPMBtnClick()
    {
        if (alarmAMPM.text == "����")
        {
            alarmAMPM.text = "����";
        }
        else
        {
            alarmAMPM.text = "����";
        }
    }

    public void AlarmHourBtnClick(int type)
    {
        alarmHourTxt = GameObject.Find("AlarmClockPopup/Clock/Hour").GetComponent<InputField>().text;
        alarmHour = int.Parse(alarmHourTxt);
        if (type == 1)
        {
            alarmHour++;
            if (alarmHour > 12)
            {
                alarmHour = 1;

            }
            alarmHourTxt = alarmHour + "";
            if (alarmHour < 10)
            {
                alarmHourTxt = "0" + alarmHour;
            }
        }
        else
        {
            alarmHour--;
            if (alarmHour < 1)
            {
                alarmHour = 12;

            }
            alarmHourTxt = alarmHour + "";
            if (alarmHour < 10)
            {
                alarmHourTxt = "0" + alarmHour;
            }
        }
        GameObject.Find("AlarmClockPopup/Clock/Hour").GetComponent<InputField>().text = alarmHourTxt;
    }

    public void AlarmMinuteBtnClick(int type)
    {
        alarmMinuteTxt = GameObject.Find("AlarmClockPopup/Clock/Minute").GetComponent<InputField>().text;
        alarmMinute = int.Parse(alarmMinuteTxt);
        if (type == 1)
        {
            alarmMinute++;
            if (alarmMinute > 59)
            {
                alarmMinute = 0;

            }
            alarmMinuteTxt = alarmMinute + "";
            if (alarmMinute < 10)
            {
                alarmMinuteTxt = "0" + alarmMinute;
            }
        }
        else
        {
            alarmMinute--;
            if (alarmMinute < 0)
            {
                alarmMinute = 59;

            }
            alarmMinuteTxt = alarmMinute + "";
            if (alarmMinute < 10)
            {
                alarmMinuteTxt = "0" + alarmMinute;
            }
        }
        GameObject.Find("AlarmClockPopup/Clock/Minute").GetComponent<InputField>().text = alarmMinuteTxt;
    }

    public void PopupCloseBtnClick(int no)
    {
        //blur.SetActive(false);
        switch (no)
        {
            case 1:
                imgPop.SetActive(false);
                break;
            case 2:
                GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text = GameObject.Find("StockPopup/Stock").GetComponent<Text>().text;
                stockPop.SetActive(false);
                break;
            case 3:
                areaPop.SetActive(false);
                areaChk = true;
                break;
            case 4:
                AddArea();
                break;
            case 5:
                if (price == 0)
                {
                    GameObject.Find("PricePopup/PriceInput/Text").GetComponent<Text>().text = "0";
                }
                GameObject.Find("Price/PriceBtn/Text").GetComponent<Text>().text = GameObject.Find("PricePopup/PriceInput/Text").GetComponent<Text>().text;
                priceChk = true;
                pricePop.SetActive(false);
                break;
            case 6:
                setAlarmTxt = GameObject.Find("AlarmInput/Text").GetComponent<Text>().text;
                if (setAlarmTxt != "")
                {
                    setAlarm = int.Parse(setAlarmTxt);
                    alarmClockPop.SetActive(true);
                }
                setAlarmPop.SetActive(false);
                break;
            case 7:
                alarmHourTxt = GameObject.Find("AlarmClockPopup/Clock/Hour").GetComponent<InputField>().text;
                alarmMinuteTxt = GameObject.Find("AlarmClockPopup/Clock/Minute").GetComponent<InputField>().text;
                GameObject.Find("SetAlarm/SetAlarmBtn/Text").GetComponent<Text>().text = setAlarm + "�� �� " + alarmAMPM.text + " " + alarmHourTxt + ":" + alarmMinuteTxt;
                alarmHour = int.Parse(alarmHourTxt);
                alarmMinute = int.Parse(alarmMinuteTxt);
                if (alarmAMPM.text == "����")
                {
                    alarmHour += 12;
                }
                alarmClockPop.SetActive(false);
                FadeControll();
                break;
            case 8:
                errorPop.SetActive(false);
                break;
            case 9:
                SceneManager.LoadScene("Locker");
                break;
            case 10:
                if (isSetExpTime)
                {
                    expHourTxt = GameObject.Find("ExpiryClockPopup/Clock/Hour").GetComponent<InputField>().text;
                    expMinuteTxt = GameObject.Find("ExpiryClockPopup/Clock/Minute").GetComponent<InputField>().text;
                    GameObject.Find("ExpiryDate/Date/Clock/Time").GetComponent<Text>().text = expAMPM.text + " " + expHourTxt + ":" + expMinuteTxt;
                }
                else
                {
                    expAMPM.text = "����";
                    expHourTxt = "11";
                    expMinuteTxt = "59";
                    GameObject.Find("ExpiryDate/Date/Clock/Time").GetComponent<Text>().text = "���� 11:59";
                }
                expHour = int.Parse(expHourTxt);
                expMinute = int.Parse(expMinuteTxt);
                if (expAMPM.text == "����")
                {
                    expHour += 12;
                }
                expClockPop.SetActive(false);
                break;
        }
    }
    public void SaveBtnClick()
    {
        //blur.SetActive(true);
        updatePop.SetActive(true);
    }

    public void OkButtonClick(bool ok)
    {
        if(ok)
        {
            ProductUpdate();
        }
        else
        {
            updatePop.SetActive(false);
        }
    }

    public void FadeControll()
    {
        fadePanel.SetActive(true);
        if (isAlarm)
        {
            if (GameObject.Find("Background/Scroll/Viewport/Content/ExpiryDate/Date/Text_Select_Date").GetComponent<Text>().text != "YY/MM/DD (��)")
            {
                expDay = Convert.ToDateTime(GameObject.Find("Background/Scroll/Viewport/Content/ExpiryDate/Date/Text_Select_Date").GetComponent<Text>().text);
                alarmDay = Convert.ToDateTime(expDay.AddDays(0 - setAlarm));
                if (alarmAMPM.text == "����" && alarmHour == 12)
                {
                    alarmHour = 0;
                }
                if (alarmHour == 24)
                {
                    alarmHour = 12;
                }
                alarmDay = alarmDay.AddHours(alarmHour);
                alarmDay = alarmDay.AddMinutes(alarmMinute);
                TimeSpan setTime = alarmDay - DateTime.Now;
                if (alarmDay < DateTime.Now)
                {
                    GameObject.Find("FadePanel/Text").GetComponent<Text>().text = "�˶� ���� �ð��� Ȯ���� �ּ���!";
                }
                else
                {
                    if (setTime.Days >= 1)
                    {
                        GameObject.Find("FadePanel/Text").GetComponent<Text>().text = setTime.Days + "�� " + setTime.Hours + "�ð� " + setTime.Minutes + "�� �ڿ� �˸� ����";
                    }
                    else
                    {
                        GameObject.Find("FadePanel/Text").GetComponent<Text>().text = setTime.Hours + "�ð� " + setTime.Minutes + "�� �ڿ� �˸� ����";
                    }
                }
            }
            else
            {
                GameObject.Find("FadePanel/Text").GetComponent<Text>().text = "���� ������ ������ �ּ���.";
            }
        }
        else
        {
            GameObject.Find("FadePanel/Text").GetComponent<Text>().text = "�˸� ������ �����Ǿ����ϴ�.";
        }
        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        yield return StartCoroutine(FadeIn());
        yield return StartCoroutine(Wait());
        yield return StartCoroutine(FadeOut());
        fadePanel.SetActive(false);
    }

    IEnumerator FadeIn()
    {
        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadePop.color = new Color(255, 255, 255, fadeCount);
            fadeTxt.color = new Color(255, 255, 255, fadeCount);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator FadeOut()
    {
        float fadeCount = 1.0f;
        while (fadeCount > 0)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadePop.color = new Color(255, 255, 255, fadeCount);
            fadeTxt.color = new Color(255, 255, 255, fadeCount);
        }
    }

    public void ProductUpdate()
    {
        bool checkSave = true;
        errorPop.SetActive(true);
        if (GameObject.Find("ProductName/ProductNameInput/Text").GetComponent<Text>().text == "")
        {
            GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "��ǰ���� �Է��� �ּ���.";
            checkSave = false;
        }
        else if (GameObject.Find("Background/Scroll/Viewport/Content/ExpiryDate/Date/Text_Select_Date").GetComponent<Text>().text == "YY/MM/DD (��)")
        {
            GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "���� ������ ������ �ּ���.";
            checkSave = false;
        }
        else if (areaChk == false)
        {
            GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "������ ������ �ּ���.";
            checkSave = false;
        }
        else if (priceChk == false)
        {
            GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "������ �Է��� �ּ���.";
            checkSave = false;
        }
        else
        {
            expDay = Convert.ToDateTime(GameObject.Find("Background/Scroll/Viewport/Content/ExpiryDate/Date/Text_Select_Date").GetComponent<Text>().text);
            if (isSetExpTime)
            {
                if (expAMPM.text == "����" && expHour == 12)
                {
                    expHour = 0;
                }
                if (expHour == 24)
                {
                    expHour = 12;
                }
                expDay = expDay.AddHours(expHour);
                expDay = expDay.AddMinutes(expMinute);
            }
            else
            {
                expDay = expDay.AddHours(23);
                expDay = expDay.AddMinutes(59);
            }
            alarmDay = Convert.ToDateTime(expDay.AddDays(0 - setAlarm));
            if (alarmAMPM.text == "����" && alarmHour == 12)
            {
                alarmHour = 0;
            }
            if (alarmHour == 24)
            {
                alarmHour = 12;
            }
            alarmDay = alarmDay.AddHours(alarmHour);
            alarmDay = alarmDay.AddMinutes(alarmMinute);

            int silStart = dm.data.silentStart;
            int silEnd = dm.data.silentEnd;
            int startHour = silStart / 60;
            int startMin = silStart % 60;
            int endHour = silEnd / 60;
            int endMin = silEnd % 60;
            string silentStr = "";
            bool silentChk = true;
            if (dm.data.isSilent)
            {
                if (isAlarm)
                {
                    if (startHour < 10)
                        silentStr += "0" + startHour + ":";
                    else
                        silentStr += startHour + ":";

                    if (startMin < 10)
                        silentStr += "0" + startMin + "~";
                    else
                        silentStr += startMin + "~";

                    if (endHour < 10)
                        silentStr += "0" + endHour + ":";
                    else
                        silentStr += endHour + ":";

                    if (endMin < 10)
                        silentStr += "0" + endMin;
                    else
                        silentStr += endMin + "";

                    if (silStart < silEnd)
                    {
                        if (alarmDay.Hour >= startHour && alarmDay.Hour <= endHour)
                        {
                            if (alarmDay.Minute > startMin || alarmDay.Minute < endMin)
                            {
                                checkSave = false;
                                silentChk = false;
                            }
                        }
                    }
                    else
                    {
                        if (alarmDay.Hour >= startHour || alarmDay.Hour <= endHour)
                        {
                            if (alarmDay.Minute > startMin || alarmDay.Minute < endMin)
                            {
                                checkSave = false;
                                silentChk = false;
                            }
                        }
                    }
                }
            }
            if (expDay < DateTime.Now)
            {
                GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "���� ������ ���� �ð� �������� ������ �� �����ϴ�!";
                checkSave = false;
            }
            else if (expDay < alarmDay)
            {
                if (isAlarm)
                {
                    GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "�˶� ���� �ð��� ������� ���ķ� ������ �� �����ϴ�!";
                    checkSave = false;
                }
            }
            else if (alarmDay < DateTime.Now)
            {
                if (isAlarm)
                {
                    GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "�˶� ���� �ð��� ���� �ð� ���ķ� ������ �ּ���!";
                    checkSave = false;
                }
            }
            else if (!silentChk)
            {
                if (isAlarm)
                {
                    GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "�˶� ���� �ð��� ���ر��� �ð��뿡 ���ԵǾ� �ֽ��ϴ�!\n" +
                    "���ر��� �ð� = " + silentStr;
                    checkSave = false;
                }
            }

            if (checkSave)
            {
                errorPop.SetActive(false);

                string content = "";

                NotificationManager.Cancel(pn);
                NotificationManager.Cancel(pn + 1000000);

                
                content = GameObject.Find("ProductName/ProductNameInput/Text").GetComponent<Text>().text;
                dm.data.pdList[pn].productName = content;
                Debug.Log("��ǰ�� : " + content);

                dm.data.pdList[pn].picture = imagePath;

                //dm.data.pdList[pn].isTakePicture = isCamera;

                dm.data.pdList[pn].isDisplayMinus = isMinus;

                dm.data.pdList[pn].expDate = expDay;
                Debug.Log("������� : " + expDay + "����");

                dm.data.pdList[pn].stock = int.Parse(GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text);
                Debug.Log("��� : " + GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text);

                dm.data.pdList[pn].areaNo = areaNum;
                Debug.Log("���� : " + GameObject.Find("Area/AreaBtn/Text").GetComponent<Text>().text);

                dm.data.pdList[pn].price = int.Parse(GameObject.Find("Price/PriceBtn/Text").GetComponent<Text>().text);
                Debug.Log("���� : " + GameObject.Find("Price/PriceBtn/Text").GetComponent<Text>().text);

                if (!isAlarm)
                {
                    dm.data.pdList[pn].isSetAlarm = false;
                    Debug.Log("�˶� ���� : X");
                }
                else
                {
                    dm.data.pdList[pn].isSetAlarm = true;
                    Debug.Log("�˶� ���� : O");

                    dm.data.pdList[pn].alarmTime = alarmDay;
                    Debug.Log("�˸� : " + alarmDay);

                    DateTime timeToNotify = alarmDay;
                    TimeSpan time = timeToNotify - DateTime.Now;
                    TimeSpan diffTime = expDay - alarmDay;

                    if (diffTime.Days > 1)
                    {
                        content += " ��ǰ�� ��������� " + diffTime.Days + "�� ���ҽ��ϴ�.";
                    }
                    else
                    {
                        content += " ��ǰ�� ��������� ���� ����˴ϴ�.";
                    }

                    NotificationManager.SendWithAppIcon(pn, time, "������", content, Color.green, NotificationIcon.Clock);

                    content = GameObject.Find("ProductName/ProductNameInput/Text").GetComponent<Text>().text + " ��ǰ�� ��������� ����Ǿ����ϴ�.";
                    NotificationManager.SendWithAppIcon(pn + 1000000, expDay - DateTime.Now, "������", content, Color.green, NotificationIcon.Clock);
                }

                dm.data.pdList[pn].memo = GameObject.Find("Memo/MemoInput/Text").GetComponent<Text>().text;
                Debug.Log("�޸� : " + GameObject.Find("Memo/MemoInput/Text").GetComponent<Text>().text);

                DataManager.SaveIngameData(dm.data);
                Debug.Log("���� �Ϸ�");
                savePop.SetActive(true);
            }
        }
    }
}
