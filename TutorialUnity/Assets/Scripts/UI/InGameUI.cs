using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour,ISaveManager
{
    [SerializeField] private UI_BolumeSlider[] volumeSettings;
    
    public void SwitchTo(GameObject _menu)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        if(_menu != null)
        {
            _menu.SetActive(true);   
        }
    }
    public void LoadData(GameData gameData)
    {
        foreach(KeyValuePair<string, float> pair in gameData.volumeSettings)
        {
            foreach(var item in volumeSettings)
            {
                if(item.parameter== pair.Key)
                {
                    item.LoadSlider(pair.Value);
                }
            }
        }
    }
    public void SaveData(ref GameData gameData)
    {
        gameData.volumeSettings.Clear();    // 저장하기 전에 혹시 모를 데이터 초기화.

        foreach (var item in volumeSettings)
        {
            gameData.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }


}
