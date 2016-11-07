using UnityEngine;
using System.Collections;

namespace GameWorld.Voxel
{
	public class AirBlock : Block
	{
		public override MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
		{
			return meshData;
		}

		public override bool IsSolid(Direction direction)
		{
			return false;
		}
	}
}