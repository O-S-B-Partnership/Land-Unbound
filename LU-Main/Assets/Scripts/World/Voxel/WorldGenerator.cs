using NoiseUtils;
using UnityEngine;

namespace GameWorld.Voxel
{
	public class WorldGenerator
	{
		float stoneBaseHeight = -24;
		float stoneBaseNoise = 0.05f;
		float stoneBaseNoiseHeight = 4f;
		float stoneMountainHeight = 48f;
		float stoneMountainFrequency = 0.008f;
		float stoneMinHeight = -12;
		float dirtBaseHeight = 1;
		float dirtNoise = 0.04f;
		float dirtNoiseHeight = 3;

		public Chunk ChunkGen (Chunk chunk)
		{
			for (int x = chunk.pos.X; x < chunk.pos.X + Chunk.ChunkSize; x++)
			{
				for (int z = chunk.pos.Z; z < chunk.pos.Z + Chunk.ChunkSize; z++)
				{
					chunk = GetChunkColumn(chunk, x, z);
				}
			}
			return chunk;
		}

		public Chunk GetChunkColumn(Chunk chunk, int x, int z)
		{
			int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
			stoneHeight += GetNoise(x, z, 0, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));
			if (stoneHeight < stoneMinHeight)
			{
				stoneHeight = Mathf.FloorToInt(stoneMinHeight);
			}
			stoneHeight += GetNoise(x, z, 0, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));
			int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
			dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));
			for (int y = chunk.pos.Y; y < chunk.pos.Y + Chunk.ChunkSize; y++)
			{
				if (y <= stoneHeight)
				{
					chunk.SetBlock(x - chunk.pos.X, y - chunk.pos.Y, z - chunk.pos.Z, new Block());
				}
				else if (y <= dirtHeight)
				{
					chunk.SetBlock(x - chunk.pos.X, y - chunk.pos.Y, z - chunk.pos.Z, new GrassBlock());
				}
				else
				{
					chunk.SetBlock(x - chunk.pos.X, y - chunk.pos.Y, z - chunk.pos.Z, new AirBlock());
				}
			}
			return chunk;
		}

		public static int GetNoise (int x, int y, int z, float frequency, int max)
		{
			NoiseCommand command = new NoiseCommand(Noise.noiseMethods[(int)NoiseMethodType.Simplex][2], x, y, z, frequency);

			int result = Mathf.FloorToInt(command.GetNoiseValue() * (max));
			
			return result;
		}
	}
}