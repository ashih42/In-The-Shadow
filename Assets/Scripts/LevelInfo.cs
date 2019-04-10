using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{
	[SerializeField] private int levelNumber;

	private Image panelImage;
	[SerializeField] private Text levelNameText;
	[SerializeField] private RawImage[] starImages;
	[SerializeField] private Button playButton;
	[SerializeField] private Text playButtonText;
	[SerializeField] private Slider pathSlider;

	private void Start()
	{
		this.panelImage = GetComponent<Image>();
		this.levelNameText.text = "Level " + this.levelNumber;
		this.SetStars();
		this.SetButton();
		this.SetSlider();

		if (GameManager.Instance.IsNewestLevel(this.levelNumber))
			StartCoroutine(this.UnlockLevelCoroutine());
	}

	private void SetStars()
	{
		int starsEarned = GameManager.Instance.GetStarsEarnedAtLevel(this.levelNumber);

		for (int i = 0; i < this.starImages.Length; i++)
		{
			if (i < starsEarned)
				this.starImages[i].texture = AssetsManager.Instance.FilledStarTexture;
			else
				this.starImages[i].texture = AssetsManager.Instance.BlankStarTexture;
		}
	}

	private void SetButton()
	{
		bool isLevelAvailable = GameManager.Instance.IsTester ||
			GameManager.Instance.GetIsLevelAvailable(this.levelNumber);

		if (!isLevelAvailable)
		{
			this.playButton.interactable = false;
			this.playButtonText.text = "Locked";
		}
	}

	private void SetSlider()
	{
		if (this.pathSlider != null)
		{
			if (GameManager.Instance.GetIsLevelAvailable(this.levelNumber))
				this.pathSlider.value = 1f;
			else
				this.pathSlider.value = 0;
		}
	}

	private IEnumerator UnlockLevelCoroutine()
	{
		// Fill up the slider
		if (this.pathSlider != null)
		{
			this.pathSlider.value = 0;
			while (this.pathSlider.value < 1f)
			{
				this.pathSlider.value += 0.005f;
				yield return null;
			}
		}
		// Flash the panel forever
		while (true)
		{
			this.panelImage.color = Color.Lerp(Color.white, Color.yellow, Mathf.PingPong(Time.time, 1));
			yield return null;
		}
	}

	public void PlayLevel()
	{
		GameManager.Instance.LoadScene("Level " + this.levelNumber);
	}
	
}
