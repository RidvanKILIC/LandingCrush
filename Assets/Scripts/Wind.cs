using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    GameObject target;
    float speed = 10;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Target");
        Destroy(this.gameObject, 50f);
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Vector3.Distance(transform.position, target.transform.position) > 0)
        //{
        //    transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        //}
        //else
        //{
        //    transform.LookAt(transform.forward);
        //}
        transform.Translate(transform.forward * speed, Space.World);
    }
}
