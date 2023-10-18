using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setScroolBarStartPos : MonoBehaviour
{

    [SerializeField] ScrollRect _sRect;
                             
    void Start()
    {
        setScroolReactPos();
    }
    void setScroolReactPos()
    {
        _sRect.horizontalNormalizedPosition = 0;
    }
}
