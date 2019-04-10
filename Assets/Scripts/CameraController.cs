using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private const float KEYBOARD_DEGREES_INCREMENT = 1f;
	private const float MOUSE_DEGREES_INCREMENT = 2f;
	private const float MOVEMENT_INCREMENT = 1f;

	private const float ROTATION_LOWER_LIMIT = -70f;
	private const float ROTATION_UPPER_LIMIT = 70f;

	[SerializeField] private Collider cameraBoundary;
	private float rotateV;

	private void Start()
	{
		this.rotateV = 0f;
	}

	void Update()
	{
		this.HandleMouseInput();
		this.HandleKeyInput();
	}

	private void HandleMouseInput()
	{
		// Must not be holding LMB or RMB
		if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftShift))
		{
			// Rotate camera horizontally
			float rotateHIncrement = Input.GetAxis("Mouse X") * MOUSE_DEGREES_INCREMENT;
			this.transform.Rotate(0, rotateHIncrement, 0, Space.World);
			
			// Rotate camera vertically
			float rotateVIncrement = Input.GetAxis("Mouse Y");
			if (rotateVIncrement > 0 && this.rotateV < ROTATION_UPPER_LIMIT)
			{
				this.rotateV += MOUSE_DEGREES_INCREMENT;
				this.transform.Rotate(-MOUSE_DEGREES_INCREMENT, 0, 0, Space.Self);
			}
			else if (rotateVIncrement < 0 && this.rotateV > ROTATION_LOWER_LIMIT)
			{
				this.rotateV -= MOUSE_DEGREES_INCREMENT;
				this.transform.Rotate(MOUSE_DEGREES_INCREMENT, 0, 0, Space.Self);
			}
		}
	}

	private void HandleKeyInput()
	{
		// Rotate camera horizontally
		if (Input.GetKey(KeyCode.Q))
			this.transform.Rotate(0, -KEYBOARD_DEGREES_INCREMENT, 0, Space.World);
		if (Input.GetKey(KeyCode.E))
			this.transform.Rotate(0, KEYBOARD_DEGREES_INCREMENT, 0, Space.World);

		// Move camera
		if (Input.GetKey(KeyCode.W))
			this.TranslateClamped(0, 0, MOVEMENT_INCREMENT);
		if (Input.GetKey(KeyCode.S))
			this.TranslateClamped(0, 0, -MOVEMENT_INCREMENT);
		if (Input.GetKey(KeyCode.A))
			this.TranslateClamped(-MOVEMENT_INCREMENT, 0, 0);
		if (Input.GetKey(KeyCode.D))
			this.TranslateClamped(MOVEMENT_INCREMENT, 0, 0);
	}

	// Clamp camera position within boundary box
	private void TranslateClamped(float x, float y, float z)
	{
		Vector3 startPosition = this.transform.position;

		this.transform.Translate(x, y, z, Space.Self);
		if (!this.cameraBoundary.bounds.Contains(this.transform.position))
			this.transform.position = startPosition;
	}

}
