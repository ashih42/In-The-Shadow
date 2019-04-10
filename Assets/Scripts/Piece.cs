using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
	public const float DEGREES_INCREMENT = 1f;
	public const float MOVEMENT_INCREMENT = 0.1f;
	
	[SerializeField] private TargetPiece[] targetPieces;
	[SerializeField] private MeshRenderer meshRenderer;
	private Material originalMaterial;

	[SerializeField] private bool canRotateHorizontally;
	[SerializeField] private bool canRotateVertically;
	[SerializeField] private bool canMove;

	public bool IsCorrect { get; private set; }

	private void Awake()
	{
		// Cache the material before it is changed in LevelManager.Start()
		this.originalMaterial = this.meshRenderer.material;
	}

	private void Start()
	{
		this.IsCorrect = false;
	}

	private void Update()
	{
		if (!this.IsCorrect)
			this.CheckIsCorrect();
	}

	public void RotateHorizontally(float degrees)
	{
		if (!this.IsCorrect && this.canRotateHorizontally)
			this.transform.Rotate(0, degrees, 0, Space.World);
	}

	public void RotateVertically(float degrees)
	{
		if (!this.IsCorrect && this.canRotateVertically)
			this.transform.Rotate(degrees, 0, 0, Space.World);
	}

	public void Move(float x, float y, float z)
	{
		if (!this.IsCorrect && this.canMove)
			this.transform.Translate(x, y, z, Space.World);
	}

	public void Select()
	{
		this.meshRenderer.material = this.IsCorrect ?
			AssetsManager.Instance.CorrectMaterial : AssetsManager.Instance.IncorrectMaterial;
	}

	public void Unselect()
	{
		this.meshRenderer.material = this.originalMaterial;
	}

	public void ShowSelf()
	{
		this.gameObject.SetActive(true);
	}

	public void HideSelf()
	{
		this.gameObject.SetActive(false);
	}

	public void ShowTarget()
	{
		this.targetPieces[0].gameObject.SetActive(true);
	}

	public void HideTarget()
	{
		this.targetPieces[0].gameObject.SetActive(false);
	}

	private void CheckIsCorrect()
	{
		foreach (TargetPiece targetPiece in this.targetPieces)
		{
			if (targetPiece.CheckIsCorrect(this.transform))
			{
				this.IsCorrect = true;
				this.meshRenderer.material = AssetsManager.Instance.CorrectMaterial;
				AudioManager.Instance.PlaySFX("CorrectPiece");
				StartCoroutine(this.LerpTowardTarget(targetPiece.transform));
				return;
			}
		}
	}

	private IEnumerator LerpTowardTarget(Transform targetTransform)
	{
		float t = 0f;

		Vector3 startPosition = this.transform.position;
		Quaternion startRotation = this.transform.rotation;

		while (t < 1f)
		{
			this.transform.position = Vector3.Lerp(startPosition, targetTransform.position, t);
			this.transform.rotation = Quaternion.Slerp(startRotation, targetTransform.rotation, t);
			t += Time.deltaTime;
			yield return null;
		}
	}

}
