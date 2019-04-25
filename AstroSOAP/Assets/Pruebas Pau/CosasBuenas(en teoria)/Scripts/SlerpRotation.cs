using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpRotation : MonoBehaviour
{

    Quaternion targetAngle_90 = Quaternion.Euler(0, 0, 90);
    Quaternion targetAngle_0 = Quaternion.Euler(0, 0, 0);

    public Quaternion currentAngle;


    // Start is called before the first frame update
    void Start()
    {
        currentAngle = targetAngle_0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Space))
        {
            ChangeCurrentAngle();
        }

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, currentAngle, 0.2f);
    }

    private void ChangeCurrentAngle()
    {
        if (currentAngle.eulerAngles.z == targetAngle_0.eulerAngles.z)
        {
            currentAngle = targetAngle_90;
        }
        else
        {
            currentAngle = targetAngle_0;
        }
    }
}
