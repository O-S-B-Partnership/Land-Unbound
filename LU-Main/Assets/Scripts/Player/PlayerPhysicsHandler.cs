using UnityEngine;
using System.Collections;
using System;

namespace Player
{
	[Serializable]
	public class PlayerPhysicsHandler
	{
		
		private double lastFrameY = 0.0;
		public static double fallRate = .05;
		public static double terminal = .50;

		public bool applyGravity = true;

		public bool CanTravel(Vector3 position, Vector3 direction)
		{
			RaycastHit hitData;
			if (Physics.Raycast(new Ray(position, direction), out hitData, direction.magnitude * 2))
			{
				return false;
			}

			return true;
		}

		public Vector3 ApplyGravity(Vector3 position, Vector3 direction)
		{
			if (applyGravity)
			{
				if (CanTravel(position, new Vector3(0, -(float)terminal, 0)))
				{
					lastFrameY -= fallRate;
					direction.y = (float)lastFrameY;
					if (lastFrameY > terminal)
					{
						lastFrameY = terminal;
					}
					return direction;
				}
				else
				{
					lastFrameY = 0.0;
					return direction;
				}
			}

			return Vector3.zero;
		}
	}
}