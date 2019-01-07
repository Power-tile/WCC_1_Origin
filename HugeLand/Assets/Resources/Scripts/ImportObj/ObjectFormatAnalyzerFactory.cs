using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SelfDefine
{
    public static class ObjFormatAnalyzerFactory
    {
        public static GameObject AnalyzeToGameObject(string objFilePath)
        {
            if (!File.Exists(objFilePath)) return null;

            var objFormatAnalyzer = new ObjFormatAnalyzer();

            objFormatAnalyzer.Analyze(File.ReadAllText(objFilePath));

            var go = new GameObject();
            var meshRenderer = go.AddComponent<MeshRenderer>();
            var meshFilter = go.AddComponent<MeshFilter>();

            var mesh = new Mesh();

            var sourceVertexArr = objFormatAnalyzer.VertexArr;
            var sourceUVArr = objFormatAnalyzer.VertexTextureArr;
            var faceArr = objFormatAnalyzer.FaceArr;
            var notQuadFaceArr = objFormatAnalyzer.FaceArr.Where(m => !m.IsQuad).ToArray();
            var quadFaceArr = objFormatAnalyzer.FaceArr.Where(m => m.IsQuad).ToArray();
            var vertexList = new List<Vector3>();
            var uvList = new List<Vector2>();

            var triangles = new int[notQuadFaceArr.Length * 3 + quadFaceArr.Length * 6];
            for (int i = 0, j = 0; i < faceArr.Length; i++)
            {
                var currentFace = faceArr[i];

                triangles[j] = j;
                triangles[j + 1] = j + 1;
                triangles[j + 2] = j + 2;

                var vec = sourceVertexArr[currentFace.Points[0].VertexIndex - 1];
                vertexList.Add(new Vector3(vec.X, vec.Y, vec.Z));

                var uv = sourceUVArr[currentFace.Points[0].TextureIndex - 1];
                uvList.Add(new Vector2(uv.X, uv.Y));

                vec = sourceVertexArr[currentFace.Points[1].VertexIndex - 1];
                vertexList.Add(new Vector3(vec.X, vec.Y, vec.Z));

                uv = sourceUVArr[currentFace.Points[1].TextureIndex - 1];
                uvList.Add(new Vector2(uv.X, uv.Y));

                vec = sourceVertexArr[currentFace.Points[2].VertexIndex - 1];
                vertexList.Add(new Vector3(vec.X, vec.Y, vec.Z));

                uv = sourceUVArr[currentFace.Points[2].TextureIndex - 1];
                uvList.Add(new Vector2(uv.X, uv.Y));

                if (currentFace.IsQuad)
                {
                    triangles[j + 3] = j + 3;
                    triangles[j + 4] = j + 4;
                    triangles[j + 5] = j + 5;
                    j += 3;

                    vec = sourceVertexArr[currentFace.Points[0].VertexIndex - 1];
                    vertexList.Add(new Vector3(vec.X, vec.Y, vec.Z));

                    uv = sourceUVArr[currentFace.Points[0].TextureIndex - 1];
                    uvList.Add(new Vector2(uv.X, uv.Y));

                    vec = sourceVertexArr[currentFace.Points[2].VertexIndex - 1];
                    vertexList.Add(new Vector3(vec.X, vec.Y, vec.Z));

                    uv = sourceUVArr[currentFace.Points[2].TextureIndex - 1];
                    uvList.Add(new Vector2(uv.X, uv.Y));

                    vec = sourceVertexArr[currentFace.Points[3].VertexIndex - 1];
                    vertexList.Add(new Vector3(vec.X, vec.Y, vec.Z));

                    uv = sourceUVArr[currentFace.Points[3].TextureIndex - 1];
                    uvList.Add(new Vector2(uv.X, uv.Y));
                }

                j += 3;
            }

            mesh.vertices = vertexList.ToArray();
            mesh.uv = uvList.ToArray();
            mesh.triangles = triangles;

            meshFilter.mesh = mesh;
            meshRenderer.material = new Material(Shader.Find("Standard"));

            return go;
        }
    }
}