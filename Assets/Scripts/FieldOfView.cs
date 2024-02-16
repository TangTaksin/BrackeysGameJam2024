using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class FieldOfView : MonoBehaviour
{
    [SerializeField] float fov = 360;
    [SerializeField] int rayCount = 2;
    
    [SerializeField] float viewDistance = 50;
    [SerializeField] LayerMask mask;

    [SerializeField] Transform originTransform;
    Vector3 origin;

    Mesh fovMesh;

    // Start is called before the first frame update
    void Start()
    {
        fovMesh = new Mesh();

        GetComponent<MeshFilter>().mesh = fovMesh;
    }

    void Update()
    {
        origin = originTransform.position;

        float angle = 0f;
        float angleIncreases = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangle = new int[rayCount * 3];

        vertices[0] = origin; //origin


        int vertexIndex = 1;
        int triangleIndex = 0;

        for ( int i = 0; i <= rayCount; i++ )
        {
            Vector3 vertex;
            var hit = Physics2D.Raycast(origin, VectorFromAngle(angle), viewDistance, mask);
            if (hit)
            {
                vertex = hit.point;
            }
            else
            {
                vertex = origin + VectorFromAngle(angle) * viewDistance;
            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangle[triangleIndex + 0] = 0;
                triangle[triangleIndex + 1] = vertexIndex - 1;
                triangle[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;

            angle -= angleIncreases;
        }

        fovMesh.vertices = vertices;
        fovMesh.uv = uv;
        fovMesh.triangles = triangle;
    }

    Vector3 VectorFromAngle(float angle)
    {
        var angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
