using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameWorld.Voxel
{
	public class World : MonoBehaviour
	{
		public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
		public GameObject chunkPrefab;

		void Start()
		{
			for (int x = -2; x < 2; x++)
			{
				for (int y = -1; y < 1; y++)
				{
					for (int z= -1; z < 1; z++)
					{
						CreateChunk(x * 16, y * 16, z * 16);
					}
				}
			}
		}

		/// <summary>
		/// Creates a chunk with a given world position
		/// </summary>
		/// <param name="x">The latitude world position</param>
		/// <param name="y">The altitude world position</param>
		/// <param name="z">The longitude world position</param>
		public void CreateChunk(int x, int y, int z)
		{
			WorldPos worldPos = new WorldPos(x, y, z);
			// Instantiate the chunk at the using the chunk prefab
			GameObject newChunkObject = Instantiate(chunkPrefab, new Vector3(x, y, z), Quaternion.Euler(Vector3.zero)) as GameObject;

			Chunk newChunk = newChunkObject.GetComponent<Chunk>();

			newChunk.pos = worldPos;
			newChunk.world = this;

			chunks.Add(worldPos, newChunk);

			for (int xi = 0; xi < 16; xi++)
			{
				for (int yi = 0; yi < 16; yi++)
				{
					for (int zi = 0; zi < 16; zi++)
					{
						if (yi <= 7)
						{
							SetBlock(x + xi, y + yi, z + zi, new GrassBlock());
						}
						else
						{
							SetBlock(x + xi, y + yi, z + zi, new AirBlock());
						}
					}
				}
			}
		}

		/// <summary>
		/// Find a Chunk given a Block's world coordinates (x, y, z)
		/// </summary>
		/// <param name="x">The target block's X world coordinate</param>
		/// <param name="y">The target block's Y world coordinate</param>
		/// <param name="z">The target block's Z world coordinate</param>
		/// <returns>The Chunk that contains the target block</returns>
		public Chunk GetChunk(int x, int y, int z)
		{
			WorldPos pos = new WorldPos();
			float multiple = Chunk.ChunkSize;
			pos.X = Mathf.FloorToInt(x / multiple) * Chunk.ChunkSize;
			pos.Y = Mathf.FloorToInt(y / multiple) * Chunk.ChunkSize;
			pos.Z = Mathf.FloorToInt(z / multiple) * Chunk.ChunkSize;
			Chunk containerChunk = null;
			chunks.TryGetValue(pos, out containerChunk);

			return containerChunk;
		}

		/// <summary>
		/// Get a block given it's overall world coordinates (x, y, z)
		/// </summary>
		/// <param name="x">The block's world coordinates (x)</param>
		/// <param name="y">The block's world coordinates (y)</param>
		/// <param name="z">The block's world coordinates (z)</param>
		/// <returns>The block that has targetted</returns>
		public Block GetBlock(int x, int y, int z)
		{
			Chunk containerChunk = GetChunk(x, y, z);
			if (containerChunk != null)
			{
				Block block = containerChunk.GetBlock(
					x - containerChunk.pos.X,
					y - containerChunk.pos.Y,
					z - containerChunk.pos.Z);

				return block;
			}
			else
			{
				return new AirBlock();
			}
		}

		/// <summary>
		/// Set a block with a new block.
		/// </summary>
		/// <param name="x">The world x position of the old block.</param>
		/// <param name="y">The world y position of the old block.</param>
		/// <param name="z">The world z position of the old block.</param>
		/// <param name="block">The new block data.</param>
		public void SetBlock(int x, int y, int z, Block block)
		{
			Chunk chunk = GetChunk(x, y, z);

			if (chunk != null)
			{
				chunk.SetBlock(x - chunk.pos.X, y - chunk.pos.Y, z - chunk.pos.Z, block);
				chunk.update = true;
			}
		}

		public void DestroyChunk(int x, int y, int z)
		{
			Chunk chunk = null;
			if (chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
			{
				Object.Destroy(chunk.gameObject);
				chunks.Remove(new WorldPos(x, y, z));
			}
		}
	}
}