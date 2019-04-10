using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPiece : MonoBehaviour
{
	[SerializeField] float positionTolerance = 1f;
	[SerializeField] float rotationTolerance = 1f;
	[SerializeField] bool isAnyOrientationOK;

	public bool CheckIsCorrect(Transform pieceTransform)
	{
		return (this.CheckIsPositionCloseEnough(pieceTransform) && this.CheckIsRotationCloseEnough(pieceTransform));
	}

	private bool CheckIsPositionCloseEnough(Transform pieceTransform)
	{
		float distance = Vector3.Distance(this.transform.position, pieceTransform.position);
		return (distance < this.positionTolerance);
	}

	private bool CheckIsRotationCloseEnough(Transform pieceTransform)
	{
		if (this.isAnyOrientationOK)
			return this.CheckIsAnyRotationCloseEnough(pieceTransform);
		else
		{
			float angle = Quaternion.Angle(this.transform.rotation, pieceTransform.rotation);
			return (angle < this.rotationTolerance);
		}
	}

	private bool CheckIsAnyRotationCloseEnough(Transform pieceTransform)
	{
		// Cache original position and rotation, to restore them at the end
		// Some models do not have pivot exactly at center, so rotation slightly moves position as well
		Vector3 startPosition = this.transform.position;
		Quaternion startRotation = this.transform.rotation;

		// Try all 1-degree shifted rotations of the correct transform
		for (int i = 0; i < 360; i++)
		{
			float angle = Quaternion.Angle(this.transform.rotation, pieceTransform.rotation);
			if (angle < this.rotationTolerance)
			{
				this.transform.position = startPosition;
				this.transform.rotation = startRotation;
				return true;
			}
			this.transform.Rotate(0, 0, 1f, Space.World);
		}
		this.transform.position = startPosition;
		this.transform.rotation = startRotation;
		return false;
	}
}
