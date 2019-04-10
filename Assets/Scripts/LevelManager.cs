using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private int levelNumber;
	[SerializeField] private AudioClip bgmClip;

	[SerializeField] private GameObject hintTextGO;
	[SerializeField] private GameObject menuPanelGO;
	[SerializeField] private Text levelNameText;
	[SerializeField] private RawImage[] starImages;

	[SerializeField] private Piece[] pieces;
	private int pieceIndex;

	private bool usedHint;
	private bool usedPeek;
	private bool isLocked;
	private bool isLevelSolved;
	
	private Piece SelectedPiece
	{
		get { return this.pieces[this.pieceIndex]; }
	}

	private void Start()
	{
		this.pieceIndex = 0;
		this.SelectedPiece.Select();
		this.isLocked = false;
		this.isLevelSolved = false;
		this.usedHint = false;
		this.usedPeek = false;
		this.hintTextGO.SetActive(false);
		this.menuPanelGO.SetActive(false);
		this.levelNameText.text = "Level " + this.levelNumber;
		AudioManager.Instance.PlayBGM(this.bgmClip);
	}

	private void Update()
	{
		if (!this.isLevelSolved)
			this.CheckIsLevelSolved();

		if (!this.isLocked)
		{
			this.HandleKeyInput();
			this.HandleMouseInput();
		}
	}

	private void CheckIsLevelSolved()
	{
		foreach (Piece piece in this.pieces)
			if (!piece.IsCorrect)
				return;

		// Just solved the level!
		this.isLevelSolved = true;
		int stars = this.GetStars();
		GameManager.Instance.SetLevelCleared(this.levelNumber, stars);
		this.menuPanelGO.SetActive(true);
		StartCoroutine(this.ShowStarsCoroutine(stars));
	}

	private int GetStars()
	{
		int stars = 3;

		if (this.usedHint)
			stars--;
		if (this.usedPeek)
			stars--;
		return stars;
	}

	private IEnumerator ShowStarsCoroutine(int stars)
	{
		const int STARS_DEGREES_INCREMENT = 5;

		for (int i = 0; i < stars; i++)
		{
			for (int j = 0; j < 90 / STARS_DEGREES_INCREMENT; j++)
			{
				this.starImages[i].transform.Rotate(0, STARS_DEGREES_INCREMENT, 0, Space.World);
				yield return null;
			}
			this.starImages[i].texture = AssetsManager.Instance.FilledStarTexture;
			for (int j = 0; j < 90 / STARS_DEGREES_INCREMENT; j++)
			{
				this.starImages[i].transform.Rotate(0, -STARS_DEGREES_INCREMENT, 0, Space.World);
				yield return null;
			}
			AudioManager.Instance.PlaySFX("Star");
			yield return new WaitForSeconds(0.5f);
		}
	}

	public void ToggleMenu()
	{
		this.menuPanelGO.SetActive(!this.menuPanelGO.activeInHierarchy);
	}

	public void RetryLevel()
	{
		GameManager.Instance.LoadScene("Level " + this.levelNumber);
	}

	public void GoToLevelSelectionScreen()
	{
		GameManager.Instance.LoadScene("Level Selection Screen");
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void ShowHint()
	{
		if (!this.usedHint && !this.isLocked)
		{
			this.usedHint = true;
			this.hintTextGO.SetActive(true);
			AudioManager.Instance.PlaySFX("Hint");
		}
	}

	public void PeekAnswer()
	{
		if (!this.isLocked)
		{
			this.usedPeek = true;
			AudioManager.Instance.PlaySFX("Peek");
			StartCoroutine(this.ShowHintCoroutine());
		}
	}

	// blink the correct shadows 3 times
	private IEnumerator ShowHintCoroutine()
	{
		this.isLocked = true;
		for (int i = 0; i < 3; i++)
		{
			foreach (Piece piece in this.pieces)
			{
				piece.HideSelf();
				piece.ShowTarget();
			}
			yield return new WaitForSeconds(0.5f);
			foreach (Piece piece in this.pieces)
			{
				piece.HideTarget();
				piece.ShowSelf();
			}
			yield return new WaitForSeconds(0.5f);
		}
		this.isLocked = false;
	}

	private void SelectNextPiece()
	{
		this.SelectedPiece.Unselect();
		this.pieceIndex = (this.pieceIndex + 1) % this.pieces.Length;
		this.SelectedPiece.Select();
	}

	private void SelectPreviousPiece()
	{
		this.SelectedPiece.Unselect();
		if (--this.pieceIndex < 0)
			this.pieceIndex += this.pieces.Length;
		this.SelectedPiece.Select();
	}

	private void HandleKeyInput()
	{
		// Select next/previous piece
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (Input.GetKey(KeyCode.LeftShift))
				this.SelectNextPiece();
			else
				this.SelectPreviousPiece();
		}

		if (Input.GetKey(KeyCode.LeftShift))
		{
			// Move horizontally
			if (Input.GetKey(KeyCode.LeftArrow))
				this.SelectedPiece.Move(-Piece.MOVEMENT_INCREMENT, 0, 0);
			if (Input.GetKey(KeyCode.RightArrow))
				this.SelectedPiece.Move(Piece.MOVEMENT_INCREMENT, 0, 0);
			// Move vertically
			if (Input.GetKey(KeyCode.UpArrow))
				this.SelectedPiece.Move(0, Piece.MOVEMENT_INCREMENT, 0);
			if (Input.GetKey(KeyCode.DownArrow))
				this.SelectedPiece.Move(0, -Piece.MOVEMENT_INCREMENT, 0);
		}
		else
		{
			// Rotate horizontally
			if (Input.GetKey(KeyCode.LeftArrow))
				this.SelectedPiece.RotateHorizontally(Piece.DEGREES_INCREMENT);
			if (Input.GetKey(KeyCode.RightArrow))
				this.SelectedPiece.RotateHorizontally(-Piece.DEGREES_INCREMENT);
			// Rotate vertically
			if (Input.GetKey(KeyCode.UpArrow))
				this.SelectedPiece.RotateVertically(Piece.DEGREES_INCREMENT);
			if (Input.GetKey(KeyCode.DownArrow))
				this.SelectedPiece.RotateVertically(-Piece.DEGREES_INCREMENT);
		}
	}

	private void HandleMouseInput()
	{
		// Select next/previous piece
		float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
		if (scrollWheelInput > 0)
			this.SelectNextPiece();
		else if (scrollWheelInput < 0)
			this.SelectPreviousPiece();

		// RMB Down => Movement
		if (Input.GetMouseButton(1))
		{
			// Move horizontally
			float moveXIncrement = Input.GetAxis("Mouse X") * Piece.MOVEMENT_INCREMENT * 2;
			this.SelectedPiece.Move(moveXIncrement, 0, 0);
			// Move vertically
			float moveYIncrement = Input.GetAxis("Mouse Y") * Piece.MOVEMENT_INCREMENT * 2;
			this.SelectedPiece.Move(0, moveYIncrement, 0);
		}
		// LMB Down => Rotate
		else if (Input.GetMouseButton(0))
		{
			// Rotate horizontally
			float rotateHIncrement = Input.GetAxis("Mouse X") * -Piece.DEGREES_INCREMENT * 2;
			this.SelectedPiece.RotateHorizontally(rotateHIncrement);
			// Rotate vertically
			float rotateVIncrement = Input.GetAxis("Mouse Y") * Piece.DEGREES_INCREMENT * 2;
			this.SelectedPiece.RotateVertically(rotateVIncrement);
		}

		// Other controls
		if (Input.GetKeyDown(KeyCode.H))
			this.ShowHint();
		if (Input.GetKeyDown(KeyCode.P))
			this.PeekAnswer();
		if (Input.GetKeyDown(KeyCode.Backspace))
			this.GoToLevelSelectionScreen();
		if (Input.GetKeyDown(KeyCode.Escape))
			this.ToggleMenu();
	}

}
