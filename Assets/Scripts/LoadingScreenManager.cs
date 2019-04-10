using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
	private static readonly string[] TIP_MESSAGES =
	{
		"The secret to completing a level is to rotate/move the objects until their shadows resemble something.",
		"Some objects cannot be rotated or moved at all.",
		"Some puzzles accept correct shadows of any orientation.  Some puzzles require you to try harder.",
		"Keep the projected shadows between the two candles, or else!",
		"To score 3 stars (or 3 doges), you must complete the level without using a hint or peeking.",
		"You must take some time to read these super helpful loading screen tips.",
		"You should go to the options menu and enable Doge Mode.",
		"You can play as tester, but only if you are actually a tester.  Otherwise, it is cheating.",
		"There used to be a door in the house, but it's gone now.",
		"If you enjoyed this game, please support the author on patreon by sending monies.",
		"Make sure you open correction slots all day everyday, so you can take new cadets' ft_debut points.",
	};

	[SerializeField] private Text tipText;

	private void Awake()
	{
		SceneManager.activeSceneChanged += this.DisableLoadingScreen;
	}

	private void OnEnable()
	{
		this.tipText.text = "Tip: " + TIP_MESSAGES[Random.Range(0, TIP_MESSAGES.Length)];
	}

	private void DisableLoadingScreen(Scene current, Scene next)
	{
		// Guarantee at least 1 second to enjoy the loading screen
		Invoke("ActuallyDisableLoadingScreen", 1f);
	}

	private void ActuallyDisableLoadingScreen()
	{
		this.gameObject.SetActive(false);
	}
}
