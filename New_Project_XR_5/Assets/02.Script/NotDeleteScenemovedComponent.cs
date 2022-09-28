using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotDeleteScenemovedComponent : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);   
    }
}
