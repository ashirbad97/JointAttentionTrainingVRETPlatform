using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField]
    GameObject platform;
    Vector3 platformPos;
    Vector3 platformScale;
    Vector3 startPos;
    float speed=3f;
    // Start is called before the first frame update
    void Start()
    {
        platformPos = platform.transform.position;
        platformScale = platform.transform.localScale;
        platformScale = Vector3.Scale(platformScale, platform.transform.GetComponent<MeshFilter>().mesh.bounds.size);
        print(platformScale+"platform scale");
        startPos = new Vector3(platformPos.x - (platformScale/2).x, platformPos.y, platformPos.z - (platformScale/2).z);
        transform.position = startPos;
        print(startPos);
        StartCoroutine(MoveRight());
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    IEnumerator MoveRight(){
        Vector3 desiredPos = new Vector3(platformScale.x+transform.position.x,transform.position.y,transform.position.z);
        print("desrird pos"+desiredPos);
        while (Vector3.Distance(transform.position,desiredPos)>0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position,desiredPos,speed*Time.deltaTime);
            yield return null;
        }
        StartCoroutine(MoveUp());
    }

    IEnumerator MoveUp(){
        Vector3 desiredPos = new Vector3(transform.position.x,transform.position.y,platformScale.z+transform.position.z);
       print("desrird pos"+desiredPos);
        while (Vector3.Distance(transform.position,desiredPos)>0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position,desiredPos,speed*Time.deltaTime);
            yield return null;
        }
        StartCoroutine(MoveLeft());
    }

    IEnumerator MoveLeft(){
        Vector3 desiredPos = new Vector3(transform.position.x-platformScale.x,transform.position.y,transform.position.z);
        print("desrird pos"+desiredPos);
        while (Vector3.Distance(transform.position,desiredPos)>0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position,desiredPos,speed*Time.deltaTime);
            yield return null;
        }
        StartCoroutine(MoveDown());
    }

    IEnumerator MoveDown(){
        Vector3 desiredPos = new Vector3(transform.position.x,transform.position.y,transform.position.z-platformScale.z);
        print("desrird pos"+desiredPos);
        while (Vector3.Distance(transform.position,desiredPos)>0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position,desiredPos,speed*Time.deltaTime);
            yield return null;
        }
        StartCoroutine(MoveRight());
    }

}
