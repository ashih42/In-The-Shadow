using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
	[SerializeField] private Toggle isTesterToggle;
	[SerializeField] private Toggle isDogeModeToggle;
	[SerializeField] private Slider bgmSlider;
	[SerializeField] private Slider sfxSlider;
	[SerializeField] private AudioClip bgmClip;

	private void Start()
	{
		this.isTesterToggle.isOn = GameManager.Instance.IsTester;
		this.isDogeModeToggle.isOn = GameManager.Instance.IsDogeMode;
		this.bgmSlider.value = AudioManager.Instance.BGMVolume;
		this.sfxSlider.value = AudioManager.Instance.SFXVolume;
		AudioManager.Instance.PlayBGM(this.bgmClip);
	}

	public void PlayGame()
	{
		GameManager.Instance.IsTester = this.isTesterToggle.isOn;
		GameManager.Instance.IsDogeMode = this.isDogeModeToggle.isOn;
		GameManager.Instance.LoadScene("Level Selection Screen");
	}

	public void SetBGMVolume()
	{
		AudioManager.Instance.BGMVolume = this.bgmSlider.value;
	}

	public void SetSFXVolume()
	{
		AudioManager.Instance.SFXVolume = this.sfxSlider.value;
	}

	public void DeleteSave()
	{
		AudioManager.Instance.PlaySFX("Delete");
		GameManager.Instance.ResetGameState();
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
