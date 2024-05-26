using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchToolkit;

public class lookAtObject : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        
    }

    void Update()
    {
        transform.LookAt(ArchToolkitManager.Instance.visitor.Head.transform);
    }

}
