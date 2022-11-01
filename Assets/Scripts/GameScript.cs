using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    // [SerializeField]
    // GameObject gameObject;
    Vector3 pos;
    Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        target = new Vector3(10,0,5);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (target*Time.deltaTime);
    }
}
