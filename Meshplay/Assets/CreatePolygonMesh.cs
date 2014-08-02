using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatePolygonMesh : MonoBehaviour {
  private Mesh mesh;
  // Use this for initialization
  Queue<Vector3> path = new Queue<Vector3>();
  Queue<Vector3> newVertices = new Queue<Vector3>();
  Queue<Vector2> newUV = new Queue<Vector2>();
  Queue<int> newTriangles = new Queue<int>();

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
    path.Enqueue(new Vector3(0.0f, 0.0f, 0.0f));
    
    //点の設定
    basevec[0] = new Vector3(0.0f, 0.0f, 1.0f);
    basevec[1] = new Vector3(0.0f, 1.0f, 0.0f);
    basevec[2] = new Vector3(0.0f, 0.8f, -1.0f);
    basevec[3] = new Vector3(0.0f, -0.8f, -1.0f);
    basevec[4] = new Vector3(0.0f, -0.8f, -1.0f);
    

    for(int i=0;i<5;i++)
      {
	newVertices.Enqueue(basevec[i]);
      }

    //UV
    newUV.Enqueue(new Vector2(0.0f, 0.0f));
    newUV.Enqueue(new Vector2(0.0f, 1.0f));
    newUV.Enqueue(new Vector2(1.0f, 1.0f));
    newUV.Enqueue(new Vector2(1.0f, 0.5f));
    newUV.Enqueue(new Vector2(1.0f, 0.0f));

    //Triangles
    /*
    newTriangles.Enqueue(1);newTriangles.Enqueue(2);newTriangles.Enqueue(0);
    newTriangles.Enqueue(2);newTriangles.Enqueue(3);newTriangles.Enqueue(0);
    newTriangles.Enqueue(3);newTriangles.Enqueue(4);newTriangles.Enqueue(0);
    */

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
    
    /*
    //点をウニョウニョ動かす
    float k = 0.001f;
    Vector3[] tmp = newVertices.ToArray();
    for(int i=0;i<tmp.Length;i++)
      {
	tmp[i].y += k*Mathf.Sin(Time.time+5*i);
	tmp[i].z += k*Mathf.Cos(Time.time+4*i);
      }
    newVertices.Clear();
    newVertices = new Queue<Vector3>(tmp);
    */
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
	Quaternion rot = Quaternion.FromToRotation(newpos - RightHand.transform.position, new Vector3(1.0f, 0.0f, 0.0f));
	delta_distance = Vector3.Distance(newpos, RightHand.transform.position);
	//Debug.Log(delta_distance);
	
        newpos = Vector3.Lerp(newpos,RightHand.transform.position,0.1f);
	int pathcount = path.Count-1;
	path.Enqueue(newpos);
    
	//点の設定

	int vertcount = newVertices.Count-1;
	for(int i=0;i<5;i++)
	  {
	    newVertices.Enqueue(rot*(0.1f*basevec[i]) + newpos);
	  }

	//UV
	newUV.Enqueue(new Vector2(0.0f, 0.0f));
	newUV.Enqueue(new Vector2(0.0f, 1.0f));
	newUV.Enqueue(new Vector2(1.0f, 1.0f));
	newUV.Enqueue(new Vector2(1.0f, 0.5f));
	newUV.Enqueue(new Vector2(1.0f, 0.0f));

	//Triangles
	int tricount = newTriangles.Count-1;
	//Debug.Log("triangle"+tricount);
	if(path.Count<=100)
	  {	  
	    //前2点，後1点
	    newTriangles.Enqueue(vertcount-4);newTriangles.Enqueue(vertcount+1);newTriangles.Enqueue(vertcount-3);
	    newTriangles.Enqueue(vertcount-3);newTriangles.Enqueue(vertcount+2);newTriangles.Enqueue(vertcount-2);
	    newTriangles.Enqueue(vertcount-2);newTriangles.Enqueue(vertcount+3);newTriangles.Enqueue(vertcount-1);
	    newTriangles.Enqueue(vertcount-1);newTriangles.Enqueue(vertcount+4);newTriangles.Enqueue(vertcount);
	    newTriangles.Enqueue(vertcount);newTriangles.Enqueue(vertcount+5);newTriangles.Enqueue(vertcount-4);
   
	    //前1点，後2点
	    newTriangles.Enqueue(vertcount+1);newTriangles.Enqueue(vertcount+2);newTriangles.Enqueue(vertcount-3);
	    newTriangles.Enqueue(vertcount+2);newTriangles.Enqueue(vertcount+3);newTriangles.Enqueue(vertcount-2);
	    newTriangles.Enqueue(vertcount+3);newTriangles.Enqueue(vertcount+4);newTriangles.Enqueue(vertcount-1);
	    newTriangles.Enqueue(vertcount+4);newTriangles.Enqueue(vertcount+5);newTriangles.Enqueue(vertcount);
	    newTriangles.Enqueue(vertcount+5);newTriangles.Enqueue(vertcount+1);newTriangles.Enqueue(vertcount-4);
	  }
	else
	  {
	    //順にデキュー
	    path.Dequeue();
	    for(int i=0;i<5;i++)
	      {
		newVertices.Dequeue();
		newUV.Dequeue();
	      }
	  }

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
