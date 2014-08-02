using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatePolygonMesh : MonoBehaviour {
  private Mesh mesh;
  // Use this for initialization
  List<Vector3> path = new List<Vector3>();
  List<Vector3> newVertices = new List<Vector3>();
  List<Vector2> newUV = new List<Vector2>();
  List<int> newTriangles = new List<int>();

  //
  int count = 0;
  
  public GameObject cube;

  void Start () {
    mesh = new Mesh();

    
    //初期path
    path.Add(new Vector3(0.0f, 0.0f, 0.0f));
    
    //点の設定
    newVertices.Add(new Vector3(0.0f, 0.0f, 1.0f));
    newVertices.Add(new Vector3(0.0f, 1.0f, 0.0f));
    newVertices.Add(new Vector3(0.0f, 0.8f, -1.0f));
    newVertices.Add(new Vector3(0.0f, -0.8f, -1.0f));
    newVertices.Add(new Vector3(0.0f, -1.0f, 0.0f));

    //UV
    newUV.Add(new Vector2(0.0f, 0.0f));
    newUV.Add(new Vector2(0.0f, 1.0f));
    newUV.Add(new Vector2(1.0f, 1.0f));
    newUV.Add(new Vector2(1.0f, 0.5f));
    newUV.Add(new Vector2(1.0f, 0.0f));

    //Triangles
    newTriangles.Add(1);newTriangles.Add(2);newTriangles.Add(0);
    newTriangles.Add(2);newTriangles.Add(3);newTriangles.Add(0);
    newTriangles.Add(3);newTriangles.Add(4);newTriangles.Add(0);


    mesh.vertices = newVertices.ToArray();
    mesh.uv = newUV.ToArray();
    mesh.triangles = newTriangles.ToArray();
    
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();

    GetComponent<MeshFilter>().sharedMesh = mesh;
    GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
  }
  
  // Update is called once per frame
  void Update () {
    
    if(Input.GetMouseButtonDown(0))
      {
	Vector3 pos = Input.mousePosition;
	pos.z = 10.0f;
	Vector3 newpos = Camera.main.ScreenToWorldPoint(pos);
	cube.transform.position = newpos;
	
      }

        
    //点をウニョウニョ動かす
    float k = 0.001f;
    for(int i=0;i<newVertices.Count;i++)
      {
	Vector3 temp = newVertices[i];
	temp.y += k*Mathf.Sin(Time.time+5*i);
	temp.z += k*Mathf.Cos(Time.time+4*i);
	newVertices[i] = temp;
      }
    
    //動かすかどうか
    if(count<10)
      {


	int pathcount = path.Count-1;
	path.Add(path[pathcount] + new Vector3(1.0f, 0.0f, 0.0f));
    
	//点の設定
	Vector3 delta_vertices = new Vector3(1.0f, 0.0f, 0.0f);
	int vertcount = newVertices.Count-1;
	Debug.Log("vertices" + vertcount);
	newVertices.Add(newVertices[vertcount-4] + delta_vertices);
	newVertices.Add(newVertices[vertcount-3] + delta_vertices);
	newVertices.Add(newVertices[vertcount-2] + delta_vertices);
	newVertices.Add(newVertices[vertcount-1] + delta_vertices);
	newVertices.Add(newVertices[vertcount] + delta_vertices);



    
	//UV
	newUV.Add(new Vector2(0.0f, 0.0f));
	newUV.Add(new Vector2(0.0f, 1.0f));
	newUV.Add(new Vector2(1.0f, 1.0f));
	newUV.Add(new Vector2(1.0f, 0.5f));
	newUV.Add(new Vector2(1.0f, 0.0f));

	//Triangles
	int tricount = newTriangles.Count-1;
	Debug.Log("triangle"+tricount);
	//前2点，後1点
	newTriangles.Add(vertcount-4);newTriangles.Add(vertcount+1);newTriangles.Add(vertcount-3);
	newTriangles.Add(vertcount-3);newTriangles.Add(vertcount+2);newTriangles.Add(vertcount-2);
	newTriangles.Add(vertcount-2);newTriangles.Add(vertcount+3);newTriangles.Add(vertcount-1);
	newTriangles.Add(vertcount-1);newTriangles.Add(vertcount+4);newTriangles.Add(vertcount);
	newTriangles.Add(vertcount);newTriangles.Add(vertcount+5);newTriangles.Add(vertcount-4);
   
	//前1点，後2点
	newTriangles.Add(vertcount+1);newTriangles.Add(vertcount+2);newTriangles.Add(vertcount-3);
	newTriangles.Add(vertcount+2);newTriangles.Add(vertcount+3);newTriangles.Add(vertcount-2);
	newTriangles.Add(vertcount+3);newTriangles.Add(vertcount+4);newTriangles.Add(vertcount-1);
	newTriangles.Add(vertcount+4);newTriangles.Add(vertcount+5);newTriangles.Add(vertcount);
	newTriangles.Add(vertcount+5);newTriangles.Add(vertcount+1);newTriangles.Add(vertcount-4);


      }
    count++;

    Mesh mesh = GetComponent<MeshFilter>().mesh;
    mesh.Clear();
    mesh.vertices = newVertices.ToArray();
    mesh.uv = newUV.ToArray();
    mesh.triangles = newTriangles.ToArray();
    


    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
    
  }
}
