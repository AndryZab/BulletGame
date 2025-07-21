using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockFPS : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 70;
    }

}
