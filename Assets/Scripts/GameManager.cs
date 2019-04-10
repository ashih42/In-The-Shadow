using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;

	public static GameManager Instance
	{
		get
		{
			if (GameManager.instance == null)
			{
				GameObject go = Instantiate(Resources.Load("GameManager") as GameObject);
				go.name = "GameManager";
				GameManager.instance = go.GetComponent<GameManager>();
				DontDestroyOnLoad(go);
			}
			return GameManager.instance;
		}
	}

	[SerializeField] private int numLevels;
	[SerializeField] private bool[] isLevelAvailable;
	[SerializeField] private int[] starsEarnedAtLevel;

	[SerializeField] private GameObject loadingScreenGO;

	public bool IsTester { get; set; }
	public bool IsDogeMode { get; set; }

	void Awake()
	{
		this.IsTester = false;
		this.IsDogeMode = false;
		this.isLevelAvailable = new bool[this.numLevels];
		this.starsEarnedAtLevel = new int[this.numLevels];

		if (PlayerPrefs.HasKey("isLevelAvailable" + 1))
			this.LoadGameState();
		else
			this.ResetGameState();
	}

	public bool IsNewestLevel(int levelNumber)
	{
		return (this.isLevelAvailable[levelNumber - 1] &&
			this.starsEarnedAtLevel[levelNumber - 1] == 0);
	}

	public bool GetIsLevelAvailable(int levelNumber)
	{
		return this.isLevelAvailable[levelNumber - 1];
	}

	public int GetStarsEarnedAtLevel(int levelNumber)
	{
		return this.starsEarnedAtLevel[levelNumber - 1];
	}

	public void SetLevelCleared(int levelNumber, int stars)
	{
		// Update stars if new high score
		if (this.starsEarnedAtLevel[levelNumber - 1] < stars)
		{
			this.starsEarnedAtLevel[levelNumber - 1] = stars;
			PlayerPrefs.SetInt("starsEarnedAtLevel" + levelNumber, stars);
		}
		// Unlock next level if available
		int nextLevelNumber = levelNumber + 1;
		if (nextLevelNumber <= this.numLevels)
		{
			this.isLevelAvailable[nextLevelNumber - 1] = true;
			PlayerPrefs.SetInt("isLevelAvailable" + nextLevelNumber, 1);
		}
	}

	public void LoadScene(string sceneName)
	{
		StopAllCoroutines();
		this.loadingScreenGO.SetActive(true);
		SceneManager.LoadScene(sceneName);
	}

	public void ResetGameState()
	{
		for (int i = 0; i < this.numLevels; i++)
		{
			this.isLevelAvailable[i] = false;
			this.starsEarnedAtLevel[i] = 0;
			int levelNumber = i + 1;
			PlayerPrefs.SetInt("isLevelAvailable" + levelNumber, 0);
			PlayerPrefs.SetInt("starsEarnedAtLevel" + levelNumber, 0);
		}
		this.isLevelAvailable[0] = true;
		PlayerPrefs.SetInt("isLevelAvailable" + 1, 1);
	}
	
	private void LoadGameState()
	{
		for (int i = 0; i < this.numLevels; i++)
		{
			int levelNumber = i + 1;
			this.isLevelAvailable[i] = PlayerPrefs.GetInt("isLevelAvailable" + levelNumber) == 1;
			this.starsEarnedAtLevel[i] = PlayerPrefs.GetInt("starsEarnedAtLevel" + levelNumber);
		}
	}

}
