using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baloon : MonoBehaviour
{
    #region Variables
    [Header("Baloon Variables")]
    [SerializeField] GameObject groundHightReference;
    [SerializeField] GameObject playerHeightReference;
    [SerializeField] GameObject speedFx;
    [SerializeField] GameObject fireFx;
    [SerializeField] GameObject expFx;
    [SerializeField] GameObject expPos;
    [SerializeField] public float Maxfuel;
    [SerializeField] public float fuel;
    [SerializeField] bool stopCalculatingStatus;
    float height;
    float _speed;
    float maxSpeed;
    float lastSpeed;
    float maxHeight;
    public bool landed=false;
    public bool stuckWithObstacle = false;
    [SerializeField]List<GameObject> collisions = new List<GameObject>();
    [Header("Objects & Variables")]
    [SerializeField]GameObject shadow;
    [SerializeField] List<GameObject> targets = new List<GameObject>();
    [Header("References")]
    public Rigidbody rb;
    gameManager _gameManager;
    public BaloonMovement _baloonMovement;

    #endregion
    private void Awake()
    {
        Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Init(); 
    }
    /// <summary>
    /// Initiliazes variables and references
    /// </summary>
    void Init()
    {
         
        Maxfuel = fuel;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        _gameManager = GameObject.FindObjectOfType<gameManager>();
        _baloonMovement = GetComponent<BaloonMovement>();
        calculateHeight();
        calculateSpeed();
        calculateMaxSpeed();
        activateDeactivateSpeedFx(0);
        activateDeactivatefireFx(0);
    }
    // Update is called once per frame
    void Update()
    {
        if (!stopCalculatingStatus)
        {
            calculateHeight();
            calculateSpeed();
        }
        //castShadow();

    }
    /// <summary>
    /// Sends ray to the down and spawn shadow object on the position of the hit point
    /// </summary>
    void castShadow()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, Vector3.down, out hit);
        if (hit.collider.gameObject.CompareTag("Ground") || hit.collider.gameObject.CompareTag("Target"))
        {
            if (!shadow.activeSelf)
            {
                shadow.SetActive(true);
            }
            shadow.transform.position = hit.point;
            //Debug.Log(hit.normal);
            //shadow.transform.LookAt(hit.normal);
        }
        else
        {
            shadow.SetActive(false);
        }

    }
    /// <summary>
    /// Calculates height from ground to player
    /// </summary>
    void calculateHeight()
    {
       //setHeight(Vector3.Distance(playerHeightReference.transform.position, groundHightReference.transform.position));
       setHeight(transform.position.y);
    }
    /// <summary>
    /// Assigns Rigidbody's speed to _speed variable
    /// </summary>
    void calculateSpeed()
    {
        setSpeed(_speed = new Vector3(0,rb.velocity.y,0).magnitude);
        //Debug.Log("Speed: " + getSpeed());
        //calculateMaxSpeed();
    }
    void calculateMaxSpeed()
    {
        // The force you add each FixedUpdate using ForceMode.Acceleration or ForceMode.Force with mass 1
        Vector3 addedForce;

        // The top speed in units per second
        float topSpeed;

        // The equation
        topSpeed = Mathf.Sqrt(((Vector3.up * Time.fixedDeltaTime * _baloonMovement.breakSensitivity).magnitude) / rb.drag) / Time.fixedDeltaTime;

        float topVelocity = (((Vector3.up * Time.fixedDeltaTime * _baloonMovement.breakSensitivity).magnitude / rb.drag) - Time.fixedDeltaTime * (Vector3.up * Time.fixedDeltaTime * _baloonMovement.breakSensitivity).magnitude) / rb.mass;
        maxSpeed = topVelocity/2;
        //Debug.Log("Top Speed: " + topSpeed/2 + " topVelocity: " + topVelocity/2);
    }
    /// <summary>
    /// Returns velocity of rigidbody 
    /// </summary>
    /// <returns></returns>
    public float getSpeed()
    {
        return Mathf.FloorToInt(_speed);
    }
    /// <summary>
    /// Returns max speed variable
    /// </summary>
    /// <returns></returns>
    public float getMaxSpeed()
    {
        return maxSpeed;
    }
    /// <summary>
    /// Returns height of baloom
    /// </summary>
    /// <returns></returns>
    public float getHeight()
    {
        return Mathf.FloorToInt(height);
    }
    /// <summary>
    /// Assigns given value to height
    /// </summary>
    /// <param name="_value"></param>
    public void setHeight(float _value)
    {
        height = _value;
    }
    /// <summary>
    /// Assigns given value to height
    /// </summary>
    /// <param name="_value"></param>
    public void setSpeed(float _value)
    {
        _speed = _value;
    }
    /// <summary>
    /// Returns fuel variable
    /// </summary>
    /// <returns></returns>
    public float getFuel()
    {
        //Debug.Log("Current Fuel:" + fuel);
        return Mathf.FloorToInt(fuel);
    }
    /// <summary>
    /// Assigns given value to fuel
    /// </summary>
    /// <param name="_value"></param>
    public void setFuel(float _value)
    {
        fuel = _value;
    }
    /// <summary>
    /// Checks trigger collisions and add them to list to check if it touches target
    /// and if it touches the ground put the rigidbody sleep
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!landed)
        {
            string targetColor="";
            if (other.gameObject.CompareTag("Obstacle"))
            {
                hitObstacle();
            }
            if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Target"))
            {
                collisions.Add(other.gameObject);
                lastSpeed = getSpeed();
                Debug.Log("Last Speed:"+lastSpeed);
                //rb.constraints = RigidbodyConstraints.FreezeAll;
                if (collisions.Find(x => x.CompareTag("Target")) || other.gameObject.CompareTag("Target"))
                {
                    foreach(var item in collisions)
                    {
                        Debug.Log(item.name);
                        targetColor = item.name;
                        //foreach (var targetItem in targets)
                        //{
                            //targetItem.GetComponent<MeshRenderer>().material.color = item.GetComponent<MeshRenderer>().material.color;
                         //   targetColor = targetItem.name;
                         //   Debug.Log(targetItem.name);
                        //}
                    }
                    other.GetComponent<MeshCollider>().enabled=false;
                    Debug.Log("Ontarget");

                    if(!stuckWithObstacle)
                        _gameManager.onTarget = true;
                    else
                        _gameManager.onTarget = false;

                    //lastSpeed = getSpeed();
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    //rb.velocity = Vector3.zero;
                    //rb.transform.position = other.transform.position;
                    //rb.useGravity = false;
                    //rb.isKinematic = true;
                    //Debug.Log(lastSpeed);
                    //Debug.Log("Landed");
                    landed = true;
                    _gameManager.landed = true;
                }
                //else if(!collisions.Find(x => x.CompareTag("Target")))
                //{
                //    Debug.Log("!Ontarget");
                //    _gameManager.onTarget = false;
                //}
                else if (other.gameObject.CompareTag("Ground") && !landed )
                {
                    lastSpeed = getSpeed();
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    //rb.velocity = Vector3.zero;
                    //rb.useGravity = false;
                    //rb.isKinematic = true;
                    //Debug.Log(lastSpeed);
                    //Debug.Log("Landed");
                    landed = true;
                    _gameManager.landed = true;
                }
                if (lastSpeed <= (maxSpeed*0.7))
                {
                    _gameManager.validSpeed = true;
                }
                else
                {
                    _gameManager.validSpeed = false;
                }
            }
            if (landed)
            {
                _gameManager.gameOver(lastSpeed,getFuel(),targetColor);
            }

        }
    }
    /// <summary>
    /// Checks if collisions exit removes it from the collisions list
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (!landed)
        {
            if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Target"))
            {
                collisions.Remove(other.gameObject);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision");
        if (collision.gameObject.CompareTag("Ground") && !landed)
        {
            landed = true;
            lastSpeed = getSpeed();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.velocity = Vector3.zero;
            //rb.useGravity = false;
            //rb.isKinematic = true;
            Debug.Log(lastSpeed);
            Debug.Log("Landed");
            landed = true;
            _gameManager.landed = true;
            if (lastSpeed <= 50)
            {
                _gameManager.validSpeed = true;
            }
            else
            {
                _gameManager.validSpeed = false;
            }
            if (landed)
            {
                _gameManager.gameOver(lastSpeed, getFuel());
            }
        }

    }
    /// <summary>
    /// Activates speedFx 0 for deactive 1 for active
    /// </summary>
    /// <param name="state"></param>
    public void activateDeactivateSpeedFx(int state)
    {
        if(state==0)
            speedFx.SetActive(false);
        else if(state == 1 && !speedFx.activeSelf)
            speedFx.SetActive(true);
    }
    /// <summary>
    /// Activates fireFx 0 for deactive 1 for active
    /// </summary>
    /// <param name="state"></param>
    public void activateDeactivatefireFx(int state)
    {
        if (state == 0)
            fireFx.SetActive(false);
        else if(state == 1 && !fireFx.activeSelf)
            fireFx.SetActive(true);
    }
    public void hitObstacle()
    {
        if (!stuckWithObstacle)
        {
            GameObject fx = Instantiate(expFx, expPos.transform.position, Quaternion.identity);
            fx.transform.SetParent(expPos.transform);
            fx.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            Destroy(fx, 3f);
            stuckWithObstacle = true;
            rb.velocity += new Vector3(0, -80, 0);
            SoundManager.SInstance.playSfx(SoundManager.SInstance.hitObstacle);
        }

    }
}
