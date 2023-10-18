using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectCollision : MonoBehaviour
{
    Baloon baloon;
    // Start is called before the first frame update
    void Start()
    {
        baloon = GameObject.FindObjectOfType<Baloon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
           baloon.hitObstacle();
        }
    }
}
