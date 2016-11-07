using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
	}
}
