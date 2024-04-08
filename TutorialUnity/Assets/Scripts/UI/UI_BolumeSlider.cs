using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_BolumeSlider : MonoBehaviour
{
   // UI �����̴��� Value�� AudioMixer Parameter�� ��������ֱ� ���� Ŭ����
   
    public Slider slider;
    public string parameter;        // AudioMixer���� �Ķ������ �̸��� ���������� �ֱ� ������, �̸� ��Ī���� ���ڿ� ������ ����

    [SerializeField] private AudioMixer audioMixer;         // AudioMixer�� ����ϱ� ���� ����
    [SerializeField] private float multiplier;              // �����̴� ���� ũ�⸦ �����ϴ� ����

    public void SliderValue(float _value)
    {
        audioMixer.SetFloat(parameter, Mathf.Log10(_value) * multiplier);  // Mathf.Log10 : �������ٴ� �۰�, �����ϰ� ���� ��ȭ�� ǥ��
    }
    public void LoadSlider(float _value)
    {
        if(_value>=0.001f)                      // min���� 0.001f ���� ���� �ҷ��� �� ������.
            slider.value = _value;
    }
}
