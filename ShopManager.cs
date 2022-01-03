using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class ShopManager : MonoBehaviour
{
    DataManager dm;

    public GameObject drawVideo;
    public VideoPlayer videoClip;

    Image colBtn;
    Image petBtn;
    Image cosBtn;
    Image bgBtn;
    Image nbgBtn;

    Image applyBg;
    Image applyCol;
    Image applyCos;
    Image applyNbg;
    Image applyPet;

    GameObject[] img;
    GameObject[] txt;
    GameObject[] btn;
    GameObject[] shadow;

    GameObject magImgPop;

    GameObject drawPop;
    GameObject selectPop;
    GameObject errorPop;
    GameObject drawing;
    GameObject resultPop;

    //GameObject blur;

    Image getImage;


    string[] colName = new string[12] { "�⺻", "����", "��Ʈ����", "�ٳ���", "����", "��纣��", "����", "�ڻ�����", "������", "Ŀ��", "����", "������" };
    string[] petName = new string[12] { "����", "�غ�", "�౸��", "����", "��ħ", "�Ž�", "�ܿ�", "���̵���", "����", "����", "ħ��", "�б�" };
    string[] cosName = new string[12] { "����", "���۶�", "�౸��", "������", "���ϸӸ���", "�峭������", "�絹��", "��Ʈ������", "����", "�߱׷�", "���Ϲ��", "�Ȱ�" };
    string[] bgName = new string[12] { "����", "�غ�", "�౸��", "����", "��ħ", "�Ž�", "�ܿ�", "���̵���", "����", "����", "ħ��", "�б�" };
    string[] nbgName = new string[12] { "����", "�غ�", "�౸��", "����", "��ħ", "�Ž�", "�ܿ�", "���̵���", "����", "����", "ħ��", "�б�" };

    // ���� ������ ��ư
    int selectType;

    int havePoint = 0;

    int nowDraw = 0;

    // Start is called before the first frame update
    void Start()
    {
        dm = GameObject.Find("DataManager").GetComponent<DataManager>();

        //6EB247 ���� �ȵ� 91C370 ���õ�
        colBtn = GameObject.Find("Inventory/SelectPanel/Color").GetComponent<Image>();
        petBtn = GameObject.Find("Inventory/SelectPanel/Pet").GetComponent<Image>();
        cosBtn = GameObject.Find("Inventory/SelectPanel/Costume").GetComponent<Image>();
        bgBtn = GameObject.Find("Inventory/SelectPanel/Background").GetComponent<Image>();
        nbgBtn = GameObject.Find("Inventory/SelectPanel/NameBackground").GetComponent<Image>();

        applyBg = GameObject.Find("ApplyImage/ApplyBg").GetComponent<Image>();
        applyCol = GameObject.Find("ApplyImage/ApplyCol").GetComponent<Image>();
        applyCos = GameObject.Find("ApplyImage/ApplyCos").GetComponent<Image>();
        applyNbg = GameObject.Find("ApplyImage/ApplyNbg").GetComponent<Image>();
        applyPet = GameObject.Find("ApplyImage/ApplyPet").GetComponent<Image>();

        applyBg.sprite = Resources.Load("Image/Deco/Background/bg_" + dm.data.deco_bg, typeof(Sprite)) as Sprite;
        applyCol.sprite = Resources.Load("Animation/1/Character/" + dm.data.deco_col + "/0", typeof(Sprite)) as Sprite;
        applyCos.sprite = Resources.Load("Image/Deco/Character/Costume/1/cos_" + dm.data.deco_cos, typeof(Sprite)) as Sprite;
        applyNbg.sprite = Resources.Load("Image/Deco/NameBackground/nbg_" + dm.data.deco_nbg, typeof(Sprite)) as Sprite;
        applyPet.sprite = Resources.Load("Image/Deco/Pet/1/pet_" + dm.data.deco_pet, typeof(Sprite)) as Sprite;

        drawPop = GameObject.Find("DrawPop");
        selectPop = GameObject.Find("SelectPop");
        errorPop = GameObject.Find("ErrorPop");
        drawing = GameObject.Find("Drawing");
        resultPop = GameObject.Find("ResultPop");
        //blur = GameObject.Find("Blur");
        //blur.SetActive(false);

        getImage = GameObject.Find("DrawPop/ResultPop/Image").GetComponent<Image>();

        havePoint = dm.data.point;

        GameObject.Find("Background/PointTxt").GetComponent<Text>().text = havePoint + "";

        drawVideo.SetActive(false);
        drawing.SetActive(false);
        errorPop.SetActive(false);
        selectPop.SetActive(false);
        drawPop.SetActive(false);

        magImgPop = GameObject.Find("MagnifyImage");
        magImgPop.SetActive(false);

        img = new GameObject[12];
        txt = new GameObject[12];
        btn = new GameObject[12];
        shadow = new GameObject[12];


        for (int i=0;i<12;i++)
        {
            img[i] = GameObject.Find("Background/Inventory/ImgPanel/" + i + "/Image");
            txt[i] = GameObject.Find("Background/Inventory/ImgPanel/" + i + "/Text");
            btn[i] = GameObject.Find("Background/Inventory/ImgPanel/" + i);
            shadow[i] = GameObject.Find("Background/Inventory/ImgPanel/" + i + "/Shadow");
            Debug.Log(i + "���� �̹���");
        }
        selectType = 1;

        GetDeco_Color();
    }

    void Update()
    {
        // �ڷΰ��� ��ư�� �ι� ���� �� MainScene���� ���ư���.
        if (Input.GetKey(KeyCode.Escape))
        {
            LoadingSceneManager.Instance.LoadScene("Home");
        }
    }

    public void SelectTypeBtn(int no)
    {
        selectType = no;

        colBtn.sprite = Resources.Load("Image/Deco/normalBtn", typeof(Sprite)) as Sprite;
        petBtn.sprite = Resources.Load("Image/Deco/normalBtn", typeof(Sprite)) as Sprite;
        cosBtn.sprite = Resources.Load("Image/Deco/normalBtn", typeof(Sprite)) as Sprite;
        bgBtn.sprite = Resources.Load("Image/Deco/normalBtn", typeof(Sprite)) as Sprite;
        nbgBtn.sprite = Resources.Load("Image/Deco/normalBtn", typeof(Sprite)) as Sprite;

        switch (no)
        {
            case 1:
                colBtn.sprite = Resources.Load("Image/Deco/clickedBtn", typeof(Sprite)) as Sprite;
                GetDeco_Color();
                break;
            case 2:
                petBtn.sprite = Resources.Load("Image/Deco/clickedBtn", typeof(Sprite)) as Sprite;
                GetDeco_Pet();
                break;
            case 3:
                cosBtn.sprite = Resources.Load("Image/Deco/clickedBtn", typeof(Sprite)) as Sprite;
                GetDeco_Costume();
                break;
            case 4:
                bgBtn.sprite = Resources.Load("Image/Deco/clickedBtn", typeof(Sprite)) as Sprite;
                GetDeco_Background();
                break;
            case 5:
                nbgBtn.sprite = Resources.Load("Image/Deco/clickedBtn", typeof(Sprite)) as Sprite;
                GetDeco_NameBackground();
                break;
        }
    }

    public void GetDeco_Color()
    {
        for(int i=0;i<12;i++)
        {
            img[i].GetComponent<Image>().sprite = Resources.Load("Image/Deco/Character/Color/1/col_" + i, typeof(Sprite)) as Sprite;
            txt[i].GetComponent<Text>().text = colName[i];
            btn[i].GetComponent<Button>().interactable = true;

            Color imgCol = img[i].GetComponent<Image>().color;
            Color shaCol = shadow[i].GetComponent<Image>().color;
            imgCol.a = 1.0f;
            shaCol.a = 1.0f;
            // �������� ���� �������� �� ���İ� ���� �� ���� ��ư ��Ȱ��ȭ
            if (!dm.data.have_deco_col[i])
            { 
                btn[i].GetComponent<Button>().interactable = false;
                imgCol.a = 0.15f;
                shaCol.a = 0f;
            }
            img[i].GetComponent<Image>().color = imgCol;
            shadow[i].GetComponent<Image>().color = shaCol;
        }
    }

    public void GetDeco_Pet()
    {
        for (int i = 0; i < 12; i++)
        {
            img[i].GetComponent<Image>().sprite = Resources.Load("Image/Deco/Pet/1/pet_" + i, typeof(Sprite)) as Sprite;
            txt[i].GetComponent<Text>().text = petName[i];
            btn[i].GetComponent<Button>().interactable = true;

            Color imgCol = img[i].GetComponent<Image>().color;
            Color shaCol = shadow[i].GetComponent<Image>().color;
            imgCol.a = 1.0f;
            shaCol.a = 1.0f;
            // �������� ���� �������� �� ���İ� ���� �� ���� ��ư ��Ȱ��ȭ
            if (!dm.data.have_deco_pet[i])
            {
                btn[i].GetComponent<Button>().interactable = false;
                imgCol.a = 0.15f;
                shaCol.a = 0f;
            }
            img[i].GetComponent<Image>().color = imgCol;
            shadow[i].GetComponent<Image>().color = shaCol;
        }
    }

    public void GetDeco_Costume()
    {
        for (int i = 0; i < 12; i++)
        {
            img[i].GetComponent<Image>().sprite = Resources.Load("Image/Deco/Character/Costume/Example/cos_" + i, typeof(Sprite)) as Sprite;
            txt[i].GetComponent<Text>().text = cosName[i];
            btn[i].GetComponent<Button>().interactable = true;

            Color imgCol = img[i].GetComponent<Image>().color;
            Color shaCol = shadow[i].GetComponent<Image>().color;
            imgCol.a = 1.0f;
            shaCol.a = 1.0f;
            // �������� ���� �������� �� ���İ� ���� �� ���� ��ư ��Ȱ��ȭ
            if (!dm.data.have_deco_cos[i])
            {
                btn[i].GetComponent<Button>().interactable = false;
                imgCol.a = 0.15f;
                shaCol.a = 0f;
            }
            img[i].GetComponent<Image>().color = imgCol;
            shadow[i].GetComponent<Image>().color = shaCol;
        }
    }

    public void GetDeco_Background()
    {
        for (int i = 0; i < 12; i++)
        {
            img[i].GetComponent<Image>().sprite = Resources.Load("Image/Deco/Background/bg_" + i, typeof(Sprite)) as Sprite;
            txt[i].GetComponent<Text>().text = bgName[i];
            btn[i].GetComponent<Button>().interactable = true;

            Color imgCol = img[i].GetComponent<Image>().color;
            Color shaCol = shadow[i].GetComponent<Image>().color;
            imgCol.a = 1.0f;
            shaCol.a = 1.0f;
            // �������� ���� �������� �� ���İ� ���� �� ���� ��ư ��Ȱ��ȭ
            if (!dm.data.have_deco_bg[i])
            {
                btn[i].GetComponent<Button>().interactable = false;
                imgCol.a = 0.15f;
                shaCol.a = 0f;
            }
            img[i].GetComponent<Image>().color = imgCol;
            shadow[i].GetComponent<Image>().color = shaCol;
        }
    }

    public void GetDeco_NameBackground()
    {
        for (int i = 0; i < 12; i++)
        {
            img[i].GetComponent<Image>().sprite = Resources.Load("Image/Deco/NameBackground/nbg_" + i, typeof(Sprite)) as Sprite;
            txt[i].GetComponent<Text>().text = nbgName[i];
            btn[i].GetComponent<Button>().interactable = true;

            Color imgCol = img[i].GetComponent<Image>().color;
            Color shaCol = shadow[i].GetComponent<Image>().color;
            imgCol.a = 1.0f;
            shaCol.a = 1.0f;
            // �������� ���� �������� �� ���İ� ���� �� ���� ��ư ��Ȱ��ȭ
            if (!dm.data.have_deco_nbg[i])
            {
                btn[i].GetComponent<Button>().interactable = false;
                imgCol.a = 0.15f;
                shaCol.a = 0f;
            }
            img[i].GetComponent<Image>().color = imgCol;
            shadow[i].GetComponent<Image>().color = shaCol;
        }
    }

    public void DecoApply(int itemNo)
    {
        switch(selectType)
        {
            case 1:
                applyCol.sprite = Resources.Load("Animation/1/Character/" + itemNo + "/0", typeof(Sprite)) as Sprite;
                dm.data.deco_col = itemNo;
                break;
            case 2:
                applyPet.sprite = Resources.Load("Image/Deco/Pet/1/pet_" + itemNo, typeof(Sprite)) as Sprite;
                dm.data.deco_pet = itemNo;
                break;
            case 3:
                applyCos.sprite = Resources.Load("Image/Deco/Character/Costume/1/cos_" + itemNo, typeof(Sprite)) as Sprite;
                dm.data.deco_cos = itemNo;
                break;
            case 4:
                applyBg.sprite = Resources.Load("Image/Deco/Background/bg_" + itemNo, typeof(Sprite)) as Sprite;
                dm.data.deco_bg = itemNo;
                break;
            case 5:
                applyNbg.sprite = Resources.Load("Image/Deco/NameBackground/nbg_" + itemNo, typeof(Sprite)) as Sprite;
                dm.data.deco_nbg = itemNo;
                break;
        }
        DataManager.SaveIngameData(dm.data);
    }

    public void ImageMagnifyBtnClick()
    {
        //blur.SetActive(true);
        magImgPop.SetActive(true);
        GameObject.Find("MagnifyImage/MagBg").GetComponent<Image>().sprite = Resources.Load("Image/Deco/Background/bg_" + dm.data.deco_bg, typeof(Sprite)) as Sprite;
        GameObject.Find("MagnifyImage/MagCol").GetComponent<Image>().sprite = Resources.Load("Animation/1/Character/" + dm.data.deco_col + "/0", typeof(Sprite)) as Sprite;
        GameObject.Find("MagnifyImage/MagCos").GetComponent<Image>().sprite = Resources.Load("Image/Deco/Character/Costume/1/cos_" + dm.data.deco_cos, typeof(Sprite)) as Sprite;
        GameObject.Find("MagnifyImage/MagNbg").GetComponent<Image>().sprite = Resources.Load("Image/Deco/NameBackground/nbg_" + dm.data.deco_nbg, typeof(Sprite)) as Sprite;
        GameObject.Find("MagnifyImage/MagPet").GetComponent<Image>().sprite = Resources.Load("Image/Deco/Pet/1/pet_" + dm.data.deco_pet, typeof(Sprite)) as Sprite;
    }

    public void MagnifyPopClick()
    {
        //blur.SetActive(false);
        magImgPop.SetActive(false);
    }

    public void HomeDrawBtnClick()
    {
        //blur.SetActive(true);
        drawPop.SetActive(true);
        selectPop.SetActive(false);
        errorPop.SetActive(false);
        drawing.SetActive(false);
        resultPop.SetActive(false);
        
        int haveCol = 0;
        int havePet = 0;
        int haveCos = 0;
        int haveBg = 0;
        int haveNbg = 0;
        for(int i=0;i<12;i++)
        {
            if (dm.data.have_deco_col[i])
                haveCol++;
            if (dm.data.have_deco_pet[i])
                havePet++;
            if (dm.data.have_deco_cos[i])
                haveCos++;
            if (dm.data.have_deco_bg[i])
                haveBg++;
            if (dm.data.have_deco_nbg[i])
                haveNbg++;
        }
        GameObject.Find("DrawScroll/Viewport/Content/ColorDraw/Count").GetComponent<Text>().text = haveCol + "/12";
        if(haveCol>=12)
        {
            GameObject.Find("DrawScroll/Viewport/Content/ColorDraw/DrawBtn").GetComponent<Button>().interactable = false;
        }
        GameObject.Find("DrawScroll/Viewport/Content/PetDraw/Count").GetComponent<Text>().text = havePet + "/12";
        if (havePet >= 12)
        {
            GameObject.Find("DrawScroll/Viewport/Content/PetDraw/DrawBtn").GetComponent<Button>().interactable = false;
        }
        GameObject.Find("DrawScroll/Viewport/Content/CostumeDraw/Count").GetComponent<Text>().text = haveCos + "/12";
        if (haveCos >= 12)
        {
            GameObject.Find("DrawScroll/Viewport/Content/CostumeDraw/DrawBtn").GetComponent<Button>().interactable = false;
        }
        GameObject.Find("DrawScroll/Viewport/Content/BgDraw/Count").GetComponent<Text>().text = haveBg + "/12";
        if (haveBg >= 12)
        {
            GameObject.Find("DrawScroll/Viewport/Content/BgDraw/DrawBtn").GetComponent<Button>().interactable = false;
        }
        GameObject.Find("DrawScroll/Viewport/Content/NameBgDraw/Count").GetComponent<Text>().text = haveNbg + "/12";
        if (haveNbg >= 12)
        {
            GameObject.Find("DrawScroll/Viewport/Content/NameBgDraw/DrawBtn").GetComponent<Button>().interactable = false;
        }

    }

    public void DrawBtnClick(int no)
    {
        nowDraw = no;
        selectPop.SetActive(true);
        switch(no)
        {
            case 1:
                GameObject.Find("SelectPop/Image/Text").GetComponent<Text>().text = "30 �������� <color=#116D0E>����</color>�� �����ðڽ��ϱ�?";
                break;
            case 2:
                GameObject.Find("SelectPop/Image/Text").GetComponent<Text>().text = "30 �������� <color=#116D0E>��</color>�� �����ðڽ��ϱ�?";
                break;
            case 3:
                GameObject.Find("SelectPop/Image/Text").GetComponent<Text>().text = "30 �������� <color=#116D0E>�ڽ�Ƭ</color>�� �����ðڽ��ϱ�?";
                break;
            case 4:
                GameObject.Find("SelectPop/Image/Text").GetComponent<Text>().text = "30 �������� <color=#116D0E>���</color>�� �����ðڽ��ϱ�?";
                break;
            case 5:
                GameObject.Find("SelectPop/Image/Text").GetComponent<Text>().text = "30 �������� <color=#116D0E>�̸���</color>�� �����ðڽ��ϱ�?";
                break;
        }
    }

    public void SelectBtnClick(bool ok)
    {
        
        if(ok)
        {
            if (havePoint < 30)
            {
                errorPop.SetActive(true);
                GameObject.Find("ErrorPop/HavePoint").GetComponent<Text>().text = "���� ����Ʈ : " + havePoint + "P";
                GameObject.Find("ErrorPop/LackPoint").GetComponent<Text>().text = "���� ����Ʈ : " + (30 - havePoint) + "P";
            }
            else
            {
                drawing.SetActive(true);
            }
            
        }
        selectPop.SetActive(false);
    }

    public void Draw()
    {
        drawVideo.SetActive(true);
        videoClip.Play();
        havePoint -= 30;
        dm.data.point -= 30;
        GameObject.Find("Background/PointTxt").GetComponent<Text>().text = havePoint + "";
        switch (nowDraw)
        {
            case 1:
                Invoke("ColorDraw",6f);
                PointData np1 = new PointData(DateTime.Now, "���� �̱�", -30);
                dm.data.ptList.Add(np1);
                break;
            case 2:
                Invoke("PetDraw", 6f);
                PointData np2 = new PointData(DateTime.Now, "�� �̱�", -30);
                dm.data.ptList.Add(np2);
                break;
            case 3:
                Invoke("CostumeDraw", 6f);
                PointData np3 = new PointData(DateTime.Now, "�ڽ�Ƭ �̱�", -30);
                dm.data.ptList.Add(np3);
                break;
            case 4:
                Invoke("BackgroundDraw", 6f);
                PointData np4 = new PointData(DateTime.Now, "��� �̱�", -30);
                dm.data.ptList.Add(np4);
                break;
            case 5:
                Invoke("NameBackgroundDraw", 6f);
                PointData np5 = new PointData(DateTime.Now, "�̸��� �̱�", -30);
                dm.data.ptList.Add(np5);
                break;
        }

        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        yield return StartCoroutine(Wait());
        drawVideo.SetActive(false);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(6.0f);
    }

    public void ColorDraw()
    {
        resultPop.SetActive(true);
        drawing.SetActive(false);
        int getItem = UnityEngine.Random.Range(1, 12);
        if (dm.data.have_deco_col[getItem])
        {
            Debug.Log("�ٽ� �̱�");
            ColorDraw();
        }
        else
        {
            GameObject.Find("ResultPop/Text").GetComponent<Text>().text = "<color=#116D0E>��" + colName[getItem] + " ������</color>�� ȹ���ϼ̽��ϴ�!";
            getImage.sprite = Resources.Load("Image/Deco/Character/Color/1/col_" + getItem, typeof(Sprite)) as Sprite;
            dm.data.have_deco_col[getItem] = true;
            DataManager.SaveIngameData(dm.data);
        }
    }

    public void PetDraw()
    {
        resultPop.SetActive(true);
        drawing.SetActive(false);
        int getItem = UnityEngine.Random.Range(1, 12);
        if (dm.data.have_deco_pet[getItem])
        {
            Debug.Log("�ٽ� �̱�");
            PetDraw();
        }
        else
        {
            GameObject.Find("ResultPop/Text").GetComponent<Text>().text = "<color=#116D0E>��" + petName[getItem] + " �ꡱ</color>�� ȹ���ϼ̽��ϴ�!";
            getImage.sprite = Resources.Load("Image/Deco/Pet/1/pet_" + getItem, typeof(Sprite)) as Sprite;
            dm.data.have_deco_pet[getItem] = true;
            DataManager.SaveIngameData(dm.data);
        }
    }

    public void CostumeDraw()
    {
        resultPop.SetActive(true);
        drawing.SetActive(false);
        int getItem = UnityEngine.Random.Range(1, 12);
        if (dm.data.have_deco_cos[getItem])
        {
            Debug.Log("�ٽ� �̱�");
            CostumeDraw();
        }
        else
        {
            GameObject.Find("ResultPop/Text").GetComponent<Text>().text = "<color=#116D0E>��" + cosName[getItem] + " �ڽ�Ƭ��</color>�� ȹ���ϼ̽��ϴ�!";
            getImage.sprite = Resources.Load("Image/Deco/Character/Costume/1/cos_" + getItem, typeof(Sprite)) as Sprite;
            dm.data.have_deco_cos[getItem] = true;
            DataManager.SaveIngameData(dm.data);
        }
    }

    public void BackgroundDraw()
    {
        resultPop.SetActive(true);
        drawing.SetActive(false);
        int getItem = UnityEngine.Random.Range(1, 12);
        if (dm.data.have_deco_bg[getItem])
        {
            Debug.Log("�ٽ� �̱�");
            BackgroundDraw();
        }
        else
        {
            GameObject.Find("ResultPop/Text").GetComponent<Text>().text = "<color=#116D0E>��" + bgName[getItem] + " ��桱</color>�� ȹ���ϼ̽��ϴ�!";
            getImage.sprite = Resources.Load("Image/Deco/Background/bg_" + getItem, typeof(Sprite)) as Sprite;
            dm.data.have_deco_bg[getItem] = true;
            DataManager.SaveIngameData(dm.data);
        }
    }

    public void NameBackgroundDraw()
    {
        resultPop.SetActive(true);
        drawing.SetActive(false);
        int getItem = UnityEngine.Random.Range(1, 12);
        if (dm.data.have_deco_nbg[getItem])
        {
            Debug.Log("�ٽ� �̱�");
            NameBackgroundDraw();
        }
        else
        {
            GameObject.Find("ResultPop/Text").GetComponent<Text>().text = "<color=#116D0E>��" + nbgName[getItem] + " �̸��ǡ�</color>�� ȹ���ϼ̽��ϴ�!";
            getImage.sprite = Resources.Load("Image/Deco/NameBackground/nbg_" + getItem, typeof(Sprite)) as Sprite;
            dm.data.have_deco_nbg[getItem] = true;
            DataManager.SaveIngameData(dm.data);
        }
    }

    
    public void CloseDrawPopup(int no)
    {
        switch (no)
        {
            case 1:
                //blur.SetActive(false);
                drawPop.SetActive(false);
                SelectTypeBtn(1);
                break;
            case 2:
                errorPop.SetActive(false);
                break;
            case 3:
                resultPop.SetActive(false);
                HomeDrawBtnClick();
                break;
        }
    }
}
