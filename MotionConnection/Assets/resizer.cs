using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resizer : MonoBehaviour
{
    public GameObject start;
    public GameObject end; 
    public float yRotationOffset = 0;
    private Vector3 initialScale; 

    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
        UpdateTransformForScale();
    }

    // Update is called once per frame
    void Update()
    {
        if(start.transform.hasChanged || end.transform.hasChanged)
        {
            UpdateTransformForScale();
        }

    }

    void UpdateTransformForScale()
    {
        float distance = Vector3.Distance(start.transform.position, end.transform.position);
        transform.localScale = new Vector3(initialScale.x,  distance, initialScale.z);

        Vector3 middlePoint = (start.transform.position + end.transform.position) / 2f;
        transform.position = middlePoint;

        Vector3 rotationDirection = (end.transform.position - start.transform.position);
        transform.up = rotationDirection;

        transform.Rotate(0 ,yRotationOffset,0);

    }
}
