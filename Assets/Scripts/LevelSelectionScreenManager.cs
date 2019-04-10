using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionScreenManager : MonoBehaviour
{
	[SerializeField] private AudioClip bgmClip;

	private void Start()
	{
		AudioManager.Instance.PlayBGM(this.bgmClip);
	}

	public void GoToTitleScreen()
	{
		GameManager.Instance.LoadScene("Title Screen");
	}
}
