using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AudioManager : MonoBehaviour
{
	private static AudioManager instance;

	public static AudioManager Instance
	{
		get
		{
			if (AudioManager.instance == null)
			{
				GameObject go = Instantiate(Resources.Load("AudioManager") as GameObject);
				go.name = "AudioManager";
				AudioManager.instance = go.GetComponent<AudioManager>();
				DontDestroyOnLoad(go);
			}
			return AudioManager.instance;
		}
	}

	[SerializeField] private AudioSource bgmAudioSource;
	[SerializeField] private AudioSource sfxAudioSource;

	[Serializable]
	private struct AudioClipInfo
	{
		public string key;
		public AudioClip value;
	}

	[SerializeField] private AudioClipInfo[] sfxClips;
	private Dictionary<string, AudioClip> sfxDictionary;

	private void Awake()
	{
		// Assert there are no duplicate keys
		Debug.Assert(this.sfxClips.Length == this.sfxClips.Distinct().Count());

		// Build the dictionary
		this.sfxDictionary = new Dictionary<string, AudioClip>();
		foreach (AudioClipInfo sfxClip in this.sfxClips)
			this.sfxDictionary[sfxClip.key] = sfxClip.value;
	}

	public float BGMVolume
	{
		get { return this.bgmAudioSource.volume; }
		set { this.bgmAudioSource.volume = value; }
	}

	public float SFXVolume
	{
		get { return this.sfxAudioSource.volume; }
		set { this.sfxAudioSource.volume = value; }
	}

	public void PlayBGM(AudioClip bgmClip)
	{
		this.bgmAudioSource.clip = bgmClip;
		this.bgmAudioSource.Play();
	}

	public void PlaySFX(string name)
	{
		AudioClip clip = this.sfxDictionary[name];

		this.sfxAudioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		this.sfxAudioSource.PlayOneShot(clip);
	}
}
