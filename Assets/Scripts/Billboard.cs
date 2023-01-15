using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera cam;
    Vector3 offset;

    private void Start()
    {
        cam = Camera.main;
        offset = cam.transform.position - transform.position;
    }

    public void LockPos()
    {
        transform.parent = null;
    }
}
