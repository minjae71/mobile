using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class TrashManager : MonoBehaviour
{
    // 보관함 관리 스크립트
    DataManager dm;

    GameObject noProductPop;

    GameObject setAreaPop;
    GameObject detailPop;
    GameObject restorePop;
    GameObject resultPop;

    public GameObject product;
    List<GameObject> productList = new List<GameObject>();

    List<ProductData> sortProductList = new List<ProductData>();

    bool isAll = true;
    int areaNum = -1;

    int count = 0;

    int selectNo = 0;

    bool isDeleted;

    string search = "";
    // Start is called before the first frame update
    void Start()
    {
        dm = GameObject.Find("DataManager").GetComponent<DataManager>();

        noProductPop = GameObject.Find("NoProductPanel");
        noProductPop.SetActive(false);

        setAreaPop = GameObject.Find("SetAreaPanel");
        for (int i = 0; i < dm.data.adList.Count; i++)
        {
            if (!dm.data.adList[i].isDeleted)
            {
                GameObject area = Instantiate(Resources.Load("Prefab/SortArea"), GameObject.Find("SetAreaPanel/AreaList/Viewport/Content").transform) as GameObject;
                int temp = i;
                area.name = temp.ToString();
                area.transform.Find("Text").GetComponent<Text>().text = dm.data.adList[i].areaName;
                area.GetComponent<Button>().onClick.AddListener(() => SetSortArea(temp));
            }
        }
        setAreaPop.SetActive(false);

        resultPop = GameObject.Find("ResultPopup");
        resultPop.SetActive(false);

        restorePop = GameObject.Find("RestorePopup");
        restorePop.SetActive(false);

        GetProduct();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            LoadingSceneManager.Instance.LoadScene("Locker");
        }
    }

    public void SortBtnClick()
    {
        setAreaPop.SetActive(true);
    }

    public void SortCloseBtnClick()
    {
        setAreaPop.SetActive(false);
    }

    public void SetSortArea(int no)
    {
        if (no == -1)
        {
            GameObject.Find("Align/Area").GetComponent<Text>().text = "전체 구역";
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

    public void SearchText()
    {
        for (int i = 0; i < productList.Count; i++)
        {
            Destroy(productList[i]);
        }
        search = GameObject.Find("Search").GetComponent<InputField>().text;
        GetProduct();
    }

    public void ProductSort()
    {
        sortProductList.Sort(delegate (ProductData A, ProductData B)
        {
            if (A.expDate > B.expDate) return 1;
            else if (A.expDate < B.expDate) return -1;
            return 0;
        });
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

        if (dm.data.pdList.Count != 0)
        {
            sortProductList = dm.data.pdList;
            ProductSort();
            for (int i = 0; i < sortProductList.Count; i++)
            {
                if (sortProductList[i].isDeleted)
                {
                    Debug.Log(i + " 1");
                    if (!dm.data.adList[dm.data.pdList[i].areaNo].isDeleted)
                    {
                        Debug.Log(i + " 1");
                        if (dm.data.pdList[i].productName.Contains(search))
                        {
                            Debug.Log(i + " 1");
                            if (!isAll)
                            {
                                Debug.Log(i + " 4");
                                if (sortProductList[i].areaNo == areaNum)
                                {
                                    Debug.Log(i + " 5");
                                    SetProduct(i);
                                }
                            }
                            else
                            {
                                Debug.Log(i + " 6");
                                SetProduct(i);
                            }
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
        product = Instantiate(Resources.Load("Prefab/DeletedProduct"),
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
        product.transform.Find("Name").GetComponent<Text>().text = sortProductList[i].productName + " (" + sortProductList[i].stock + ") / " + dm.data.adList[sortProductList[i].areaNo].areaName;
        product.transform.Find("RegDate").GetComponent<Text>().text = regdate.ToString("yyyy / MM / dd 등록");
        product.transform.Find("RestoreBtn").GetComponent<Button>().onClick.AddListener(() => RestoreBtnClick(temp));
        if (sortProductList[i].stock <= 0)
        {
            product.transform.Find("RestoreBtn").GetComponent<Button>().interactable = false;
        }
        product.transform.Find("ExpDate").GetComponent<Text>().text = expdate.ToString("yyyy / MM / dd 만료");
        if (sortProductList[i].expDate < DateTime.Now)
        {
            product.transform.Find("Slider/Fill").GetComponent<Image>().fillAmount = 0;
            product.transform.Find("ExpDate").GetComponent<Text>().text = "<color=#CF4444>" + expdate.ToString("yyyy / MM / dd 만료됨") + "</color>";
            product.transform.Find("Slider/Fill/Handle/Text").GetComponent<Text>().text = "만료";
            product.transform.Find("RestoreBtn").GetComponent<Button>().interactable = false;
        }
        else
        {
            subdate = (float)(expdate.Day - now.Day) / (expdate.Day - regdate.Day);
            subdate = (float)Math.Round(subdate, 1);
            if (sortProductList[i].isDisplayMinus)
            {
                product.transform.Find("DTxt").GetComponent<Text>().text = "D-" + (expdate.Day - now.Day);
                product.transform.Find("Slider/Fill/Handle/Text").GetComponent<Text>().text = (expdate.Day - now.Day) + "일";
                product.transform.Find("Slider/Fill").GetComponent<Image>().fillAmount = subdate;
                product.transform.Find("Slider/Fill/Handle").GetComponent<RectTransform>().anchoredPosition = new Vector2(subdate * 350, 0);

            }
            else
            {
                product.transform.Find("DTxt").GetComponent<Text>().text = "D+" + (now.Day - regdate.Day);
                product.transform.Find("Slider/Fill/Handle/Text").GetComponent<Text>().text = (now.Day - regdate.Day) + "일";
                product.transform.Find("Slider/Fill").GetComponent<Image>().fillAmount = 1.0f - subdate;
                product.transform.Find("Slider/Fill/Handle").GetComponent<RectTransform>().anchoredPosition = new Vector2(subdate * 350, 0);
            }
        }
        productList.Add(product);
    }

    public void RestoreBtnClick(int no)
    {
        selectNo = no;
        string name = dm.data.pdList[no].productName;
        restorePop.SetActive(true);
        GameObject.Find("RestorePopup/ErrorTxt").GetComponent<Text>().text = name + " 상품을 복구하시겠습니까?";
    }

    public void CheckDelete(bool del)
    {
        if (del)
        {
            SetPopup();
        }
        restorePop.SetActive(false);
    }

    public void SetPopup()
    {
        resultPop.SetActive(true);
        dm.data.pdList[selectNo].isDeleted = false;
        DataManager.SaveIngameData(dm.data);
        GameObject.Find("ResultPopup/ResultTxt").GetComponent<Text>().text = "상품이 복구되었습니다!";
    }

    public void PopupOkButtonClick()
    {
        resultPop.SetActive(false);
        for (int i = 0; i < productList.Count; i++)
        {
            Destroy(productList[i]);
        }
        GetProduct();
    }
}
