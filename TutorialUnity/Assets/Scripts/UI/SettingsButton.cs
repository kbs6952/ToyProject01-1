using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public Button ConfirmBtn;
    public Button BackButton;
    public Button ReturnToButton;

    [SerializeField] private GamePlayUIManager gamePlayUIManger;

    public void Awake()
    {
        ReturnToButton.onClick.
        ConfirmBtn.onClick.AddListener(()=>SaveManager.Instance.SaveGame());
    }

}
