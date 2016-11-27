using UnityEngine;
using System.Collections;

namespace GameWorld.Voxel
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshCollider))]
	public class Chunk : MonoBehaviour
	{
		#region Public Static
		public static readonly int ChunkSize = 16;
		#endregion Public Static

		#region Public Members
		public bool update = false;
		public World world;
		public WorldPos pos;
		public bool rendered;
		#endregion

		#region Private Members
		Block[,,] _Blocks = new Block[ChunkSize, ChunkSize, ChunkSize];
		#endregion

		private MeshFilter _filter;
		private MeshFilter filter
		{
			get
			{
				if (_filter == null)
				{
					_filter = gameObject.GetComponent<MeshFilter>();
				}

				return _filter;
			}
		}

		private MeshCollider _mcollider;
		private MeshCollider mcollider
		{
			get
			{
				if (_mcollider == null)
				{
					_mcollider = GetComponent<MeshCollider>();
				}

				return _mcollider;
			}
		}

		public static bool InRange(int index)
		{
			if (index < 0 || index >= ChunkSize)
			{
				return false;
			}
			return true;
		}

		void Update()
		{
			if (update)
			{
				update = false;
				UpdateChunk();
			}
		}

		public Block GetBlock(int x, int y, int z)
		{
			if (InRange(x) && InRange(y) && InRange(z))
			{
				return _Blocks[x, y, z];
			}
			return world.GetBlock(pos.X + x, pos.Y + y, pos.Z + z);
		}

		public void SetBlock(int x, int y, int z, Block block)
		{
			if (InRange(x) && InRange(y) && InRange(z))
			{
				_Blocks[x, y, z] = block;
			}
			else
			{
				world.SetBlock(pos.X + x, pos.Y + y, pos.Z + z, block);
			}
		}

		// Updates the chunk based on its contents
		void UpdateChunk()
		{
			rendered = true;
			MeshData meshData = new MeshData();
			for (int x = 0; x < ChunkSize; x++)
			{
				for (int y = 0; y < ChunkSize; y++)
				{
					for (int z = 0; z < ChunkSize; z++)
					{
						meshData = _Blocks[x, y, z].Blockdata(this, x, y, z, meshData);
					}
				}
			}
			RenderMesh(meshData);
		}

		// Sends the calculated mesh information
		// to the mesh and collision components
		void RenderMesh(MeshData meshData)
		{
			filter.mesh.Clear();
			filter.mesh.vertices = meshData.vertices.ToArray();
			filter.mesh.triangles = meshData.triangles.ToArray();

			// Manage uvs
			filter.mesh.uv = meshData.uv.ToArray();
			filter.mesh.RecalculateNormals();

			// Add to collider
			mcollider.sharedMesh = null;
			Mesh mesh = new Mesh();
			mesh.vertices = meshData.colVertices.ToArray();
			mesh.triangles = meshData.colTriangles.ToArray();
			mesh.RecalculateNormals();

			mcollider.sharedMesh = mesh;
		}
	}
}