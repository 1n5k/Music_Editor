using UnityEngine;
using System.Collections;

public class SlideCreate : MonoBehaviour
{
    //これ全部コピペ
    Mesh _mesh;
    private MeshFilter _meshFilter;
    private Vector3[] _newVertices;

    void Awake()
    {
        //Reference
        _meshFilter = GetComponent<MeshFilter>();
    }

    void Start()
    {
        _mesh = new Mesh();
        _newVertices = new Vector3[4];
        Vector2[] _newUV = new Vector2[4];

        //Set vertices
        _newVertices[0] = new Vector3(0, 0, 0);
        _newVertices[1] = new Vector3(1, 0, 0);
        _newVertices[2] = new Vector3(0, 2, 0);
        _newVertices[3] = new Vector3(1, 1, 0);

        //Set UV
        _newUV[0] = new Vector2(0, 0);
        _newUV[1] = new Vector2(1, 0);
        _newUV[2] = new Vector2(0, 1);
        _newUV[3] = new Vector2(1, 1);

        _mesh.vertices = _newVertices;
        _mesh.uv = _newUV;

        //Meshの設定、名前付け
        GetComponent<MeshFilter>().mesh = _mesh;
        GetComponent<MeshFilter>().mesh.name = "LineQuadMesh";

        _meshFilter.mesh.SetIndices(MakeIndices(), MeshTopology.Quads, 0);

        //再計算、必要？SetIndicesの前でも後でも表示は変わらなかった。
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        //MeshTopologyの確認
        MeshTopology topo = _meshFilter.mesh.GetTopology(0);
        Debug.Log(topo); // Quads と出力されるはず
    }

    int[] MakeIndices()
    {
        int[] indices = new int[4];

        indices[0] = 0;
        indices[1] = 2;
        indices[2] = 3;
        indices[3] = 1;

        return indices;
    }
}
