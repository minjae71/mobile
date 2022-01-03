using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void HomeBtnClick()
    {
        LoadingSceneManager.Instance.LoadScene("Home");
    }
    public void AddBtnClick()
    {
        LoadingSceneManager.Instance.LoadScene("AddProduct");
    }

    public void DrawBtnClick()
    {
        LoadingSceneManager.Instance.LoadScene("Shop");
    }

    public void LockerBtnClick()
    {
        LoadingSceneManager.Instance.LoadScene("Locker");
    }

    public void AchievementsBtnClick()
    {
        LoadingSceneManager.Instance.LoadScene("Achievements");
    }

    public void PreferenceBtnClick()
    {
        LoadingSceneManager.Instance.LoadScene("Preferences");
    }
}
