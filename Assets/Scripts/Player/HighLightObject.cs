using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightObject : MonoBehaviour
{
    [Header("Highlight Material")]
    [SerializeField] Material highLight;
    Material baseMaterial;
    //[SerializeField] List<GameObject> touchedObjects = new List<GameObject>();
    [SerializeField] GameObject currentObject;
    bool inRange = false;
    bool isCouroutineActive = false;
    Color matColour;
    [Header("References")]
    gameManager _gameManager;
    Baloon _baloon;
    // Start is called before the first frame update
    void Start()
    {
        baseMaterial = currentObject.GetComponent<SkinnedMeshRenderer>().material;
        matColour = currentObject.GetComponent<SkinnedMeshRenderer>().materials[1].GetColor("_OutlineColor");
        _gameManager = GameObject.FindObjectOfType<gameManager>();
        _baloon = GameObject.FindObjectOfType<Baloon>();
        //currentObject.GetComponent<SkinnedMeshRenderer>().sharedMaterials = new Material[2];
        //currentObject.GetComponent<SkinnedMeshRenderer>().materials[0] = baseMaterial;
        //currentObject.GetComponent<SkinnedMeshRenderer>().materials[1] = baseMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameManager.gameEnded && !_gameManager.gamePaused &&(!_baloon.stuckWithObstacle && !_baloon.landed))
        {
            castRay();
        }
        else if (_gameManager.gameEnded)
        {
            clearMaterials();
        }
        else if (_gameManager.gamePaused)
        {
            SoundManager.SInstance.playSfx(null);
        }

    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position - transform.forward * 150, 150);
    //}    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position - transform.forward * 150, 150);
    //}
    /// <summary>
    /// Casts spherecal ray  to detect obstacles if there is an obstacle within range activates outline and plays danger sound
    /// </summary>
    void castRay()
    {
        RaycastHit hit;

        Vector3 p1 = transform.position;
        //float distanceToObstacle = 0;

        if (Physics.SphereCast(p1, 150, -transform.forward, out hit, 150))
        {
            if (hit.collider.gameObject.CompareTag("Obstacle"))
            {

                currentObject.GetComponent<SkinnedMeshRenderer>().materials[1].SetColor("_OutlineColor", new Color(matColour.r, matColour.g, matColour.b, 1f));
                currentObject.GetComponent<SkinnedMeshRenderer>().materials[1].SetFloat("_Width", 0.04f);

                SoundManager.SInstance.playSfx(SoundManager.SInstance.danger);

                //currentObject.GetComponent<SkinnedMeshRenderer>().material = highLight;
                //Debug.Log("Changing Materials");

                //if(!isCouroutineActive)
                //    StartCoroutine(highLightObject());

                //if (currentObject != null && hit.collider.gameObject != currentObject)
                //{
                //    //touchedObjects.Add(hit.collider.gameObject);
                //    //currentObject.GetComponent<MeshRenderer>().materials[1] = null;
                //    //currentObject = hit.collider.gameObject;
                //    //hit.collider.gameObject.GetComponent<MeshRenderer>().materials[1] = highLight;
                //}
                //else
                //{
                //    //touchedObjects.Add(hit.collider.gameObject);
                //    //currentObject = hit.collider.gameObject;
                //    //hit.collider.gameObject.GetComponent<MeshRenderer>().materials[1] = highLight;
                //}
            }

        }
        else
        {
            clearMaterials();
        }
    }
    /// <summary>
    /// Sets shaders _Width value to -0.02 wich is makes it invisible and cancels danger sound
    /// </summary>
    void clearMaterials()
    {
        //inRange = false;
        //isCouroutineActive = false;
        if (currentObject.GetComponent<SkinnedMeshRenderer>().materials[1].GetColor("_OutlineColor") != new Color(matColour.r, matColour.g, matColour.b, 0f))
        {
            //Debug.Log("Clearing Materials");
            currentObject.GetComponent<SkinnedMeshRenderer>().materials[1].SetColor("_OutlineColor", new Color(matColour.r, matColour.g, matColour.b, 0f));
            currentObject.GetComponent<SkinnedMeshRenderer>().materials[1].SetFloat("_Width", -0.02f);
            SoundManager.SInstance.playSfx(SoundManager.SInstance.avoidDanger);
            //currentObject.GetComponent<SkinnedMeshRenderer>().material = baseMaterial;
        }

        //if(touchedObjects != null)
        //{
        //    foreach (var x in touchedObjects)
        //    {
        //        x.GetComponent<MeshRenderer>().materials[1] = null;
        //    }
        //    touchedObjects.Clear();
        //}

    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Obstacle"))
    //    {
    //        other.gameObject.GetComponent<MeshRenderer>().materials[1] = highLight;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Obstacle"))
    //    {
    //        other.gameObject.GetComponent<MeshRenderer>().materials[1] = null;
    //    }
    //}

    //IEnumerator highLightObject()
    //{
    //        isCouroutineActive = true;
    //        while (inRange)
    //        {
    //            currentObject.GetComponent<SkinnedMeshRenderer>().material = highLight;
    //            yield return new WaitForSeconds(1f);
    //            currentObject.GetComponent<SkinnedMeshRenderer>().material = baseMaterial;
    //        }

    //}
}
