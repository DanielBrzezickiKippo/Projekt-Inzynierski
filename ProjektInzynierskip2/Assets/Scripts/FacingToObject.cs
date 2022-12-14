using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingToObject : MonoBehaviour
{
    public Transform lookAt;

    private void Awake()
    {
        lookAt = Camera.main.gameObject.transform;
    }

    void Update()
    {
        this.gameObject.transform.LookAt(lookAt);
    }
}
