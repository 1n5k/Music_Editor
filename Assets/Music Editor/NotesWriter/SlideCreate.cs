using UnityEngine;
using System.Collections;

public class SlideCreate : MonoBehaviour
{
    //これ全部コピペ
    private float mag = 0;
    Mesh _mesh;
    private MeshFilter _meshFilter;
    private Vector3[] _newVertices;

    void Awake()
    {
        //Reference
        _meshFilter = GetComponent<MeshFilter>();
    }

    public void Slider(float x,float y)
    {
        mag = Notescreate.getmag();
        _mesh = new Mesh();
        _newVertices = new Vector3[4];
        Vector2[] _newUV = new Vector2[4];

        //Set vertices
        _newVertices[0] = new Vector3(0, 0);
        _newVertices[1] = new Vector3(-54f * mag , 0);
        _newVertices[2] = new Vector3(x, -y);
        _newVertices[3] = new Vector3(x - (54f * mag), -y);

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
