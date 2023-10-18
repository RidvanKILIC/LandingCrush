using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directionArrows : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] GameObject leftArrow;
    [SerializeField] GameObject rightArrow;
    [Header("References")]
    [SerializeField] Camera _camera;
    [SerializeField] Plane[] _cameraFrustum;
    [SerializeField] Collider _collider;

    Baloon _baloon;
    // Start is called before the first frame update
    private void Start()
    {
        Init();
        activeDeactiveLeftArrow(false);
        activeDeactiveRightArrow(false);
    }
    void Init()
    {
        _baloon = GameObject.FindObjectOfType<Baloon>();
        //collider = transform.Find("TargetCanvas/Collider").gameObject.GetComponent<Collider>();
        //camera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        var bounds =_collider.bounds;
        _cameraFrustum = GeometryUtility.CalculateFrustumPlanes(_camera);
        if (GeometryUtility.TestPlanesAABB(_cameraFrustum, bounds))
        {
                //Debug.Log("Visible");
                activeDeactiveLeftArrow(false);
                activeDeactiveRightArrow(false);
        }
        else
        {
                //Debug.Log("Invisible");
                checkArrow();
        }
    }
    //public void OnBecameVisible()
    //{
    //    Debug.Log("Visible");
    //    activeDeactiveLeftArrow(false);
    //    activeDeactiveRightArrow(false);

    //}
    //public void OnBecameInvisible()
    //{
    //    Debug.Log("Invisible");
    //    checkArrow();
    //}
    /// <summary>
    /// Check baloon position and decides which arrow should be activated
    /// </summary>
    void checkArrow()
    {
        if (_baloon.gameObject.transform.position.x < _collider.gameObject.transform.position.x)
        {
            activeDeactiveLeftArrow(true);
            activeDeactiveRightArrow(false);
        }
        else if (_baloon.gameObject.transform.position.x > _collider.gameObject.transform.position.x)
        {
            activeDeactiveLeftArrow(false);
            activeDeactiveRightArrow(true);
        }
        else
        {
            activeDeactiveLeftArrow(false);
            activeDeactiveRightArrow(false);
        }

    }

    /// <summary>
    /// Activates or deactivates left arrow object
    /// </summary>
    /// <param name="state"></param>
    void activeDeactiveLeftArrow(bool state)
    {
        leftArrow.SetActive(state);
    }
    /// <summary>
    /// Activates or deactivates right arrow object
    /// </summary>
    /// <param name="state"></param>
    void activeDeactiveRightArrow(bool state)
    {
        rightArrow.SetActive(state);
    }
}
