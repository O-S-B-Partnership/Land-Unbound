
using UnityEngine;

namespace NoiseUtils
{
	public class NoiseCommand
	{
		public int x, y, z;
		public int octaves;
		public float frequency;
		public float lacunarity;
		public float persistance;
		public NoiseMethod method;

		public NoiseCommand() { }

		public NoiseCommand(NoiseMethod method, int x, int y, int z, int octaves, float frequency, float lacunarity, float persistance)
		{
			this.x = x; this.y = y; this.z = z;
			this.octaves = octaves;
			this.frequency = frequency; this.lacunarity = lacunarity; this.persistance = persistance;
			this.method = method;
		}

		public NoiseCommand(NoiseMethod method, int x, int y, int z, float frequency)
		{
			this.x = x; this.y = y; this.z = z;
			this.frequency = frequency;
			this.method = method;
			octaves = 1;
			lacunarity = 2f;
			persistance = 1.0f;
		}

		public NoiseSample GetNoise ()
		{
			Vector3 vector = new Vector3(x, y, z);
			return Noise.Sum(method, vector, frequency, octaves, lacunarity, persistance);
		}

		public float GetNoiseValue ()
		{
			return GetNoise().value;
		}

		public Vector3 GetNoiseVelocity ()
		{
			return GetNoise().derivative;
		}
	}
}
