using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] [Range(0.01f, 1f)]
    private float smoothSpeed = 0.125f;

    [SerializeField] private Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        //offset = gameObject.transform.position;
    }

    void LateUpdate()
    {
        if (target)
        {
            Vector3 desiredPosition = target.position + GetProperOffset();
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        }
    }

    Vector3 GetProperOffset()
    {
        float x = Mathf.Floor(target.position.x);
        float z = Mathf.Floor(target.position.z);

        if (z == 0 && x!=0)//z==0
            return new Vector3(-9F, 9.5F, -5f);
        else if (x == 9)//x==9
            return new Vector3(-7f, 9.5f, -8f);
        else if (z == 9)//z==9
            return new Vector3(-5f, 9.5f, -6f);
        else
            return new Vector3(-5f, 9.5f, -4f);


    }


    public void SetTarget(Transform target)
    {
        this.target = target;
    }

}
