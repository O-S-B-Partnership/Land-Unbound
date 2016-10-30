using UnityEngine;
using System.Collections;
using Player;

namespace LandUnbound.GameCamera
{
	public class NavigationFollowCamera : FollowCamera
	{

		[SerializeField]
		private Vector3 _FollowVector;
		public Vector3 FollowVector
		{
			get
			{
				return _FollowVector;
			}
		}

		void Start()
		{
			PlayerCameraNavigation pcn = FocusTarget.GetComponent<PlayerCameraNavigation>();

			if (pcn != null)
			{
				pcn.FocalPoint = GetComponent<Camera>();
			}
		}

		protected override void OnCameraUpdate()
		{
			// Place the camera at it's follow position and update the lookat()
			transform.position = FocusTarget.position + FollowVector;
		}
	}
}
