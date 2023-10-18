using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

    Mesh mesh;

    List<Quadrilateral> quads;

    public float initialWidth = 16;
    public float initialHeight = 5;
    public int iterations = 1;

    // Start is called before the first frame update
    void Start() {
        GenerateNewMesh();
    }

    // Procedurally generates a new mesh and then sets it to this object's mesh filter component.
    void GenerateNewMesh() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateInitialQuadList();
        RunGeneration(iterations);
        UpdateMesh();
    }

    // test function that creates a list holding one quadrilateral that is the size given by initialWidth and initialHeight
    void GenerateInitialQuadList() {
        quads = new List<Quadrilateral>();
        quads.Add(new Quadrilateral(
            new Vector3(-initialWidth/2,-initialHeight/2,0),
            new Vector3(-initialWidth/2,initialHeight/2,0),
            new Vector3(initialWidth/2,initialHeight/2,0),
            new Vector3(initialWidth/2,-initialHeight/2,0)
        ));
    }

    // runs procedural generation on the face
    // generates # of faces == 2^iterations
    void RunGeneration(int iterations) {
        for(int i = 0; i < iterations; i++) {
            List<Quadrilateral> oldList = new List<Quadrilateral>(quads);
            quads.Clear();

            foreach (Quadrilateral oldQuad in oldList) {
                Debug.Log("quadrilateral split!");
                Vector3 topMidpoint = (oldQuad.v1() + oldQuad.v4()) / 2;
                Vector3 bottomMidpoint = (oldQuad.v2() + oldQuad.v3()) / 2;

                // midpoint displacement goes here

                quads.Add(new Quadrilateral(oldQuad.v1(), oldQuad.v2(), bottomMidpoint, topMidpoint));
                quads.Add(new Quadrilateral(topMidpoint, bottomMidpoint, oldQuad.v3(), oldQuad.v4()));
            }
        }
    }

    // takes the current list of quadrilaterals and generates a mesh from its coordinates.
    void UpdateMesh() {
        mesh.Clear();


        List<Vector3> vertices = new List<Vector3>();
        foreach(Quadrilateral quad in quads) {
            // is there a better way to do this...
            vertices.AddRange(new List<Vector3>(quad.vertices));
        }
        mesh.SetVertices(vertices);


        List<int> indices = new List<int>();
        // because of how the Quadrilateral class is setup, this index list should be sufficient
        for(int i = 0; i < 4 * quads.Count; i++) {
            indices.Add(i);
        }
        mesh.SetIndices(indices, MeshTopology.Quads, 0);


        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }
}

// class representing a quadrilateral
class Quadrilateral {
    public Vector3[] vertices;

    // create a new Quadrilateral
    // vertices are defined like this:
    // v1, v4,
    // v2, v3
    public Quadrilateral(Vector3 v1, Vector3 v2, Vector3 v3, Vector4 v4) {
        vertices = new Vector3[] {
            v1,v2,v3,v4
        };
    }

    public Vector3 v1() {return vertices[0];}
    public Vector3 v2() {return vertices[1];}
    public Vector3 v3() {return vertices[2];}
    public Vector3 v4() {return vertices[3];}
}