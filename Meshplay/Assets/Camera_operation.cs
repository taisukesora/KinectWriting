using UnityEngine;
using System.Collections;

public class Camera_operation : MonoBehaviour {
  CreatePolygonMesh script;
  float x;
  float k;
  // Use this for initialization
  void Start () {
    script = GameObject.Find("Drawing").GetComponent<CreatePolygonMesh>();
  }
  
  // Update is called once per frame
  void Update () {
    x = (x+script.delta_distance)/2.0f;
    k = (0.3f*Mathf.Exp(-x/2.0f) + k)/2.0f;
    Debug.Log(k);
    
    /*
    transform.position = new Vector3(Mathf.Sin(Time.time/k), 5.0f/20.0f, Mathf.Cos(Time.time/k));
    transform.position *=20.0f;
	  
    transform.LookAt(Vector3.zero);
    */
    transform.RotateAround(Vector3.zero, Vector3.up, k);
  }
}
