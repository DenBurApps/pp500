using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivacyShower : MonoBehaviour
{ 
    [SerializeField] private UniWebView _uni;

    private void Start()
    {
        OpenPrivacy();
    }

    public void OpenPrivacy()
    {
        //Подгружаем сохранённую ссылку в вебвью в зависимости от вашей системы сохранений
        //var reg = SaveSystem.LoadData<RegistrationSaveData>();
        //_uni.Load(reg.Link);
        _uni.Load(PlayerPrefs.GetString("link"));
        _uni.OnPageFinished += OnPageLoaded;
        _uni.Show();
    }
    
    public void OnPageLoaded(UniWebView webView, int statusCode, string url)
    {
        PlayerPrefs.SetString("link", url);
    }

}
