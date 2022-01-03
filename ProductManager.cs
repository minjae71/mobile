using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using Assets.SimpleAndroidNotifications;

public class ProductManager : MonoBehaviour
{
    // ������ ���� ��ũ��Ʈ
    DataManager dm;

    GameObject noProductPop;

    //GameObject blur;
    Image productImage;
    Transform productTrans;

    GameObject setAreaPop;
    GameObject setStandardPop;
    GameObject detailPop;
    GameObject deletePop;
    GameObject resultPop;
    GameObject trashBtn;
    GameObject deleteAreaBtn;
    GameObject areaDeletePopup;
    GameObject deleteFailPop;
    GameObject fadePanel;
    Image fadePop;
    Text fadeTxt;

    public GameObject product;
    List<GameObject> productList = new List<GameObject>();

    List<ProductData> sortProductList = new List<ProductData>();

    bool isAll = true;
    int areaNum = -1;
    int sortType = 0;

    int count = 0;

    int selectNo = 0;

    bool isDeleted;

    string search = "";

    bool isEtcOn = false;

    List<GameObject> areaList = new List<GameObject>();
    bool areaDelete = false;

    // Start is called before the first frame update
    void Start()
    {
        dm = GameObject.Find("DataManager").GetComponent<DataManager>();

        noProductPop = GameObject.Find("NoProductPanel");
        noProductPop.SetActive(false);

        //blur = GameObject.Find("Blur");
        //blur.SetActive(false);

        setAreaPop = GameObject.Find("SetAreaPanel");
        SetArea();

        setStandardPop = GameObject.Find("SetStandardPanel");
        setStandardPop.SetActive(false);

        detailPop = GameObject.Find("ProductDetail");
        productImage = GameObject.Find("ProductDetail/Image/ProductImage").GetComponent<Image>();
        productTrans = GameObject.Find("ProductDetail/Image/ProductImage").GetComponent<RectTransform>();
        detailPop.SetActive(false);

        resultPop = GameObject.Find("ResultPopup");
        resultPop.SetActive(false);

        deletePop = GameObject.Find("DeletePopup");
        deletePop.SetActive(false);

        fadePanel = GameObject.Find("FadePanel");
        fadePop = GameObject.Find("FadePanel").GetComponent<Image>();
        fadeTxt = GameObject.Find("FadePanel/Text").GetComponent<Text>();
        fadePop.color = new Color(255, 255, 255, 0);
        fadeTxt.color = new Color(255, 255, 255, 0);
        fadePanel.SetActive(false);

        trashBtn = GameObject.Find("TrashBtn");
        trashBtn.SetActive(false);

        deleteAreaBtn = GameObject.Find("DeleteAreaBtn");
        deleteAreaBtn.SetActive(false);

        areaDeletePopup = GameObject.Find("AreaDeletePopup");
        areaDeletePopup.SetActive(false);

        deleteFailPop = GameObject.Find("DeleteFailPopup");
        deleteFailPop.SetActive(false);

        GetProduct();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            LoadingSceneManager.Instance.LoadScene("Home");
        }
    }

    public void SetArea()
    {
        setAreaPop.SetActive(true);
        for (int i = 0; i < dm.data.adList.Count; i++)
        {
            if (!dm.data.adList[i].isDeleted)
            {
                GameObject area = Instantiate(Resources.Load("Prefab/SortArea"), GameObject.Find("SetAreaPanel/AreaList/Viewport/Content").transform) as GameObject;
                int temp = i;
                area.name = temp.ToString();
                area.transform.Find("Text").GetComponent<Text>().text = dm.data.adList[i].areaName;
                area.GetComponent<Button>().onClick.AddListener(() => SetSortArea(temp));

                areaList.Add(area);
            }
        }
        setAreaPop.SetActive(false);
    }

    public void SortBtnClick(int no)
    {
        if(no==1)
        {
            setAreaPop.SetActive(true);
        }
        else
        {
            setStandardPop.SetActive(true);
        }
    }

    public void SortCloseBtnClick(int no)
    {
        if (no == 1)
        {
            setAreaPop.SetActive(false);
        }
        else
        {
            setStandardPop.SetActive(false);
        }
    }

    public void SetSortArea(int no)
    {
        if(no==-1)
        {
            GameObject.Find("Align/Area").GetComponent<Text>().text = "��ü ����";
            isAll = true;
        }
        else
        {
            GameObject.Find("Align/Area").GetComponent<Text>().text = dm.data.adList[no].areaName;
            isAll = false;
            areaNum = no;
        }
        setAreaPop.SetActive(false);
        for (int i = 0; i < productList.Count; i++)
        {
            Destroy(productList[i]);
        }
        GetProduct();
    }

    public void SetSortType(int no)
    {
        sortType = no;
        GameObject.Find("Align/Standard").GetComponent<Text>().text = GameObject.Find("SetStandardPanel/Sort" + no + "/Text").GetComponent<Text>().text;
        setStandardPop.SetActive(false);
        for (int i = 0; i < productList.Count; i++)
        {
            Destroy(productList[i]);
        }
        GetProduct();
    }

    public void SearchText()
    {
        for (int i = 0; i < productList.Count; i++)
        {
            Destroy(productList[i]);
        }
        search = GameObject.Find("Search").GetComponent<InputField>().text;
        GetProduct();
    }

    public void ProductSort(int type)
    {
        switch(type)
        {
            case 0:
                sortProductList.Sort(delegate (ProductData A, ProductData B)
                {
                    if (A.expDate > B.expDate) return 1;
                    else if (A.expDate < B.expDate) return -1;
                    return 0;
                });
                break;
            case 1:
                sortProductList.Sort(delegate (ProductData A, ProductData B)
                {
                    if (A.regDate > B.regDate) return 1;
                    else if (A.regDate < B.regDate) return -1;
                    return 0;
                });
                break;
            case 2:
                sortProductList.Sort(delegate (ProductData A, ProductData B)
                {
                    if (A.stock > B.stock) return 1;
                    else if (A.stock < B.stock) return -1;
                    return 0;
                });
                break;
            case 3:
                sortProductList.Sort(delegate (ProductData A, ProductData B)
                {
                    if (A.stock > B.stock) return -1;
                    else if (A.stock < B.stock) return 1;
                    return 0;
                });
                break;
        }
    }

    private static Texture2D LoadImage(Vector2 size, string filePath)
    {
        byte[] bytes = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGB24, false);
        texture.filterMode = FilterMode.Trilinear;
        texture.LoadImage(bytes);

        return texture;
    }

    public void GetProduct()
    {
        noProductPop.SetActive(false);
        count = 0;
        
        if(dm.data.pdList.Count!=0)
        {
            sortProductList = dm.data.pdList;
            ProductSort(sortType);
            for (int i = 0; i < sortProductList.Count; i++)
            {
                if (!sortProductList[i].isDeleted)
                {
                    if (dm.data.pdList[i].productName.Contains(search))
                    {
                        if (!isAll)
                        {
                            if (sortProductList[i].areaNo == areaNum)
                            {
                                SetProduct(i);
                            }
                        }
                        else
                        {
                            SetProduct(i);
                        }
                    }
                }
            }
        }
        GameObject.Find("Count/Text").GetComponent<Text>().text = count + "";
        if (count == 0)
        {
            noProductPop.SetActive(true);
        }
    }

    public void SetProduct(int i)
    {
        string tempNow = DateTime.Now.ToString("MM/dd/yyyy");
        DateTime now = Convert.ToDateTime(tempNow);

        count++;
        product = Instantiate(Resources.Load("Prefab/Product"),
            GameObject.Find("PdList/Viewport/Content").transform) as GameObject;

        Image pdImg;
        Transform pdTrans;
        string imagePath = sortProductList[i].picture;

        DateTime regdate = sortProductList[i].regDate;
        DateTime expdate = sortProductList[i].expDate;
        float subdate;

        product.name = i + "";
        int temp = i;

        pdImg = product.transform.Find("Image/ProductImage").GetComponent<Image>();
        pdTrans = product.transform.Find("Image/ProductImage").GetComponent<RectTransform>();

        if (imagePath != "none")
        {
            NativeGallery.ImageProperties prop = NativeGallery.GetImageProperties(imagePath);
            Vector2 imageSize = new Vector2(prop.width, prop.height);
            Texture2D text = LoadImage(imageSize, imagePath);
            //Texture2D rotateTex;
            string cameraPath = "/storage/emulated/0/DCIM/Camera/";
            string rot = "_rotateUtongE";
            if (imagePath.Contains(cameraPath) && prop.width <= prop.height)
            {
                //rotateTex = RotateTexture(text, true);
                //text = rotateTex;
                if (imagePath.Contains(rot))
                {
                    pdTrans.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    pdTrans.rotation = Quaternion.Euler(0, 0, 270);
                }
            }

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
            pdImg.sprite = Sprite.Create(text, new Rect(spaceW, spaceH, width, height), new Vector2(0, 0));
        }
        else
        {
            pdImg.sprite = Resources.Load("Image/Locker/imageBox", typeof(Sprite)) as Sprite;
        }
        product.transform.Find("Name").GetComponent<Text>().text = sortProductList[i].productName + " / " + dm.data.adList[sortProductList[i].areaNo].areaName;
        product.GetComponent<Button>().onClick.AddListener(() => ProductClick(temp));
        product.transform.Find("RegDate").GetComponent<Text>().text = regdate.ToString("yyyy / MM / dd ���");
        product.transform.Find("StockTxt").GetComponent<Text>().text = sortProductList[i].stock + "";
        product.transform.Find("StockMinusBtn").GetComponent<Button>().onClick.AddListener(() => StockMinusBtnClick(temp));
        product.transform.Find("StockPlusBtn").GetComponent<Button>().onClick.AddListener(() => StockPlusBtnClick(temp));
        if (sortProductList[i].stock <= 0)
        {
            product.transform.Find("StockMinusBtn").GetComponent<Button>().interactable = false;
            product.transform.Find("StockPlusBtn").GetComponent<Button>().interactable = false;
        }
        product.transform.Find("ExpDate").GetComponent<Text>().text = expdate.ToString("yyyy / MM / dd ����");
        if (sortProductList[i].expDate < DateTime.Now)
        {
            product.transform.Find("Slider/Fill").GetComponent<Image>().fillAmount = 0;
            product.transform.Find("ExpDate").GetComponent<Text>().text = "<color=#CF4444>" + expdate.ToString("yyyy / MM / dd �����") + "</color>";
            product.transform.Find("Slider/Fill/Handle/Text").GetComponent<Text>().text = "����";
        }
        else
        {
            subdate = (float)(expdate.Day - now.Day) / (expdate.Day - regdate.Day);
            subdate = (float)Math.Round(subdate, 1);
            if (sortProductList[i].isDisplayMinus)
            {
                product.transform.Find("DTxt").GetComponent<Text>().text = "D-" + (expdate.Day - now.Day);
                product.transform.Find("Slider/Fill/Handle/Text").GetComponent<Text>().text = (expdate.Day - now.Day) + "��";
                product.transform.Find("Slider/Fill").GetComponent<Image>().fillAmount = subdate;
                product.transform.Find("Slider/Fill/Handle").GetComponent<RectTransform>().anchoredPosition = new Vector2(subdate * 350, 0);

            }
            else
            {
                product.transform.Find("DTxt").GetComponent<Text>().text = "D+" + (now.Day - regdate.Day);
                product.transform.Find("Slider/Fill/Handle/Text").GetComponent<Text>().text = (now.Day - regdate.Day) + "��";
                product.transform.Find("Slider/Fill").GetComponent<Image>().fillAmount = 1.0f - subdate;
                product.transform.Find("Slider/Fill/Handle").GetComponent<RectTransform>().anchoredPosition = new Vector2((1.0f - subdate) * 350, 0);
            }
        }
        productList.Add(product);
    }

    public void ProductClick(int no)
    {
        productTrans.rotation = Quaternion.Euler(0, 0, 0);
        selectNo = no;
        Debug.Log(dm.data.pdList[no] + "��° ��ǰ Ŭ��");
        Debug.Log(no + " no");
        Debug.Log(selectNo + " selectNo");
        DateTime regD = dm.data.pdList[no].regDate;
        DateTime expD = dm.data.pdList[no].expDate;
        int diffD;

        string tempNow = DateTime.Now.ToString("MM/dd/yyyy");
        DateTime now = Convert.ToDateTime(tempNow);

        detailPop.SetActive(true);

        GameObject.Find("ProductDetail/Update").GetComponent<Button>().interactable = true;
        GameObject.Find("ProductDetail/StockMinus").GetComponent<Button>().interactable = true;

        if (dm.data.pdList[no].picture!="none")
        {
            string imagePath = dm.data.pdList[no].picture;
            NativeGallery.ImageProperties prop = NativeGallery.GetImageProperties(imagePath);
            Vector2 imageSize = new Vector2(prop.width, prop.height);
            Texture2D text = LoadImage(imageSize, imagePath);
            //Texture2D rotateTex;
            string cameraPath = "/storage/emulated/0/DCIM/Camera/";
            string rot = "_rotateUtongE";
            if (imagePath.Contains(cameraPath) && prop.width <= prop.height)
            {
                //rotateTex = RotateTexture(text, true);
                //text = rotateTex;
                if(!imagePath.Contains(rot))
                {
                    productTrans.rotation = Quaternion.Euler(0, 0, 270);
                }
            }

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
        else
        {
            productImage.sprite = Resources.Load("Image/Locker/imageBox", typeof(Sprite)) as Sprite;
        }
        GameObject.Find("ProductDetail/ProductName").GetComponent<Text>().text = dm.data.pdList[no].productName;
        GameObject.Find("ProductDetail/Area").GetComponent<Text>().text = "���� : " + dm.data.adList[dm.data.pdList[no].areaNo].areaName;
        GameObject.Find("ProductDetail/Expdate").GetComponent<Text>().text = "���/������� : " + dm.data.pdList[no].expDate.ToString("yyyy�� MM�� dd�ϱ���");

        if (sortProductList[no].isDisplayMinus)
        {
            diffD = (expD - now).Days;
            GameObject.Find("ProductDetail/DDay").GetComponent<Text>().text = "������� D-" + diffD;
        }
        else
        { 
            diffD = (now - regD).Days;
            GameObject.Find("ProductDetail/DDay").GetComponent<Text>().text = "��ǰ ��� D+" + diffD;
        }

        GameObject.Find("ProductDetail/Stock").GetComponent<Text>().text = "���� ��� : " + dm.data.pdList[no].stock + "��";

        if(dm.data.pdList[no].isSetAlarm)
        {
            GameObject.Find("ProductDetail/Alarm").GetComponent<Text>().text = "�˸� ���� : O";
        }
        else
        {
            GameObject.Find("ProductDetail/Alarm").GetComponent<Text>().text = "�˸� ���� : X";
        }

        GameObject.Find("ProductDetail/StockMinus").GetComponent<Button>().onClick.AddListener(() => StockMinus());
        if(dm.data.pdList[no].stock<=0)
        {
            GameObject.Find("ProductDetail/StockMinus").GetComponent<Button>().interactable = false;
        }

        GameObject.Find("ProductDetail/Update").GetComponent<Button>().onClick.AddListener(() => ProductUpdateButtonClick());
        if(DateTime.Now>expD)
        {
            GameObject.Find("ProductDetail/Update").GetComponent<Button>().interactable = false;
        }

        GameObject.Find("ProductDetail/Delete").GetComponent<Button>().onClick.AddListener(() => ProductDeleteButtonClick());
    }

    public void StockMinusBtnClick(int no)
    {
        selectNo = no;
        Debug.Log(selectNo + "��° ��ǰ ��� ���� ��ư Ŭ��");
        if (dm.data.pdList[no].stock > 1)
        {
            dm.data.pdList[no].stock--;
            DataManager.SaveIngameData(dm.data);
            GameObject.Find("PdList/Viewport/Content/" + no + "/StockMinusBtn").GetComponent<Button>().interactable = true;
            SetPopup(1);
            GameObject.Find("PdList/Viewport/Content/" + no + "/StockTxt").GetComponent<Text>().text = dm.data.pdList[no].stock + "";
        }
        else
        {
            dm.data.pdList[no].stock--;
            DataManager.SaveIngameData(dm.data);
            
            GameObject.Find("PdList/Viewport/Content/" + no + "/StockMinusBtn").GetComponent<Button>().interactable = false;
            GameObject.Find("PdList/Viewport/Content/" + no + "/StockPlusBtn").GetComponent<Button>().interactable = false;
            SetPopup(3);
            GameObject.Find("PdList/Viewport/Content/" + no + "/StockTxt").GetComponent<Text>().text = dm.data.pdList[no].stock + "";
        }
    }

    public void StockPlusBtnClick(int no)
    {
        selectNo = no;
        Debug.Log(selectNo + "��° ��ǰ ��� ���� ��ư Ŭ��");
        if (dm.data.pdList[no].stock >= 99)
        {
            GameObject.Find("PdList/Viewport/Content/" + no + "/StockPlusBtn").GetComponent<Button>().interactable = false;
            SetPopup(5);
            GameObject.Find("PdList/Viewport/Content/" + no + "/StockTxt").GetComponent<Text>().text = dm.data.pdList[no].stock + "";
        }
        else
        {
            dm.data.pdList[no].stock++;
            DataManager.SaveIngameData(dm.data);
            GameObject.Find("PdList/Viewport/Content/" + no + "/StockPlusBtn").GetComponent<Button>().interactable = true;
            SetPopup(4);
            GameObject.Find("PdList/Viewport/Content/" + no + "/StockTxt").GetComponent<Text>().text = dm.data.pdList[no].stock + "";
        }
    }

    public void CancelButtonClick()
    {
        detailPop.SetActive(false);
        //blur.SetActive(false);
    }

    public void StockMinus()
    {
        Debug.Log(selectNo + "��° ��ǰ ��� ���� ��ư Ŭ��");
        if (dm.data.pdList[selectNo].stock>1)
        {
            dm.data.pdList[selectNo].stock--;
            DataManager.SaveIngameData(dm.data);
            GameObject.Find("ProductDetail/Stock").GetComponent<Text>().text = "���� ��� : " + dm.data.pdList[selectNo].stock + "��";
            SetPopup(1);
        }
        else
        {
            dm.data.pdList[selectNo].stock--;
            DataManager.SaveIngameData(dm.data);
            GameObject.Find("ProductDetail/StockMinus").GetComponent<Button>().interactable=false;
            GameObject.Find("ProductDetail/Stock").GetComponent<Text>().text = "���� ��� : " + dm.data.pdList[selectNo].stock + "��";
            SetPopup(3);
        }
    }

    public void ProductUpdateButtonClick()
    {
        dm.data.updateProductNo = selectNo;
        SceneManager.LoadScene("UpdateProduct");
    }

    public void ProductDeleteButtonClick()
    {
        Debug.Log(selectNo + "��° ��ǰ ���� ��ư Ŭ��");
        deletePop.SetActive(true);
    }

    public void SetPopup(int no)
    {
        
        GameObject.Find("PdList/Viewport/Content/" + selectNo + "/StockTxt").GetComponent<Text>().text = dm.data.pdList[selectNo].stock + "";
        switch (no)
        {
            case 1:
                FadeControll(true);
                Debug.Log(selectNo + "��° ��ǰ ��� ����");
                isDeleted = false;
                break;
            case 2:
                resultPop.SetActive(true);
                GameObject.Find("ResultPopup/ResultTxt").GetComponent<Text>().text = "��ǰ�� �����Ǿ����ϴ�.";
                Debug.Log(selectNo + "��° ��ǰ ����");
                isDeleted = true;
                break;
            case 3:
                resultPop.SetActive(true);
                GameObject.Find("ResultPopup/ResultTxt").GetComponent<Text>().text = "��� ��� �����Ͽ����ϴ�!";
                dm.data.safe++;
                isDeleted = true;
                Debug.Log(dm.data.pdList[selectNo].stock);
                break;
            case 4:
                FadeControll(false);
                Debug.Log(selectNo + "��° ��ǰ ��� ����");
                isDeleted = false;
                break;
            case 5:
                resultPop.SetActive(true);
                GameObject.Find("ResultPopup/ResultTxt").GetComponent<Text>().text = "���� 99�� �̻� ����� �� �����ϴ�.";
                Debug.Log(selectNo + "��° ��ǰ ��� ����");
                isDeleted = false;
                break;
        }
    }

    public void PopupOkButtonClick()
    {
        resultPop.SetActive(false);
        if(isDeleted)
        {
            if(dm.data.pdList[selectNo].stock<=0)
            {
                Debug.Log("if�� 1 ���");
                dm.data.pdList[selectNo].isSpend = true;
                if (dm.data.isCompleteProductDelete)
                {
                    Debug.Log("if�� 2 ���");
                    dm.data.pdList[selectNo].isDeleted = true;
                }
            }
            else
            {
                Debug.Log("else�� 1 ���");
                dm.data.pdList[selectNo].isDeleted = true;
            }
            //blur.SetActive(false);
            DataManager.SaveIngameData(dm.data);
            detailPop.SetActive(false);
            for(int i=0;i<productList.Count;i++)
            {
                Destroy(productList[i]);
            }
            Debug.Log(selectNo + "��° ��ǰ ���� �Ϸ�");
            GetProduct();
        }
        else
        {
            detailPop.SetActive(false);
        }
    }

    public void CheckDelete(bool del)
    {
        if (del)
        {
            SetPopup(2);
            Debug.Log(selectNo + "��° ��ǰ ����2");
        }
        deletePop.SetActive(false);
    }

    public void ETCButtonClick()
    {
        if(isEtcOn)
        {
            isEtcOn = false;
            trashBtn.SetActive(false);
            deleteAreaBtn.SetActive(false);
        }
        else
        {
            isEtcOn = true;
            trashBtn.SetActive(true);
            deleteAreaBtn.SetActive(true);
        }
    }

    public void TrashButtonClick()
    {
        LoadingSceneManager.Instance.LoadScene("Trash");
    }

    public void DeleteAreaButtonClick()
    {
        int areaCount = 0;
        for(int i = 0; i < dm.data.adList.Count; i++)
        {
            if(!dm.data.adList[i].isDeleted)
            {
                areaCount++;
            }
        }
        if(areaCount<=1)
        {
            ErrorAreaDeletePopup(5);
            //������ 1�� ����
        }
        else
        {
            if (dm.data.pdList.Count != 0)
            {
                if (areaNum != -1)
                {
                    int pdCount = 0;
                    for (int i = 0; i < dm.data.pdList.Count; i++)
                    {
                        if (!dm.data.pdList[i].isDeleted)
                        {
                            if (dm.data.pdList[i].areaNo == areaNum)
                            {
                                pdCount++;
                            }
                        }
                    }
                    if (pdCount == 0)
                    {
                        //���� ���� ����
                        areaDeletePopup.SetActive(true);
                        GameObject.Find("AreaDeletePopup/ErrorTxt").GetComponent<Text>().text = dm.data.adList[areaNum].areaName + " ������ �����Ͻðڽ��ϱ�?";
                    }
                    else
                    {
                        ErrorAreaDeletePopup(1);
                        //��ǰ ����
                    }
                }
                else
                {
                    ErrorAreaDeletePopup(2);
                    //���� ���� ����
                }
            }
            else
            {
                ErrorAreaDeletePopup(3);
                //��ǰ�� ����
            }
        }
        
    }
    public void CheckDeleteArea(bool del)
    {
        if(del)
        {
            dm.data.adList[areaNum].isDeleted = true;
            DataManager.SaveIngameData(dm.data);
            ErrorAreaDeletePopup(4);
        }
        areaDeletePopup.SetActive(false);
    }

    public void ErrorAreaDeletePopup(int no)
    {
        deleteFailPop.SetActive(true);
        switch(no)
        {
            case 1:
                GameObject.Find("DeleteFailPopup/ResultTxt").GetComponent<Text>().text = "�ش� ������ ��ǰ�� ���� �ֽ��ϴ�!";
                areaDelete = false;
                break;
            case 2:
                GameObject.Find("DeleteFailPopup/ResultTxt").GetComponent<Text>().text = "��ü ������ ���õǾ� �ֽ��ϴ�!";
                areaDelete = false;
                break;
            case 3:
                GameObject.Find("DeleteFailPopup/ResultTxt").GetComponent<Text>().text = "��ǰ�� �� �� �̻� ����� �ּ���!";
                areaDelete = false;
                break;
            case 4:
                GameObject.Find("DeleteFailPopup/ResultTxt").GetComponent<Text>().text = "������ �����Ǿ����ϴ�.";
                areaDelete = true;
                break;
            case 5:
                GameObject.Find("DeleteFailPopup/ResultTxt").GetComponent<Text>().text = "������ �ּ� �ϳ� �̻� �����־�� �մϴ�.";
                areaDelete = false;
                break;
        }
    }

    public void ErrorAreaDeleteOkButtonClick()
    {
        deleteFailPop.SetActive(false);
        if(areaDelete)
        {
            for (int i = 0; i < areaList.Count; i++)
            {
                Destroy(areaList[i]);
            }
            SetSortArea(-1);
            SetArea();
            for (int i = 0; i < productList.Count; i++)
            {
                Destroy(productList[i]);
            }
            GetProduct();
        }
    }

    public void FadeControll(bool del)
    {
        fadePanel.SetActive(true);
        string pn = dm.data.pdList[selectNo].productName;
        if(del)
        {
            GameObject.Find("FadePanel/Text").GetComponent<Text>().text = pn + "��ǰ�� ��� �����Ͽ����ϴ�.";
        } else
        {
            GameObject.Find("FadePanel/Text").GetComponent<Text>().text = pn + "��ǰ�� ��� �߰��Ǿ����ϴ�.";
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
}
