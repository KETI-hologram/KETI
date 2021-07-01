using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rotate : MonoBehaviour
{
    public float RotateSpeed = 120f;
    public float DestroyTime = 3.1f;
    public float Count = 0;

    void Start ()
    {

    }
    // Update is called once per frame
    void Update()
    {
        Count += Time.deltaTime;

        //transform.position = new Vector3(0, 0, 0);
        
        transform.Rotate(new Vector3(0f, RotateSpeed, 0f) * Time.deltaTime);

        if (Count >= DestroyTime){
            Count = 0;
            GameObject.Find("spawn Object").GetComponent<create_object>().Create_new_object();
            Destroy(this.gameObject);

        }
//        transform.rotation = Quaternion.Euler (new Vector3 (10, 0, 0));
    }
}