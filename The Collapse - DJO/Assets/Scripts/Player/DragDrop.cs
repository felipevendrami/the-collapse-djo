using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    
    /*
        FUNCIONALIDADE NAO SERA UTILIZADA
    */

    public Camera cam;
    Vector3 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        GameObject cameraObject = GameObject.Find("Camera");
        cam = cameraObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {   
        transform.position = cam.ScreenToWorldPoint(Input.mousePosition - mousePosition);
    }

    private Vector3 GetMousePosition()
    {
        return cam.WorldToScreenPoint(transform.position);
    }

    public void Ativar()
    {
        mousePosition = Input.mousePosition - GetMousePosition();
    }
}
