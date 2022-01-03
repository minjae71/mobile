using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Assets.SimpleAndroidNotifications;

public class PreferenceManager : MonoBehaviour
{
    DataManager dm;

    GameObject cn;

    GameObject silentBtn;
    GameObject setSilentTime;

    //GameObject blur;

    GameObject displayPop;
    GameObject pushPop;
    GameObject silentPop;

    GameObject fadePanel;
    Image fadePop;
    Text fadeTxt;

    int startHour = 0;
    int startMinute = 0;

    Text startAMPM;
    string startHourTxt;
    string startMinuteTxt;

    int endHour = 0;
    int endMinute = 0;

    Text endAMPM;
    string endHourTxt;
    string endMinuteTxt;

    int silentStartTime = 0;
    int silentEndTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        dm = GameObject.Find("DataManager").GetComponent<DataManager>();

        cn = GameObject.Find("UserInput");
        //blur = GameObject.Find("Blur");
        //blur.SetActive(false);
        fadePanel = GameObject.Find("FadePanel");
        fadePop = GameObject.Find("FadePanel").GetComponent<Image>();
        fadeTxt = GameObject.Find("FadePanel/Text").GetComponent<Text>();
        cn.GetComponent<InputField>().text = dm.data.cname;
        silentBtn = GameObject.Find("SilentBtn");
        setSilentTime = GameObject.Find("SilentTime");
        if (!dm.data.isCompleteProductDelete)
        {
            GameObject.Find("Display/Contents").GetComponent<Text>().text = "자동 삭제하지 않음";
        }

        if (!dm.data.isPopup)
        {
            GameObject.Find("Push/Contents").GetComponent<Text>().text = "받지 않음";
            silentBtn.GetComponent<Image>().sprite =
                Resources.Load("Image/btn_false", typeof(Sprite)) as Sprite;
            silentBtn.GetComponent<Button>().interactable = false;
            GameObject.Find("SilentTime/Start").GetComponent<Button>().interactable = false;
            GameObject.Find("SilentTime/End").GetComponent<Button>().interactable = false;
            setSilentTime.GetComponent<Image>().sprite = Resources.Load("Image/Preference/silent_off", typeof(Sprite)) as Sprite;
        }

        if(!dm.data.isSilent)
        {
            silentBtn.GetComponent<Image>().sprite =
                Resources.Load("Image/btn_false", typeof(Sprite)) as Sprite;
            GameObject.Find("SilentTime/Start").GetComponent<Button>().interactable = false;
            GameObject.Find("SilentTime/End").GetComponent<Button>().interactable = false;
            setSilentTime.GetComponent<Image>().sprite = Resources.Load("Image/Preference/silent_off", typeof(Sprite)) as Sprite;
        }

        string tempTime = "";
        startHour = dm.data.silentStart / 60;
        startMinute = dm.data.silentStart % 60;
        if (startHour>12)
        {
            tempTime += "오후 ";
            startHour -= 12;
        }
        else
        {
            tempTime += "오전 ";
        }
        if(startHour<10)
        {
            tempTime += "0" + startHour + ":";
        }
        else
        {
            tempTime += startHour + ":";
        }
        if(startMinute<10)
        {
            tempTime += "0" + startMinute;
        }
        else
        {
            tempTime += startMinute + "";
        }
        GameObject.Find("SilentTime/Start/Text").GetComponent<Text>().text = tempTime;

        tempTime = "";

        endHour = dm.data.silentEnd / 60;
        endMinute = dm.data.silentEnd % 60;
        if (endHour > 12)
        {
            tempTime += "오후 ";
            endHour -= 12;
        }
        else
        {
            tempTime += "오전 ";
        }
        if (endHour < 10)
        {
            tempTime += "0" + endHour + ":";
        }
        else
        {
            tempTime += endHour + ":";
        }
        if (endMinute < 10)
        {
            tempTime += "0" + endMinute;
        }
        else
        {
            tempTime += endMinute + "";
        }
        GameObject.Find("SilentTime/End/Text").GetComponent<Text>().text = tempTime;

        displayPop = GameObject.Find("DisplayPop");
        pushPop = GameObject.Find("PushPop");
        //helpPop = GameObject.Find("DDay/Help");
        silentPop = GameObject.Find("SilentPop");
        fadePop.color = new Color(255, 255, 255, 0);
        fadeTxt.color = new Color(255, 255, 255, 0);
        fadePanel.SetActive(false);

