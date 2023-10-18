using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonAnimController : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] SkinnedMeshRenderer _baloonSMR;
    Baloon _baloon;
    Animator animator;
    Animator heightAnim;
    [Header("Controllers")]
    [SerializeField] bool applyWeightToBaloon;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    /// <summary>
    /// Initiliazes variables and references
    /// </summary>
    void Init()
    {
        animator = GetComponent<Animator>();
        heightAnim = GetComponentInChildren<Animator>();
        _baloon = GameObject.FindObjectOfType<Baloon>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_baloon.landed)
        {
            if (applyWeightToBaloon && _baloon.rb.velocity.y < 0)
            {
                float blend = _baloon.getSpeed();
                blend = Mathf.Clamp(blend, 0, 100);
                _baloonSMR.SetBlendShapeWeight(0, blend);
            }
            else
            {
                _baloonSMR.SetBlendShapeWeight(0, 0);
            }

        }

    }
    /// <summary>
    /// Sets animator's float values
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public void playMovementAnimation(float x,float z)
    {
        animator.SetFloat("MoveX", x);
        animator.SetFloat("MoveZ", z);
    }
    public void playHeightAnimations(float speed)
    {
        if (speed <= 30)
            heightAnim.Play("moveUp");
        if(speed>30)
            heightAnim.Play("fall");
        //animator.SetFloat("Speed", speed);
    }
}
