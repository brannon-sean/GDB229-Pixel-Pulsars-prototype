using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;
    [SerializeField] bool invertY;

    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //get input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        if (invertY)
            xRotation += mouseY;
        else 
            xRotation -= mouseY;

        // keeps rotation on x axis
        xRotation = Mathf.Clamp(xRotation, lockVertMin, lockVertMax);

        // rotate camera x axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // rotate camera y axis
        transform.parent.Rotate(Vector3.up, mouseX);

    }
}
