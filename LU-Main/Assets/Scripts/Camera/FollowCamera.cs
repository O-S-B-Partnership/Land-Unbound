using UnityEngine;
using System;
using System.Collections;

namespace LandUnbound.GameCamera
{
	public abstract class FollowCamera : MonoBehaviour
	{

		[SerializeField]
		private Transform _FocusTarget;
		public Transform FocusTarget
		{
			get
			{
				return _FocusTarget;
			}
			set
			{
				_FocusTarget = value;
				OnFocusTargetChanged();
			}
		}

		private Action _FocusTargetChanged;
		public event Action FocusTargetChanged
		{
			add { _FocusTargetChanged += value; }
			remove { _FocusTargetChanged -= value; }
		}

		protected void OnFocusTargetChanged()
		{
			if (_FocusTargetChanged == null) return;

			_FocusTargetChanged();
		}

		/// <summary>
		/// OnCameraUpdate - called when the camera should be updating.
		/// </summary>
		protected abstract void OnCameraUpdate();

		void LateUpdate()
		{
			OnCameraUpdate();

			transform.LookAt(FocusTarget);
		}
	}
}