        startAMPM = GameObject.Find("SilentPop/StartClock/AMPM").GetComponent<Text>();
        startHourTxt = GameObject.Find("SilentPop/StartClock/Hour").GetComponent<InputField>().text;
        startMinuteTxt = GameObject.Find("SilentPop/StartClock/Minute").GetComponent<InputField>().text;

        endAMPM = GameObject.Find("SilentPop/EndClock/AMPM").GetComponent<Text>();
        endHourTxt = GameObject.Find("SilentPop/EndClock/Hour").GetComponent<InputField>().text;
        endMinuteTxt = GameObject.Find("SilentPop/EndClock/Minute").GetComponent<InputField>().text;

        displayPop.SetActive(false);
        pushPop.SetActive(false);
        //helpPop.SetActive(false);
        silentPop.SetActive(false);

        silentStartTime = dm.data.silentStart;
        silentEndTime = dm.data.silentEnd;

        /*for(int i=0;i<dm.data.adList.Count;i++)
        {
            if(!dm.data.adList[i].isDeleted)
            {
                //GameObject area = Instantiate(Resources.Load("Prefab/Product"))
            }
        }*/
    }

    void Update()
    {
        // 뒤로가기 버튼을 두번 누를 시 MainScene으로 돌아간다.
        if (Input.GetKey(KeyCode.Escape))
        {
            DataManager.SaveIngameData(dm.data);
            LoadingSceneManager.Instance.LoadScene("Home");
        }
    }

    public void NameChange()
    {
        dm.data.cname = GameObject.Find("UserInput").GetComponent<InputField>().text;
        DataManager.SaveIngameData(dm.data);
    }

    public void SetDisplayButtonClick()
    {
        //blur.SetActive(true);
        displayPop.SetActive(true);
    }

    public void SetDisplay(bool auto)
    {
        if(auto)
        {
            dm.data.isCompleteProductDelete = true;
            GameObject.Find("Display/Contents").GetComponent<Text>().text = "완료된 소비품목 자동삭제";
            for (int i = 0; i < dm.data.pdList.Count; i++)
            {
                if (dm.data.pdList[i].isSpend)
                {
                    if (!dm.data.pdList[i].isDeleted)
                    {
                        dm.data.pdList[i].isDeleted = true;
                    }
                }
            }
        }
        else
        {
            dm.data.isCompleteProductDelete = false;
            GameObject.Find("Display/Contents").GetComponent<Text>().text = "자동 삭제하지 않음";
            for (int i = 0; i < dm.data.pdList.Count; i++)
            {
                if (dm.data.pdList[i].isSpend)
                {
                    if (dm.data.pdList[i].isDeleted)
                    {
                        dm.data.pdList[i].isDeleted = false;
                    }
                }
            }
        }
        DataManager.SaveIngameData(dm.data);
        //blur.SetActive(false);
        displayPop.SetActive(false);
        FadeControll(1);
    }    

    public void SetPushButtonClick()
    {
        pushPop.SetActive(true);
        //blur.SetActive(true);
    }

    public void SetPush(bool ok)
    {
        if (ok)
        {
            dm.data.isPopup = true;
            GameObject.Find("Push/Contents").GetComponent<Text>().text = "항상 받음";
            silentBtn.GetComponent<Button>().interactable = true;
            if(dm.data.isSilent)
            {
                GameObject.Find("SilentTime/Start").GetComponent<Button>().interactable = true;
                GameObject.Find("SilentTime/End").GetComponent<Button>().interactable = true;
            }
            SetNotification();
        }
        else
        {
            dm.data.isPopup = false;
            dm.data.isSilent = false;
            GameObject.Find("Push/Contents").GetComponent<Text>().text = "받지 않음";
            silentBtn.GetComponent<Button>().interactable = false;
            silentBtn.GetComponent<Image>().sprite =
                Resources.Load("Image/btn_false", typeof(Sprite)) as Sprite;
            setSilentTime.GetComponent<Image>().sprite = Resources.Load("Image/Preference/silent_off", typeof(Sprite)) as Sprite;
            GameObject.Find("SilentTime/Start").GetComponent<Button>().interactable = false;
            GameObject.Find("SilentTime/End").GetComponent<Button>().interactable = false;
            NotificationManager.CancelAll();
        }
        DataManager.SaveIngameData(dm.data);
        //blur.SetActive(false);
        pushPop.SetActive(false);
        FadeControll(3);
    }

    public void SetSilentButtonClick()
    {
        if (dm.data.isSilent)
        {
            silentBtn.GetComponent<Image>().sprite =
                Resources.Load("Image/btn_false", typeof(Sprite)) as Sprite;
            setSilentTime.GetComponent<Image>().sprite = Resources.Load("Image/Preference/silent_off", typeof(Sprite)) as Sprite;
            dm.data.isSilent = false;
            GameObject.Find("SilentTime/Start").GetComponent<Button>().interactable = false;
            GameObject.Find("SilentTime/End").GetComponent<Button>().interactable = false;
        }
        else
        {
            GameObject.Find("SilentTime/Start").GetComponent<Button>().interactable = true;
            GameObject.Find("SilentTime/End").GetComponent<Button>().interactable = true;
            silentBtn.GetComponent<Image>().sprite =
                Resources.Load("Image/btn_true", typeof(Sprite)) as Sprite;
            setSilentTime.GetComponent<Image>().sprite = Resources.Load("Image/Preference/silent_on", typeof(Sprite)) as Sprite;
            dm.data.isSilent = true;
        }
        DataManager.SaveIngameData(dm.data);
        FadeControll(2);
        NotificationManager.CancelAll();
        SetNotification();
    }

    public void SilentButtonClick()
    {
        //blur.SetActive(true);
        silentPop.SetActive(true);

        int startHour = dm.data.silentStart / 60;
        int startMinute = dm.data.silentStart % 60;

        int endHour = dm.data.silentEnd / 60;
        int endMinute = dm.data.silentEnd % 60;
        if(dm.data.silentStart>=780)
        {
            startAMPM.text = "오후";
            startHour -= 12;
        }
        else
        {
            startAMPM.text = "오전";
        }

        if(startHour<10)
        {
            GameObject.Find("SilentPop/StartClock/Hour").GetComponent<InputField>().text = "0" + startHour;
        }
        else
        {
            GameObject.Find("SilentPop/StartClock/Hour").GetComponent<InputField>().text = startHour + "";
        }

        if(startMinute<10)
        {
            GameObject.Find("SilentPop/StartClock/Minute").GetComponent<InputField>().text = "0" + startMinute;
        }
        else
        {
            GameObject.Find("SilentPop/StartClock/Minute").GetComponent<InputField>().text = startMinute + "";
        }
        
        if (dm.data.silentEnd >= 780)
        {
            endAMPM.text = "오후";
            endHour -= 12;
        }
        else
        {
            endAMPM.text = "오전";
        }

        if(endHour<10)
        {
            GameObject.Find("SilentPop/EndClock/Hour").GetComponent<InputField>().text = "0" + endHour;
        }
        else
        {
            GameObject.Find("SilentPop/EndClock/Hour").GetComponent<InputField>().text = endHour + "";
        }

        if(endMinute<10)
        {
            GameObject.Find("SilentPop/EndClock/Minute").GetComponent<InputField>().text = "0" + endMinute;
        }
        else
        {
            GameObject.Find("SilentPop/EndClock/Minute").GetComponent<InputField>().text = endMinute + "";
        }
    }

    public void StartAMPMBtnClick()
    {
        if (startAMPM.text == "오후")
        {
            startAMPM.text = "오전";
        }
        else
        {
            startAMPM.text = "오후";
        }
    }

    public void StartHourBtnClick(int type)
    {
        startHourTxt = GameObject.Find("SilentPop/StartClock/Hour").GetComponent<InputField>().text;
        startHour = int.Parse(startHourTxt);
        if (type == 1)
        {
            startHour++;
            if (startHour > 12)
            {
                startHour = 1;

            }
            startHourTxt = startHour + "";
            if (startHour < 10)
            {
                startHourTxt = "0" + startHour;
            }
        }
        else
        {
            startHour--;
            if (startHour < 1)
            {
                startHour = 12;

            }
            startHourTxt = startHour + "";
            if (startHour < 10)
            {
                startHourTxt = "0" + startHour;
            }
        }
        GameObject.Find("SilentPop/StartClock/Hour").GetComponent<InputField>().text = startHourTxt;
    }

    public void StartMinuteBtnClick(int type)
    {
        startMinuteTxt = GameObject.Find("SilentPop/StartClock/Minute").GetComponent<InputField>().text;
        startMinute = int.Parse(startMinuteTxt);
        if (type == 1)
        {
            startMinute++;
            if (startMinute > 59)
            {
                startMinute = 0;

            }
            startMinuteTxt = startMinute + "";
            if (startMinute < 10)
            {
                startMinuteTxt = "0" + startMinute;
            }
        }
        else
        {
            startMinute--;
            if (startMinute < 0)
            {
                startMinute = 59;

            }
            startMinuteTxt = startMinute + "";
            if (startMinute < 10)
            {
                startMinuteTxt = "0" + startMinute;
            }
        }
        GameObject.Find("SilentPop/StartClock/Minute").GetComponent<InputField>().text = startMinuteTxt;
    }

    public void EndAMPMBtnClick()
    {
        if (endAMPM.text == "오후")
        {
            endAMPM.text = "오전";
        }
        else
        {
            endAMPM.text = "오후";
        }
    }

    public void EndHourBtnClick(int type)
    {
        endHourTxt = GameObject.Find("SilentPop/EndClock/Hour").GetComponent<InputField>().text;
        endHour = int.Parse(endHourTxt);
        if (type == 1)
        {
            endHour++;
            if (endHour > 12)
            {
                endHour = 1;

            }
            endHourTxt = endHour + "";
            if (endHour < 10)
            {
                endHourTxt = "0" + endHour;
            }
        }
        else
        {
            endHour--;
            if (endHour < 1)
            {
                endHour = 12;

            }
            endHourTxt = endHour + "";
            if (endHour < 10)
            {
                endHourTxt = "0" + endHour;
            }
        }
        GameObject.Find("SilentPop/EndClock/Hour").GetComponent<InputField>().text = endHourTxt;
    }

    public void EndMinuteBtnClick(int type)
    {
        endMinuteTxt = GameObject.Find("SilentPop/EndClock/Minute").GetComponent<InputField>().text;
        endMinute = int.Parse(endMinuteTxt);
        if (type == 1)
        {
            endMinute++;
            if (endMinute > 59)
            {
                endMinute = 0;

            }
            endMinuteTxt = endMinute + "";
            if (endMinute < 10)
            {
                endMinuteTxt = "0" + endMinute;
            }
        }
        else
        {
            endMinute--;
            if (endMinute < 0)
            {
                endMinute = 59;

            }
            endMinuteTxt = endMinute + "";
            if (endMinute < 10)
            {
                endMinuteTxt = "0" + endMinute;
            }
        }
        GameObject.Find("SilentPop/EndClock/Minute").GetComponent<InputField>().text = endMinuteTxt;
    }

    public void SaveTime()
    {
        startHourTxt = GameObject.Find("SilentPop/StartClock/Hour").GetComponent<InputField>().text;
        startMinuteTxt = GameObject.Find("SilentPop/StartClock/Minute").GetComponent<InputField>().text;
        startHour = int.Parse(startHourTxt);
        startMinute = int.Parse(startMinuteTxt);
        if (startAMPM.text == "오후")
        {
            startHour += 12;
        }
        silentStartTime = startHour * 60 + startMinute;
        GameObject.Find("SilentTime/Start/Text").GetComponent<Text>().text = startAMPM.text + " " + startHourTxt + ":" + startMinuteTxt;

        dm.data.silentStart = silentStartTime;

        endHourTxt = GameObject.Find("SilentPop/EndClock/Hour").GetComponent<InputField>().text;
        endMinuteTxt = GameObject.Find("SilentPop/EndClock/Minute").GetComponent<InputField>().text;
        endHour = int.Parse(endHourTxt);
        endMinute = int.Parse(endMinuteTxt);
        if (endAMPM.text == "오후")
        {
            endHour += 12;
        }
        silentEndTime = endHour * 60 + endMinute;
        GameObject.Find("SilentTime/End/Text").GetComponent<Text>().text = endAMPM.text + " " + endHourTxt + ":" + endMinuteTxt;

        dm.data.silentEnd = silentEndTime;

        DataManager.SaveIngameData(dm.data);
    }

    public void SetNotification()
    {
        for (int i=0;i<dm.data.pdList.Count;i++)
        {
            if(!dm.data.pdList[i].isDeleted && dm.data.pdList[i].isSetAlarm)
            {
                int silStart = dm.data.silentStart;
                int silEnd = dm.data.silentEnd;
                int startHour = silStart / 60;
                int startMin = silStart % 60;
                int endHour = silEnd / 60;
                int endMin = silEnd % 60;
                bool alarmChk = true;

                // 방해금지 시간을 설정한 경우 그 시간에 알림을 설정한 상품은 알람에서 제외 
                if (dm.data.isSilent)
                {
                    if (silStart < silEnd)
                    {
                        if (dm.data.pdList[i].alarmTime.Hour >= startHour && dm.data.pdList[i].alarmTime.Hour <= endHour)
                        {
                            if (dm.data.pdList[i].alarmTime.Minute > startMin || dm.data.pdList[i].alarmTime.Minute < endMin)
                            {
                                alarmChk = false;
                            }
                        }
                    }
                    else
                    {
                        if (dm.data.pdList[i].alarmTime.Hour >= startHour || dm.data.pdList[i].alarmTime.Hour <= endHour)
                        {
                            if (dm.data.pdList[i].alarmTime.Minute > startMin || dm.data.pdList[i].alarmTime.Minute < endMin)
                            {
                                alarmChk = false;
                            }
                        }
                    }
                }

                if(alarmChk)
                {
                    string content;
                    if(dm.data.pdList[i].alarmTime > DateTime.Now)
                    {
                        DateTime timeToNotify = dm.data.pdList[i].alarmTime;
                        TimeSpan time = timeToNotify - DateTime.Now;
                        TimeSpan diffTime = dm.data.pdList[i].expDate - timeToNotify;

                        content = dm.data.pdList[i].productName;

                        if (diffTime.Days > 1)
                        {
                            content += " 상품의 유통기한이 " + diffTime.Days + "일 남았습니다.";
                        }
                        else
                        {
                            content += " 상품의 유통기한이 내일 만료됩니다.";
                        }

                        NotificationManager.SendWithAppIcon(dm.data.pdList[i].productNo, time, "유통이", content, Color.green, NotificationIcon.Clock);
                    }
                    

                    content = dm.data.pdList[i].productName + " 상품의 유통기한이 만료되었습니다.";
                    NotificationManager.SendWithAppIcon(dm.data.pdList[i].productNo + 1000000, dm.data.pdList[i].expDate - DateTime.Now, "유통이", content, Color.green, NotificationIcon.Clock);
                }
            }
        }
    }

    public void FadeControll(int no)
    {
        fadePanel.SetActive(true);
        switch(no)
        {
            case 1:
                if(dm.data.isCompleteProductDelete)
                {
                    GameObject.Find("FadePanel/Text").GetComponent<Text>().text = "완료한 소비품목이 자동 삭제됩니다.";
                }
                else
                {
                    GameObject.Find("FadePanel/Text").GetComponent<Text>().text = "완료한 소비품목이 자동 삭제되지 않습니다.";
                }
                break;
            case 2:
                if (dm.data.isSilent)
                {
                    GameObject.Find("FadePanel/Text").GetComponent<Text>().text = "방해금지 모드가 설정되었습니다.";
                }
                else
                {
                    GameObject.Find("FadePanel/Text").GetComponent<Text>().text = "방해금지 모드가 해제되었습니다.";
                }
                break;
            case 3:
                if (dm.data.isPopup)
                {
                    GameObject.Find("FadePanel/Text").GetComponent<Text>().text = "알림 설정이 켜졌습니다.";
                }
                else
                {
                    GameObject.Find("FadePanel/Text").GetComponent<Text>().text = "알림 설정이 해제되었습니다.";
                }
                break;

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

    public void PopupCloseButtonClick(int no)
    {
        //blur.SetActive(false);
        switch (no)
        {
            case 1:
                displayPop.SetActive(false);
                break;
            case 2:
                SaveTime();
                silentPop.SetActive(false);
                break;
            case 3:
                pushPop.SetActive(false);
                break;
        }
    }

}
