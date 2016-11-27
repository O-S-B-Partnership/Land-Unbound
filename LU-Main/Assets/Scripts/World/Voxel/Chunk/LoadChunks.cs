using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameWorld.Voxel.ChunkUtils
{
	public class LoadChunks : MonoBehaviour
	{
		static List<WorldPos> chunkPositions = null;

		static bool AddSearchPos(WorldPos pos)
		{
			if (chunkPositions.Contains(pos)) return false;

			chunkPositions.Add(pos);
			return true;
		}

		static void GenerateSearchPattern(int maxSteps)
		{
			chunkPositions = new List<WorldPos>();

			chunkPositions.Add(new WorldPos(0, 0, 0));

			List<WorldPos> lastStepPositions = new List<WorldPos>();
			lastStepPositions.Add(new WorldPos(0, 0, 0));

			for (int x = 0; x < maxSteps; x++)
			{
				List<WorldPos> currentStepPos = new List<WorldPos>();
				for (int stepI = 0; stepI < lastStepPositions.Count; stepI++)
				{
					WorldPos currPosition = lastStepPositions[stepI];
					if (AddSearchPos(new WorldPos(currPosition.X - 1, 0, currPosition.Z))) currentStepPos.Add(new WorldPos(currPosition.X - 1, 0, currPosition.Z));
					if (AddSearchPos(new WorldPos(currPosition.X + 1, 0, currPosition.Z))) currentStepPos.Add(new WorldPos(currPosition.X + 1, 0, currPosition.Z));
					if (AddSearchPos(new WorldPos(currPosition.X, 0, currPosition.Z - 1))) currentStepPos.Add(new WorldPos(currPosition.X, 0, currPosition.Z - 1));
					if (AddSearchPos(new WorldPos(currPosition.X, 0, currPosition.Z + 1))) currentStepPos.Add(new WorldPos(currPosition.X, 0, currPosition.Z + 1));
				}
				lastStepPositions = currentStepPos;
			}
		}

		public World world;
		public int genStepsOut = 6;
		public int deleteTimeout = 10;

		int timer = 0;

		List<WorldPos> updateList = new List<WorldPos>();
		List<WorldPos> buildList = new List<WorldPos>();

		void Start()
		{
			if (chunkPositions == null)
			{
				GenerateSearchPattern(genStepsOut);
			}
		}

		void Update()
		{
			DeleteChunks();
			FindChunksToLoad();
			LoadAndRenderChunks();
		}

		void FindChunksToLoad()
		{
			WorldPos playerPos = new WorldPos
			(
				Mathf.FloorToInt(transform.position.x / Chunk.ChunkSize) * Chunk.ChunkSize,
				Mathf.FloorToInt(transform.position.y / Chunk.ChunkSize) * Chunk.ChunkSize,
				Mathf.FloorToInt(transform.position.z / Chunk.ChunkSize) * Chunk.ChunkSize
			);
			if (buildList.Count == 0)
			{
				for (int i = 0; i < chunkPositions.Count; i++)
				{
					WorldPos newChunkPos = chunkPositions[i] * Chunk.ChunkSize + playerPos;
					newChunkPos.Y = 0;
					Chunk newChunk = world.GetChunk(
						newChunkPos.X, newChunkPos.Y, newChunkPos.Z);

					if (newChunk != null && (newChunk.rendered || updateList.Contains(newChunkPos)))
					{
						continue;
					}

					for (int y = -4; y < 4; y++)
					{
						buildList.Add(new WorldPos(newChunkPos.X, y * Chunk.ChunkSize, newChunkPos.Z));
					}
					return;
				}
			}
		}

		void BuildChunk(WorldPos pos)
		{
			for (int y = pos.Y - Chunk.ChunkSize; y <= pos.Y + Chunk.ChunkSize; y += Chunk.ChunkSize)
			{
				if (y > 64 || y < -64)
					continue;
				for (int x = pos.X - Chunk.ChunkSize; x <= pos.X + Chunk.ChunkSize; x += Chunk.ChunkSize)
				{
					for (int z = pos.Z - Chunk.ChunkSize; z <= pos.Z + Chunk.ChunkSize; z += Chunk.ChunkSize)
					{
						if (world.GetChunk(x, y, z) == null)
						{
							world.CreateChunk(x, y, z);
						}
					}
				}
			}
			updateList.Add(pos);
		}

		void LoadAndRenderChunks()
		{
			for (int i = 0; i < 4; i++)
			{
				if (buildList.Count != 0)
				{
					BuildChunk(buildList[0]);
					buildList.RemoveAt(0);
				}
			}

			for (int i = 0; i < updateList.Count; i++)
			{
				Chunk chunk = world.GetChunk(updateList[0].X, updateList[0].Y, updateList[0].Z);
				if (chunk != null)
				{
					chunk.update = true;
				}
				updateList.RemoveAt(0);
			}
		}

		void DeleteChunks()
		{
			if (timer == deleteTimeout)
			{
				List<WorldPos> chunksToDelete = new List<WorldPos>();
				foreach (KeyValuePair<WorldPos, Chunk> chunk in world.chunks)
				{
					float distance = Vector3.Distance(
						new Vector3(chunk.Value.pos.X, 0, chunk.Value.pos.Z),
						new Vector3(transform.position.x, 0, transform.position.z));
					if (distance > 256)
						chunksToDelete.Add(chunk.Key);
				}
				foreach (WorldPos chunk in chunksToDelete)
				{
					world.DestroyChunk(chunk.X, chunk.Y, chunk.Z);
				}
				timer = 0;
			}
			timer++;
		}
	}
}
