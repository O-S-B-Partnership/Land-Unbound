using UnityEngine;
using System.Collections;

namespace Player
{
	public class PlayerCameraNavigation : MonoBehaviour
	{

		[SerializeField]
		private float _LandSpeed;

		private Camera _FocalPoint;
		public Camera FocalPoint
		{
			get { return _FocalPoint; }
			set
			{
				_FocalPoint = value;
			}
		}

		private Vector3 ControlForward
		{
			get
			{
				// TODO: determine Forward (depth)
				Vector3 forwardVector = transform.forward;

				if (FocalPoint != null)
				{
					forwardVector = FocalPoint.transform.forward;

					forwardVector.y = 0;
					Vector3.Normalize(forwardVector);
				}

				return forwardVector;
			}
		}

		private Vector3 ControlRight
		{
			get
			{
				// TODO: determine Right (across camera)
				Vector3 rightVector = transform.right;

				if (FocalPoint != null)
				{
					rightVector = FocalPoint.transform.right;
					rightVector.y = 0;
					Vector3.Normalize(rightVector);
				}

				return rightVector;
			}
		}

		void Update()
		{
			Vector3 controlDirection = HandleInput();

			HandleFacing(controlDirection);

			transform.position += controlDirection;
		}

		private Vector3 HandleInput()
		{
			Vector3 result = Vector3.zero;

			if (Input.GetKey(KeyCode.W))
			{
				result += ControlForward * _LandSpeed * Time.deltaTime;
			}
			else if (Input.GetKey(KeyCode.S))
			{
				result -= ControlForward * _LandSpeed * Time.deltaTime;
			}

			if (Input.GetKey(KeyCode.A))
			{
				result -= ControlRight * _LandSpeed * Time.deltaTime;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				result += ControlRight * _LandSpeed * Time.deltaTime;
			}

			return result;
		}

		private void HandleFacing (Vector3 direction)
		{
			transform.LookAt(transform.position + direction);
		}
	}
}