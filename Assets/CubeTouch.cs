using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTouch : MonoBehaviour
{

    enum GestureType { None, Tap, Drag, Scale, Rotate };

    GestureType currentGesture = GestureType.None;



    public float speed = 0.1F;
    private Vector3 touchedPos;
    private float timeTouched;
    private bool hasMoved;
    private CubeControl nullSelectedObject = new CubeControl();
    private CubeControl currentlySelectedObject;
    private float distanceToSelectedObject;
    public float perspectiveZoomSpeed = 0.5f;
    private float startingDistanceBetweenTouches;
    private float startScale;
    private bool isItaCube;


    

    // Use this for initialization
    void Start()
    {
        currentlySelectedObject = nullSelectedObject;
    }

    // Update is called once per frame
    void Update()
    {
        timeTouched += Time.deltaTime;

        if (Input.touchCount > 0)
        {

            foreach (Touch touch in Input.touches)
            {


                // Debug.Log("Touching at: " + touch.position);
                Ray r = Camera.main.ScreenPointToRay(touch.position);
                // Debug.DrawRay(r.origin, 100 * r.direction);


                RaycastHit hit = new RaycastHit();
                
               


                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        timeTouched = 0f;
                        hasMoved = true;

                        

                        if (Physics.Raycast(r, out hit))
                        {
                            CubeControl latestCube = hit.collider.gameObject.GetComponent<CubeControl>();

                            if (latestCube)
                            {
                                distanceToSelectedObject = Vector3.Distance(Camera.main.transform.position, latestCube.transform.position);
                            }
                        }

                       

                        break;
                    case TouchPhase.Moved:


                        switch (currentGesture)
                        {

                            case GestureType.None:
                                //Need to determine gesture here

                                //Is it a drag
                                if ((Input.touchCount == 1) && (timeTouched >= 0.5f))
                                {

                                    if ((currentlySelectedObject))
                                    {
                                        currentGesture = GestureType.Drag;


                                    }

                                }


                                if (Input.touchCount == 2)
                                {

                                    Touch touchZero = Input.GetTouch(0);
                                    Touch touchOne = Input.GetTouch(1);

                                    if (Input.touches[0].fingerId == touch.fingerId)
                                    {
                                        startingDistanceBetweenTouches = Vector2.Distance(touchZero.position, touchOne.position);

                                        if ((currentlySelectedObject))  startScale = currentlySelectedObject.Scale;
                                    }
                                    else if ((touchOne.phase == TouchPhase.Moved) || (touchZero.phase == TouchPhase.Moved))
                                    {
                                        //Calculate distacne difference
                                        float prevTouchDeltaMag = ((touchZero.position - touchZero.deltaPosition) - (touchOne.position - touchOne.deltaPosition)).magnitude;
                                        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
                                        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                                        print("magDiff: " + deltaMagnitudeDiff);

                                        //Calculate Scale factor
                                        float currentDistanceBetweenTouches = Vector2.Distance(touchZero.position, touchOne.position);
                                        float scaleFactor = currentDistanceBetweenTouches / startingDistanceBetweenTouches;
                                        print("scaleFactor: " + scaleFactor);


                                        // Scale if selected
                                        if (currentlySelectedObject)
                                        {

                                            currentlySelectedObject.Scale += -deltaMagnitudeDiff * 0.01f;
                                        }
                                        // Zoom if not selected
                                        else
                                        {

                                            Camera.main.fieldOfView += deltaMagnitudeDiff * 0.1f;
                                            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 20f, 100f);


                                        }
                                        Vector3 diff = touchOne.position - touchZero.position;
                                        float angle = Mathf.Atan2(diff.y, diff.x);
                                        currentlySelectedObject.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * angle);
                                        
                                    }
                                }





                                


                                break;

                            case GestureType.Drag:

                                if (currentlySelectedObject)
                                {
                                    r = Camera.main.ScreenPointToRay(touch.position);

                                    currentlySelectedObject.transform.position = r.origin + distanceToSelectedObject * r.direction;

                                }
                                break;

                        }



                        print("Moved");
                        hasMoved = false;








                        break;

                    case TouchPhase.Ended:
                        if ((timeTouched < 0.15f) && hasMoved)

                        {
                      
                            if (Physics.Raycast(r, out hit))
                            {
                                CubeControl latestCube = hit.collider.gameObject.GetComponent<CubeControl>();

                                if (latestCube)
                                {
                                    if (currentlySelectedObject != latestCube)
                                    {
                                        if (currentlySelectedObject) currentlySelectedObject.ToggleSelectThisCube();

                                    }

                                    currentlySelectedObject = latestCube;
                                    latestCube.ToggleSelectThisCube();

                                }




                                print("hit");

                            }

                            else
                            {

                                print("here");
                                if (currentlySelectedObject)
                                {
                                    currentlySelectedObject.ToggleSelectThisCube();
                                    currentlySelectedObject = null;
                                }
                            }
                        }
                        if (currentlySelectedObject.gyroIsOn())
                        {
                            currentlySelectedObject.gyroOn(false);
                        }
                        else if (!currentlySelectedObject.gyroIsOn()) 
                        {
                            currentlySelectedObject.gyroOn(true);
                        }
                        print("Time Touch Held:" + timeTouched);

                        

                        break;

                        

                }
                //currentlySelectedObject.gyroOn(true);


                //switch (currentGesture)
                //{
                //    case GestureType.None:
                //        break;
                //    case GestureType.Tap:

                //        if (isItaCube && (hit.collider.gameObject.GetComponent<CubeControl>() != currentlySelectedObject))
                //        {
                //            currentlySelectedObject = hit.collider.gameObject.GetComponent<CubeControl>();
                //            currentlySelectedObject.ToggleSelectThisCube();
                //            distanceToSelectedObject = Vector3.Distance(hit.collider.transform.position, transform.position);

                //        }
                //        else if (isItaCube && (hit.collider.gameObject.GetComponent<CubeControl>() == currentlySelectedObject))
                //        {

                //            currentlySelectedObject.ToggleSelectThisCube();
                //            currentlySelectedObject = nullSelectedObject;
                //        }


                //        break;
                //    case GestureType.Drag:



                //        //Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                //        //transform.Translate(touch.deltaPosition.y * speed, -touch.deltaPosition.x * speed, 0);


                //        break;
                //    case GestureType.Scale:

                //        break;
                //    case GestureType.Rotate:
                //        break;
                //    default:
                //        break;
                //}

                //currentlySelectedObject.RotateCube(touch);




                //pinch and zoom 2 touches
                //if (Input.touchCount == 2)
                //{
                //    Touch touchZero = Input.GetTouch(0);
                //    Touch touchOne = Input.GetTouch(1);

                //    if (touchOne.phase == TouchPhase.Began)
                //    {
                //        startingDistanceBetweenTouches = Vector2.Distance(touchZero.position, touchOne.position);
                //        startScale = currentlySelectedObject.Scale;

                //    }
                //    if ((touchOne.phase == TouchPhase.Moved) || (touchZero.phase == TouchPhase.Moved))
                //    {
                //        //Calculate distacne difference
                //        float prevTouchDeltaMag = ((touchZero.position - touchZero.deltaPosition) - (touchOne.position - touchOne.deltaPosition)).magnitude;
                //        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
                //        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                //        print("magDiff: " + deltaMagnitudeDiff);

                //        //Calculate Scale factor
                //        float currentDistanceBetweenTouches = Vector2.Distance(touchZero.position, touchOne.position);
                //        float scaleFactor = currentDistanceBetweenTouches / startingDistanceBetweenTouches;
                //        print("scaleFactor: " + scaleFactor);

                //        // Scale if selected
                //        if (currentlySelectedObject)
                //        {

                //            currentlySelectedObject.Scale += -deltaMagnitudeDiff * 0.01f;
                //        }
                //        // Zoom if not selected
                //        else
                //        {

                //            Camera.main.fieldOfView += deltaMagnitudeDiff * 0.1f;
                //            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 20f, 100f);


                //        }

                //    }

                //}



            }



            
        }
    }


}
