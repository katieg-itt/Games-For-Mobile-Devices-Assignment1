using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour {
    bool Imselected = false;

    public float Scale;
    private float rotationRate = 3.0F;
    public float speed = 0.1F;




    Renderer ourRenderer;
	// Use this for initialization
	void Start () {
        ourRenderer = GetComponent<Renderer>();
        Scale = transform.localScale.magnitude;
        Input.gyro.enabled = true;
        Imselected = false;
       

    }
	
	// Update is called once per frame
	void Update () {

        
        transform.localScale = Scale * Vector3.one;
        if (Scale <= 0f) Scale = 0.1f;

       transform.rotation = Input.gyro.attitude;

       // transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);


        //////Accelerometer
      // Vector3 vect = Vector3.zero;

      //  //remap the device acceleration axis to game coordinates:
      //  ////// 1) XY plane of the device is mapped onto XZ plane
      //  ////// 2) rotated 90 degrees around Y axis
      //  vect.x = -Input.acceleration.y;
      //  vect.z = Input.acceleration.x;

      //  ////// clamp acceleration vector to the unit cube
      //if (vect.sqrMagnitude > 1)
      //     vect.Normalize();

      //  ////// Make it move 10 meters per second instead of 10 meters per frame
      // vect *= Time.deltaTime;

      //  ////// Move object
      // transform.Translate(vect * speed);


    }

    public void gyroOn(bool on)
    {
        if (on == false)
        {
            Input.gyro.enabled = false;
        }
        else
        {
            Input.gyro.enabled = true;
        }

    }
    public bool gyroIsOn()
        {
            if (Input.gyro.enabled == true)
            {
                return true;
            }
            else
                return false;
        }
      


    

    internal void ToggleSelectThisCube()
    {

        Imselected = !Imselected;

        if (Imselected && (ourRenderer.material.color == Color.white))
        {
            ourRenderer.material.color = Color.red;
        }
        else if(!Imselected && (ourRenderer.material.color == Color.red))
        {
            ourRenderer.material.color = Color.white;
        }
    }
    }

