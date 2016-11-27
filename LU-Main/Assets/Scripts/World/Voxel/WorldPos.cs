using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameWorld.Voxel
{
	public struct WorldPos
	{
		public int X, Y, Z;
		public WorldPos(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is WorldPos))
				return false;

			WorldPos pos = (WorldPos)obj;
			if (pos.X != X || pos.Y != Y || pos.Z != Z)
			{
				return false;
			}

			return true;
		}

		public static WorldPos operator + (WorldPos a, WorldPos b)
		{
			a.X += b.X;
			a.Y += b.Y;
			a.Z += b.Z;
			return a;
		}

		public static WorldPos operator + (WorldPos a, Vector3 b)
		{
			a.X += Mathf.FloorToInt(b.x);
			a.Y += Mathf.FloorToInt(b.y);
			a.Z += Mathf.FloorToInt(b.z);
			return a;
		}

		public static WorldPos operator + (Vector3 a, WorldPos b)
		{
			return b + a;
		}

		public static WorldPos operator * (WorldPos a, int b)
		{
			a.X *= b;
			a.Y *= b;
			a.Z *= b;
			return a;
		}

		public static WorldPos operator * (int a, WorldPos b)
		{
			return b * a;
		}
	}
}
