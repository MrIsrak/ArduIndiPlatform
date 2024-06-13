using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField, Range(0f, 1f)] float parllaxStrenght = 0.1f;
    [SerializeField] bool disableParralax;
    Vector3 targetPV;

    void Start()
    {
        if (!followTarget)
        {
            followTarget = Camera.main.transform;
        }
        targetPV = followTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta = followTarget.position - targetPV;

        if (disableParralax)
        {
            delta.y = 0;
        }
        targetPV = followTarget.position;

        transform.position += delta * parllaxStrenght;
    }
}
