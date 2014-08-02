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

  //vectors
  Vector3[] basevec = new Vector3[5];

  //
  int count = 0;
  public bool mousePointed = false;

  //
  public GameObject RightHand;
  private Vector3 newpos;
  public float delta_distance;

  //timer
  private float timer;
  private float waitingTime=0.01f;


  void Start () {
    mesh = new Mesh();    
    //初期path
    path.Add(new Vector3(0.0f, 0.0f, 0.0f));
    
    //点の設定
    basevec[0] = new Vector3(0.0f, 0.0f, 1.0f);
    basevec[1] = new Vector3(0.0f, 1.0f, 0.0f);
    basevec[2] = new Vector3(0.0f, 0.8f, -1.0f);
    basevec[3] = new Vector3(0.0f, -0.8f, -1.0f);
    basevec[4] = new Vector3(0.0f, -0.8f, -1.0f);
    

    for(int i=0;i<5;i++)
      {
	newVertices.Add(basevec[i]);
      }

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
    

    //点をウニョウニョ動かす
    float k = 0.001f;
    for(int i=0;i<newVertices.Count;i++)
      {
	Vector3 temp = newVertices[i];
	temp.y += k*Mathf.Sin(Time.time+5*i);
	temp.z += k*Mathf.Cos(Time.time+4*i);
	newVertices[i] = temp;
      }
    
    delta_distance = 0.0f;
    

    timer += Time.deltaTime;
    if(timer>waitingTime)
      {
	//アクティブでない場合
	if(!RightHand.activeSelf)
	  {
	    return;
	  }

	//アクティブな場合
	timer = 0;
	delta_distance = Vector3.Distance(newpos, RightHand.transform.position);
	//Debug.Log(delta_distance);
	
        newpos = Vector3.Lerp(newpos,RightHand.transform.position,0.1f);
	int pathcount = path.Count-1;
	path.Add(newpos);
    
	//点の設定

	int vertcount = newVertices.Count-1;
	for(int i=0;i<5;i++)
	  {
	    newVertices.Add(0.1f*basevec[i] + newpos);
	  }

	//UV
	newUV.Add(new Vector2(0.0f, 0.0f));
	newUV.Add(new Vector2(0.0f, 1.0f));
	newUV.Add(new Vector2(1.0f, 1.0f));
	newUV.Add(new Vector2(1.0f, 0.5f));
	newUV.Add(new Vector2(1.0f, 0.0f));

	//Triangles
	int tricount = newTriangles.Count-1;
	//Debug.Log("triangle"+tricount);
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
