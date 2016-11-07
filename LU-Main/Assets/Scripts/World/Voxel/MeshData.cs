using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GameWorld.Voxel
{
	public class MeshData
	{
		public List<Vector3> vertices = new List<Vector3>();
		public List<int> triangles = new List<int>();
		public List<Vector2> uv = new List<Vector2>();
		public List<Vector3> colVertices = new List<Vector3>();
		public List<int> colTriangles = new List<int>();
		public bool UseRenderDataForCol;
		
		public void AddQuadTriangles()
		{
			triangles.Add(vertices.Count - 4);
			triangles.Add(vertices.Count - 3);
			triangles.Add(vertices.Count - 2);
			triangles.Add(vertices.Count - 4);
			triangles.Add(vertices.Count - 2);
			triangles.Add(vertices.Count - 1);
			// If this mesh data is for collision detection, we add all our vertices to the collision vertices.
			if (UseRenderDataForCol)
			{
				colTriangles.Add(colVertices.Count - 4);
				colTriangles.Add(colVertices.Count - 3);
				colTriangles.Add(colVertices.Count - 2);
				colTriangles.Add(colVertices.Count - 4);
				colTriangles.Add(colVertices.Count - 2);
				colTriangles.Add(colVertices.Count - 1);
			}
		}

		public void AddVertex(Vector3 vertex)
		{
			vertices.Add(vertex);
			if (UseRenderDataForCol)
			{
				colVertices.Add(vertex);
			}
		}

		public void AddTriangle(int tri)
		{
			triangles.Add(tri);
			if (UseRenderDataForCol)
			{
				colTriangles.Add(tri - (vertices.Count - colVertices.Count));
			}
		}

		// TODO: Constructor may go here later.
	}
}