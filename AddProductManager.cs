using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using Assets.SimpleAndroidNotifications;

public class AddProductManager : MonoBehaviour
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
    bool isAlarm = true;
    string setAlarmTxt = "";

    //int imgChk = 0;
    bool areaChk = false; // 구역을 설정했는지 확인하는 변수
    bool priceChk = false; // 가격을 입력했는지 확인하는 변수

    int price = 0;
    int addAreaNum = 0;
    int areaCount = 0;
    int areaNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        dm = GameObject.Find("DataManager").GetComponent<DataManager>();
        areaCount = dm.data.adList.Count;

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

        GameObject.Find("Background/Scroll/Viewport/Content/ExpiryDate/Date/Text_Select_Date").GetComponent<Text>().text = DateTime.Now.ToString("yyyy/MM/dd");

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
        errorPop.SetActive(false);
        savePop.SetActive(false);
        fadePanel.SetActive(false);

        stock = int.Parse(GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text);
    }

    void Update()
    {
        // 뒤로가기 버튼을 두번 누를 시 MainScene으로 돌아간다.
        if (Input.GetKey(KeyCode.Escape))
        {
            LoadingSceneManager.Instance.LoadScene("Home");
        }
    }

    public void DisplayBtnClick(bool minus)
    {
        if(minus)
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
        //blur.SetActive(true);
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
        imgPop.SetActive(false);
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
        imgPop.SetActive(false);
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

        string newPath = "/storage/emulated/0/DCIM/Camera/" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + rot +".jpg";
        StartCoroutine(SaveJpg(newPath, text));
        
        imagePath = newPath;

        int width = text.width;
        int height = text.height;
        int spaceW = 0;
        int spaceH = 0;
        if(text.width > text.height)
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
        if (expAMPM.text == "오후")
        {
            expAMPM.text = "오전";
        }
        else
        {
            expAMPM.text = "오후";
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

        if(stock<=1)
        {
            stock = 1;
        } else
        {
            --stock;
        }

        GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text = stock.ToString();
    }

    public void StockPlusBtnClick()
    {
        stock = int.Parse(GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text);

        if(stock>=99)
        {
            stock = 99;
        } else
        {
            ++stock;
        }

        GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text = stock.ToString();
    }

    public void StockBtnClick()
    {
        stock = int.Parse(GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text);
        //blur.SetActive(true);
        stockPop.SetActive(true);

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
        //blur.SetActive(true);
        areaPop.SetActive(true);
        if(addAreaNum!=0)
        {
            for (int i = areaCount; i < areaCount + addAreaNum; i++)
            {
                if(!dm.data.adList[i].isDeleted)
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
        //blur.SetActive(false);
        areaPop.SetActive(false);
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
        addAreaPop.SetActive(false);
        areaPop.SetActive(false);
        //blur.SetActive(false);
    }

    public void PriceBtnClick()
    {
        //blur.SetActive(true);
        if (priceChk)
        {
            price = int.Parse(GameObject.Find("Price/PriceBtn/Text").GetComponent<Text>().text);
        }
        pricePop.SetActive(true);

        if(priceChk==true)
        {
            GameObject.Find("PricePopup/PriceInput/Placeholder").GetComponent<Text>().text = GameObject.Find("Price/PriceBtn/Text").GetComponent<Text>().text;
            GameObject.Find("PricePopup/PriceInput/Text").GetComponent<Text>().text = GameObject.Find("Price/PriceBtn/Text").GetComponent<Text>().text;
        }
    }

    public void AlarmOnOff()
    {
        if(isAlarm)
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

        GameObject.Find("SetAlarm/SetAlarmBtn/Text").GetComponent<Text>().text = day + "일 전";
        alarmClockPop.SetActive(true);
        setAlarmPop.SetActive(false);
    }

    public void AlarmAMPMBtnClick()
    {
        if (alarmAMPM.text == "오후")
        {
            alarmAMPM.text = "오전";
        }
        else
        {
            alarmAMPM.text = "오후";
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
                if(price==0)
                {
                    GameObject.Find("PricePopup/PriceInput/Text").GetComponent<Text>().text = "0";
                }
                GameObject.Find("Price/PriceBtn/Text").GetComponent<Text>().text = GameObject.Find("PricePopup/PriceInput/Text").GetComponent<Text>().text;
                priceChk = true;
                pricePop.SetActive(false);
                break;
            case 6:
                setAlarmTxt = GameObject.Find("AlarmInput/Text").GetComponent<Text>().text;
                if (setAlarmTxt!="")
                {
                    setAlarm = int.Parse(setAlarmTxt);
                    alarmClockPop.SetActive(true);
                }
                setAlarmPop.SetActive(false);
                break;
            case 7:
                alarmHourTxt = GameObject.Find("AlarmClockPopup/Clock/Hour").GetComponent<InputField>().text;
                alarmMinuteTxt = GameObject.Find("AlarmClockPopup/Clock/Minute").GetComponent<InputField>().text;
                GameObject.Find("SetAlarm/SetAlarmBtn/Text").GetComponent<Text>().text = setAlarm + "일 전 " + alarmAMPM.text + " " + alarmHourTxt + ":" + alarmMinuteTxt;
                alarmHour = int.Parse(alarmHourTxt);
                alarmMinute = int.Parse(alarmMinuteTxt);
                if (alarmAMPM.text == "오후")
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
                LoadingSceneManager.Instance.LoadScene("Locker");
                break;
            case 10:
                if(isSetExpTime)
                {
                    expHourTxt = GameObject.Find("ExpiryClockPopup/Clock/Hour").GetComponent<InputField>().text;
                    expMinuteTxt = GameObject.Find("ExpiryClockPopup/Clock/Minute").GetComponent<InputField>().text;
                    GameObject.Find("ExpiryDate/Date/Clock/Time").GetComponent<Text>().text = expAMPM.text + " " + expHourTxt + ":" + expMinuteTxt;
                }
                else
                {
                    expAMPM.text = "오후";
                    expHourTxt = "11";
                    expMinuteTxt = "59";
                    GameObject.Find("ExpiryDate/Date/Clock/Time").GetComponent<Text>().text = "오후 11:59";
                }
                expHour = int.Parse(expHourTxt);
                expMinute = int.Parse(expMinuteTxt);
                if (expAMPM.text == "오후")
                {
                    expHour += 12;
                }
                expClockPop.SetActive(false);
                break;
        }
    }

    public void FadeControll()
    {
        fadePanel.SetActive(true);
        if(isAlarm)
        {
            if(GameObject.Find("Background/Scroll/Viewport/Content/ExpiryDate/Date/Text_Select_Date").GetComponent<Text>().text != "YY/MM/DD (일)")
            {
                expDay = Convert.ToDateTime(GameObject.Find("Background/Scroll/Viewport/Content/ExpiryDate/Date/Text_Select_Date").GetComponent<Text>().text);
                alarmDay = Convert.ToDateTime(expDay.AddDays(0 - setAlarm));
                if (alarmAMPM.text == "오전" && alarmHour == 12)
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
                if(alarmDay < DateTime.Now)
                {
                    GameObject.Find("FadePanel/Text").GetComponent<Text>().text = "알람 설정 시간을 확인해 주세요!";
                }
                else
                {
                    if (setTime.Days >= 1)
                    {
                        GameObject.Find("FadePanel/Text").GetComponent<Text>().text = setTime.Days + "일 " + setTime.Hours + "시간 " + setTime.Minutes + "분 뒤에 알림 설정";
                    }
                    else
                    {
                        GameObject.Find("FadePanel/Text").GetComponent<Text>().text = setTime.Hours + "시간 " + setTime.Minutes + "분 뒤에 알림 설정";
                    }
                }
            }
            else
            {
                GameObject.Find("FadePanel/Text").GetComponent<Text>().text = "유통 기한을 설정해 주세요.";
            }
        }
        else
        {
            GameObject.Find("FadePanel/Text").GetComponent<Text>().text = "알림 설정이 해제되었습니다.";
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
        while(fadeCount < 1.0f)
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

    public void SaveBtnClick()
    {
        bool checkSave = true;
        //blur.SetActive(true);
        errorPop.SetActive(true);
        if (GameObject.Find("ProductName/ProductNameInput/Text").GetComponent<Text>().text == "")
        {
            GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "제품명을 입력해 주세요.";
            checkSave = false;
        }
        else if (GameObject.Find("Background/Scroll/Viewport/Content/ExpiryDate/Date/Text_Select_Date").GetComponent<Text>().text == "YY/MM/DD (일)")
        {
            GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "유통 기한을 설정해 주세요.";
            checkSave = false;
        }
        else if (areaChk == false)
        {
            GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "구역을 설정해 주세요.";
            checkSave = false;
        }
        else if (priceChk == false)
        {
            GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "가격을 입력해 주세요.";
            checkSave = false;
        }
        else
        {
            expDay = Convert.ToDateTime(GameObject.Find("Background/Scroll/Viewport/Content/ExpiryDate/Date/Text_Select_Date").GetComponent<Text>().text);
            if (isSetExpTime)
            {
                if(expAMPM.text == "오전" && expHour == 12)
                {
                    expHour = 0;
                }
                if(expHour == 24)
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
            if(alarmAMPM.text == "오전" && alarmHour == 12)
            {
                alarmHour = 0;
            }
            if(alarmHour == 24)
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
            if(dm.data.isSilent)
            {
                if(isAlarm)
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
                GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "유통 기한은 현재 시각 이전으로 설정할 수 없습니다!";
                checkSave = false;
            }
            else if (expDay < alarmDay)
            {
                if (isAlarm)
                {
                    GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "알람 설정 시간은 유통기한 이후로 설정할 수 없습니다!";
                    checkSave = false;
                }
            }
            else if (alarmDay < DateTime.Now)
            {
                if (isAlarm)
                {
                    GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "알람 설정 시간은 현재 시각 이후로 설정해 주세요!";
                    checkSave = false;
                }
            }
            else if(!silentChk)
            {
                if(isAlarm)
                {
                    GameObject.Find("ErrorPopup/ErrorTxt").GetComponent<Text>().text = "알람 설정 시간이 방해금지 시간대에 포함되어 있습니다!\n" +
                    "방해금지 시간 = " + silentStr;
                    checkSave = false;
                }
            }

            if(checkSave)
            {
                errorPop.SetActive(false);
                string content = "";
                int productNum = 0;
                ProductData np = new ProductData(productNum, "", "", true, DateTime.MaxValue, 0, 0, 0, true, DateTime.MaxValue, false, "", false);
                if (dm.data.pdList.Count != 0)
                {
                    productNum = dm.data.pdList.Count;
                    np.productNo = productNum;
                }
                content = GameObject.Find("ProductName/ProductNameInput/Text").GetComponent<Text>().text;
                np.productName = content;
                Debug.Log("제품명 : " + content);

                Debug.Log(isSetExpTime + " " + expHour + " " + expMinute);

                np.picture = imagePath;
                //np.isTakePicture = isCamera;

                np.isDisplayMinus = isMinus;

                np.expDate = expDay;
                Debug.Log("유통기한 : " + expDay + "까지");

                np.stock = int.Parse(GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text);
                Debug.Log("재고 : " + GameObject.Find("Stock/StockBtn/Text").GetComponent<Text>().text);

                np.areaNo = areaNum;
                Debug.Log("구역 : " + GameObject.Find("Area/AreaBtn/Text").GetComponent<Text>().text);

                np.price = int.Parse(GameObject.Find("Price/PriceBtn/Text").GetComponent<Text>().text);
                Debug.Log("가격 : " + GameObject.Find("Price/PriceBtn/Text").GetComponent<Text>().text);

                if (!isAlarm)
                {
                    np.isSetAlarm = false;
                    Debug.Log("알람 설정 : X");
                }
                else
                {
                    np.isSetAlarm = true;
                    Debug.Log("알람 설정 : O");

                    np.alarmTime = alarmDay;
                    Debug.Log("알림 : " + alarmDay);

                    DateTime timeToNotify = alarmDay;
                    TimeSpan time = timeToNotify - DateTime.Now;
                    TimeSpan diffTime = expDay - alarmDay;

                    if (diffTime.Days > 1)
                    {
                        content += " 상품의 유통기한이 " + diffTime.Days + "일 남았습니다.";
                    }
                    else
                    {
                        content += " 상품의 유통기한이 내일 만료됩니다.";
                    }

                    NotificationManager.SendWithAppIcon(productNum, time, "유통이", content, Color.green, NotificationIcon.Clock);

                    content = GameObject.Find("ProductName/ProductNameInput/Text").GetComponent<Text>().text + " 상품의 유통기한이 만료되었습니다.";
                    NotificationManager.SendWithAppIcon(productNum+1000000, expDay-DateTime.Now, "유통이", content, Color.green, NotificationIcon.Clock);
                }

                np.memo = GameObject.Find("Memo/MemoInput/Text").GetComponent<Text>().text;
                Debug.Log("메모 : " + GameObject.Find("Memo/MemoInput/Text").GetComponent<Text>().text);

                RegistNewProduct(np);

                savePop.SetActive(true);
            }
        }
    }

    public void RegistNewProduct(ProductData np)
    {
        dm.data.pdList.Add(np);
        DataManager.SaveIngameData(dm.data);
    }
}
