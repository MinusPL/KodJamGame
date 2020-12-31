using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControllers : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public string volumeName;

	public void Update()
	{
        mixer.SetFloat(volumeName, Mathf.Log10(slider.value) * 20);
    }
}
