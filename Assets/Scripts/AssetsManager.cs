using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsManager : MonoBehaviour
{
	private static AssetsManager instance;

	public static AssetsManager Instance
	{
		get
		{
			if (AssetsManager.instance == null)
			{
				GameObject go = Instantiate(Resources.Load("AssetsManager") as GameObject);
				go.name = "AssetsManager";
				AssetsManager.instance = go.GetComponent<AssetsManager>();
				DontDestroyOnLoad(go);
			}
			return AssetsManager.instance;
		}
	}

	[SerializeField] private Material incorrectMaterial;
	[SerializeField] private Material correctMaterial;

	[SerializeField] private Texture blankStarTexture;
	[SerializeField] private Texture filledStarTexture;
	[SerializeField] private Texture dogeStarTexture;

	public Material IncorrectMaterial { get { return this.incorrectMaterial; } }
	public Material CorrectMaterial { get { return this.correctMaterial; } }

	public Texture BlankStarTexture { get { return this.blankStarTexture; } }
	public Texture FilledStarTexture
	{
		get { return GameManager.Instance.IsDogeMode ? this.dogeStarTexture : this.filledStarTexture; }
	}

}
