using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameWorld.Voxel
{
	class GrassBlock : Block
	{
		public override Tile TexturePosition(Direction direction)
		{
			Tile tile = new Tile();
			switch (direction)
			{
				case Direction.up:
					tile.X = 2;
					tile.Y = 0;
					break;
				case Direction.down:
					tile.X = 1;
					tile.Y = 0;
					break;
				default:
					tile.X = 3;
					tile.Y = 0;
					break;
			}
			return tile;
		}
	}
}
