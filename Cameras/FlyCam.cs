using UnityEngine;

namespace Elephant.UnityLibrary.Cameras
{
	/// <summary>
	/// Attach this script to your camera to get a fly-cam that can be controlled using WASD, Shift, Spacebar (hold to lock Y-axis) and mouse.
	/// Version 1.01
	/// </summary>
	public class FlyCam : MonoBehaviour
	{
		/// <summary>
		/// Normal speed.
		/// </summary>
		[SerializeField] private float _normalSpeed = 15.0f;

		/// <summary>
		/// Multiplied by how long shift is held. Basically running.
		/// </summary>
		[SerializeField] private float _shiftAdd = 200.0f;

		/// <summary>
		/// The maximum speed when holding shift.
		/// </summary>
		[SerializeField] private float _shiftMax = 1000.0f;

		/// <summary>
		/// Mouse sensitivity.
		/// </summary>
		[SerializeField] private float _mouseSensitivity = 0.1f;

		/// <summary>
		/// Kind of in the middle of the screen, rather than at the top (play).
		/// </summary>
		private Vector3 _lastMousePosition = new Vector3(255, 255, 255);

		private float _totalRun = 1.0f;

		/// <summary>
		/// Set/Get whether input is enabled.
		/// </summary>
		public bool IsInputEnabled = true;

		private void ProcessMouse()
		{
			_lastMousePosition = Input.mousePosition - _lastMousePosition;
			_lastMousePosition = new Vector3(-_lastMousePosition.y * _mouseSensitivity, _lastMousePosition.x * _mouseSensitivity, 0);
			_lastMousePosition = new Vector3(transform.eulerAngles.x + _lastMousePosition.x, transform.eulerAngles.y + _lastMousePosition.y, 0);
			transform.eulerAngles = _lastMousePosition;
			_lastMousePosition = Input.mousePosition;
		}

		/// <summary>
		/// Update.
		/// </summary>
		private void Update()
		{
			if (!IsInputEnabled)
				return;

			ProcessMouse();

			// Process keyboard.
			Vector3 p = GetBaseInput();
			if (Input.GetKey(KeyCode.LeftShift))
			{
				_totalRun += Time.deltaTime;
				p = p * _totalRun * _shiftAdd;
				p.x = Mathf.Clamp(p.x, -_shiftMax, _shiftMax);
				p.y = Mathf.Clamp(p.y, -_shiftMax, _shiftMax);
				p.z = Mathf.Clamp(p.z, -_shiftMax, _shiftMax);
			}
			else
			{
				_totalRun = Mathf.Clamp(_totalRun * 0.5f, 1f, 1000f);
				p = p * _normalSpeed;
			}

			p = p * Time.deltaTime;
			Vector3 newPosition = transform.position;
			if (Input.GetKey(KeyCode.Space))
			{
				// Player wants to move on X and Z axis only (y-axis locked).
				transform.Translate(p);
				newPosition.x = transform.position.x;
				newPosition.z = transform.position.z;
				transform.position = newPosition;
			}
			else
			{
				transform.Translate(p);
			}
		}

		/// <summary>
		/// Returns the basic values, if it's 0 than it's not active.
		/// </summary>
		private Vector3 GetBaseInput()
		{
			Vector3 p_Velocity = Vector3.zero;
			if (Input.GetKey(KeyCode.W))
				p_Velocity += new Vector3(0, 0, 1);
			if (Input.GetKey(KeyCode.S))
				p_Velocity += new Vector3(0, 0, -1);
			if (Input.GetKey(KeyCode.A))
				p_Velocity += new Vector3(-1, 0, 0);
			if (Input.GetKey(KeyCode.D))
				p_Velocity += new Vector3(1, 0, 0);

			return p_Velocity;
		}
	}
}