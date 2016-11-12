using UnityEngine;
using System.Collections;
using System;

namespace GameWorld.Voxel
{
	public class Block
	{

		public struct Tile
		{
			public static readonly float TileSize = 0.25f;
			public int X; public int Y;
		}
		

		protected static readonly float scale = 0.5f;

		// May have constructor here later.
		
		public virtual MeshData Blockdata (Chunk chunk, int x, int y, int z, MeshData meshData)
		{
			meshData.UseRenderDataForCol = true;
			if (!IsNeighborBlockSolid(chunk, x, y + 1, z, Direction.down))
			{
				meshData = FaceDataUp(chunk, x, y, z, meshData);
			}

			if (!IsNeighborBlockSolid(chunk, x, y - 1, z, Direction.up))
			{
				meshData = FaceDataDown(chunk, x, y, z, meshData);
			}

			if (!IsNeighborBlockSolid(chunk, x, y, z + 1, Direction.south))
			{
				meshData = FaceDataNorth(chunk, x, y, z, meshData);
			}

			if (!IsNeighborBlockSolid(chunk, x, y, z - 1, Direction.north))
			{
				meshData = FaceDataSouth(chunk, x, y, z, meshData);
			}

			if (!IsNeighborBlockSolid(chunk, x + 1, y, z, Direction.west))
			{
				meshData = FaceDataEast(chunk, x, y, z, meshData);
			}

			if (!IsNeighborBlockSolid(chunk, x - 1, y, z, Direction.east))
			{
				meshData = FaceDataWest(chunk, x, y, z, meshData);
			}

			return meshData;
		}

		public virtual bool IsSolid(Direction direction)
		{
			switch(direction)
			{
				case Direction.north:
				case Direction.south:
				case Direction.west:
				case Direction.east:
				case Direction.up:
				case Direction.down:
					return true;
				default:
					throw new Exception(String.Format("Incorrect data sent to direction [{0}]", direction));
			}
		}

		public virtual Tile TexturePosition(Direction direction)
		{
			Tile tile = new Tile();
			tile.X = 0;
			tile.Y = 0;
			return tile;
		}

		public virtual Vector2[] FaceUVs(Direction direction)
		{
			Vector2[] UVs = new Vector2[4];
			Tile tilePos = TexturePosition(direction);
			UVs[0] = new Vector2(Tile.TileSize * tilePos.X + Tile.TileSize, Tile.TileSize * tilePos.Y);
			UVs[1] = new Vector2(Tile.TileSize * tilePos.X + Tile.TileSize, Tile.TileSize * tilePos.Y + Tile.TileSize);
			UVs[2] = new Vector2(Tile.TileSize * tilePos.X, Tile.TileSize * tilePos.Y + Tile.TileSize);
			UVs[3] = new Vector2(Tile.TileSize * tilePos.X, Tile.TileSize * tilePos.Y);
			return UVs;
		}

		#region Face Data
		protected virtual MeshData FaceDataUp (Chunk chunk, int x, int y, int z, MeshData meshData)
		{
			meshData.AddVertex(new Vector3(x - scale, y + scale, z + scale));
			meshData.AddVertex(new Vector3(x + scale, y + scale, z + scale));
			meshData.AddVertex(new Vector3(x + scale, y + scale, z - scale));
			meshData.AddVertex(new Vector3(x - scale, y + scale, z - scale));
			meshData.AddQuadTriangles();
			meshData.uv.AddRange(FaceUVs(Direction.up));
			return meshData;
		}

		protected virtual MeshData FaceDataDown (Chunk chunk, int x, int y, int z, MeshData meshData)
		{
			meshData.AddVertex(new Vector3(x - scale, y - scale, z - scale));
			meshData.AddVertex(new Vector3(x + scale, y - scale, z - scale));
			meshData.AddVertex(new Vector3(x + scale, y - scale, z + scale));
			meshData.AddVertex(new Vector3(x - scale, y - scale, z + scale));
			meshData.AddQuadTriangles();
			meshData.uv.AddRange(FaceUVs(Direction.down));
			return meshData;
		}

		protected virtual MeshData FaceDataNorth (Chunk chunk, int x, int y, int z, MeshData meshData)
		{
			meshData.AddVertex(new Vector3(x + scale, y - scale, z + scale));
			meshData.AddVertex(new Vector3(x + scale, y + scale, z + scale));
			meshData.AddVertex(new Vector3(x - scale, y + scale, z + scale));
			meshData.AddVertex(new Vector3(x - scale, y - scale, z + scale));
			meshData.AddQuadTriangles();
			meshData.uv.AddRange(FaceUVs(Direction.north));
			return meshData;
		}

		protected virtual MeshData FaceDataSouth (Chunk chunk, int x, int y, int z, MeshData meshData)
		{
			meshData.AddVertex(new Vector3(x - scale, y - scale, z - scale));
			meshData.AddVertex(new Vector3(x - scale, y + scale, z - scale));
			meshData.AddVertex(new Vector3(x + scale, y + scale, z - scale));
			meshData.AddVertex(new Vector3(x + scale, y - scale, z - scale));
			meshData.AddQuadTriangles();
			meshData.uv.AddRange(FaceUVs(Direction.south));
			return meshData;
		}

		protected virtual MeshData FaceDataWest (Chunk chunk, int x, int y, int z, MeshData meshData)
		{
			meshData.AddVertex(new Vector3(x - scale, y - scale, z + scale));
			meshData.AddVertex(new Vector3(x - scale, y + scale, z + scale));
			meshData.AddVertex(new Vector3(x - scale, y + scale, z - scale));
			meshData.AddVertex(new Vector3(x - scale, y - scale, z - scale));

			meshData.AddQuadTriangles();
			meshData.uv.AddRange(FaceUVs(Direction.west));
			return meshData;
		}

		protected virtual MeshData FaceDataEast (Chunk chunk, int x, int y, int z, MeshData meshData)
		{
			meshData.AddVertex(new Vector3(x + scale, y - scale, z - scale));
			meshData.AddVertex(new Vector3(x + scale, y + scale, z - scale));
			meshData.AddVertex(new Vector3(x + scale, y + scale, z + scale));
			meshData.AddVertex(new Vector3(x + scale, y - scale, z + scale));

			meshData.AddQuadTriangles();
			meshData.uv.AddRange(FaceUVs(Direction.east));
			return meshData;
		}
		#endregion

		protected bool IsNeighborBlockSolid(Chunk chunk, int x, int y, int z, Direction direction)
		{
			return chunk.GetBlock(x, y, z).IsSolid(direction);
		}
	}
}