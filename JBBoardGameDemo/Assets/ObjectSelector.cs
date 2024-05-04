using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSelector : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject selected;
    [SerializeField] private bool fixOffset = false;
    [SerializeField] private LayerMask layersToInteractWith;
    [SerializeField] private LayerMask placementLayers;
    [SerializeField] private Vector3 specificOffset;


    //Making a note here for whenever I decide to start implementing this stuff myself
    //Note that he created a UI with buttons.  Which will be used to select the current Piece
    //So he sets   Selected   to that piecesInventoryRef variable if(!=null)

    private void Awake()
    {
        var c = GetComponent<Camera>();
        if (c != null && cam == null)
        {
            cam = c; //instead of using Camera.main, we cache it using this part of our script so we don't have to go searching for the Camera every call
        }

    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //Keep in mind you can do this on Up instead. kinda like how Genshin does it
        if(Input.GetMouseButtonDown(0)) //Now we're going to setup how we will detect a mouse input
        { //This returns true during the frame the user pressed the given mouse button //index 0 is left-click. AND single-tap on touchscreen
            //Debug.Log("User clicked the left mouse button");
            Vector3 mouse = Vector3.zero;
            mouse.x = Input.mousePosition.x;
            mouse.y = Input.mousePosition.y;
            Ray ray = cam.ScreenPointToRay(mouse);

            //gonna use a Raycast - projects a line - and will detect any collisions w/ anything that has a Collider on it, and report back
            //We're going to give, as a parameter to the raycast function, and empty RaycatHit variable. So that when it hits an object, it also tells us WHAT object we hit
            RaycastHit hitInfo;
            Debug.DrawLine(ray.origin, ray.direction * 100, Color.blue, 3);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.green, 3);
                Debug.Log($"Hit Name: {hitInfo.collider.gameObject.name} " +
                    $"Tag: {hitInfo.collider.gameObject.tag} " +
                    $"Layer: {LayerMask.LayerToName(hitInfo.collider.gameObject.layer)}");
                
                //Only assign an object as  Selected,  if the click interacts with a Collider
                if (selected != null)
                {//Here is where we can put all of our Object re-placing logic:
                    
                    int layer = hitInfo.collider.gameObject.layer;
                    if ((placementLayers & (1 << layer)) != 0)
                    {
                        //layer exists in mask
                        
                    }
                    else
                    {
                        //layer does NOT exist in the mask.
                        return; //don't place item if we haven't selected a placement layer
                    }

                    
                    if (fixOffset) //Y offset
                    {
                        Vector3 pos = hitInfo.point;
                        selected.transform.position = pos + specificOffset;
                    }
                    else
                    {
                        selected.transform.position = hitInfo.point;
                    }
                    Debug.Log("Placed item"); //we're going to place the item
                    Debug.Log($"The item we last clicked on (where we placed): {hitInfo.collider.gameObject.name}");

                    
                }
                else
                {
                    Debug.Log("Selected new item");
                    int layer = hitInfo.collider.gameObject.layer;
                    if ((layersToInteractWith & (1 << layer)) != 0)
                    {
                        //layer exists in mask
                        //so it's part of the group of layers we WANT to SELECT.
                        //We can still Collide/interact with layers  that are NOT in this mask.
                        selected = hitInfo.collider.gameObject;
                        Selectable s = selected.GetComponent<Selectable>();
                        if(s != null)
                        {
                            s.SetSelected(true);
                        }
                    }

                }
            
            
            
            }
            else
            {
                if (selected != null)
                {

                    Selectable s = selected.GetComponent<Selectable>();
                    if (s != null)
                    {
                        s.SetSelected(false);
                    }
                }
                selected = null;
            }

        }

        

    }
}
