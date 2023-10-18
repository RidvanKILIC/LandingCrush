using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Variables
    [Header("Camare Follow Variables & Objects")]
    [SerializeField] float cameraRotationLevel;
    public Transform target;
    public bool isCustomOffset;
    public Vector3 startPosition;
    public Quaternion startRotation;
    public Vector3 Inintialoffset;
    public Quaternion initialRot;
    public Vector3 offset;
    public float smoothSpeed = 0.05f;
    [Header("References")]
    gameManager _gameManager;

    #endregion
    private void Start()
    {
        Init();
    }
    /// <summary>
    /// Initialize variables objects and references
    /// </summary>
    void Init()
    {
        _gameManager = GameObject.FindObjectOfType<gameManager>();
        startPosition = new Vector3(target.position.x, startPosition.y, target.position.z);
        if (!isCustomOffset)
        {
            offset = transform.position - target.position;
        }
    }
    private void LateUpdate()
    {
        SmoothFollow();
    }
    /// <summary>
    /// Smoothly follows target with the given distance
    /// </summary>
    public void SmoothFollow()
    {
        //if (!_gameManager.gameStarted && _gameManager.moveCamera)
        //{
        //    changeCamera();
        //}
        //else
        //{
            //if (target.position.y < cameraRotationLevel)
            //{
            //    offset = Vector3.Lerp(offset, Inintialoffset, 0.01f);
            //    transform.rotation = Quaternion.Lerp(transform.rotation, initialRot, 0.008f);
                //transform.LookAt(target);
            //}

            Vector3 targetPos = target.position + offset;
            Vector3 smoothFollow = Vector3.Lerp(transform.position,
             targetPos, smoothSpeed);
            transform.position = new Vector3(smoothFollow.x,transform.position.y,transform.position.z);
        //}


        
        
    }
    void changeCamera()
    {
        Vector3 targetPos = startPosition;
        Vector3 smoothFollow = Vector3.Lerp(transform.position,
         targetPos, 0.02f);
        if (transform.position.y >= targetPos.y-30)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, 0.008f);
            //_gameManager.startGame();
        }
        transform.position = smoothFollow;
    }
}
