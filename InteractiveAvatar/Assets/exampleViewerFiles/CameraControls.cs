using UnityEngine;
using System.Collections;

namespace tm
{
		public class CameraControls : MonoBehaviour
		{
				private float sensitivityX = 240.0f;
				private float sensitivityY = 240.0f;
				private float cameraDistance = 1f;
				private float scrollSpeed = 5f;
				private float upDownSensitivity = 1f;
				public GameObject cameraTarget;
				public bool autoRotate = false;
        
				private void Start ()
				{
						cameraDistance = Vector3.Distance (transform.position, cameraTarget.transform.position); 
				}
        
				Vector3 upVector = Vector3.up;
        
				private void Update ()
				{
						if (autoRotate) {
								transform.RotateAround (cameraTarget.transform.position, Vector2.up, .01f); 
						}
            
						Quaternion originalRot = transform.rotation;
						Vector3 originalPos = transform.position;
            
						//orbit around target left button
						bool down = Input.GetMouseButton (0);
						if (down) {
								float rotationX = Input.GetAxis ("Mouse X") * sensitivityX;
								float rotationY = Input.GetAxis ("Mouse Y") * sensitivityY;
								transform.RotateAround (cameraTarget.transform.position, transform.right, Mathf.Deg2Rad * rotationY);
								transform.RotateAround (cameraTarget.transform.position, transform.up, -Mathf.Deg2Rad * rotationX);
						}
            
						//affect up down with other button right button
						down = Input.GetMouseButton (1);
						if (down) {
								float upDown = Input.GetAxis ("Mouse Y") * upDownSensitivity;
                
								transform.position += upVector * upDown;
						}
            
						//affect target height with middle mouse ?
						down = Input.GetMouseButton (2);
						if (down) {
								float upDown = Input.GetAxis ("Mouse Y") * upDownSensitivity;
                
								cameraTarget.transform.position += upVector * upDown;
						}
            
						//zoom with scroll wheel
						float scrollWheel = Input.GetAxis ("Mouse ScrollWheel");
						if (scrollWheel != 0) {
								cameraDistance += Input.GetAxis ("Mouse ScrollWheel") * scrollSpeed;
								transform.position = cameraTarget.transform.position + (cameraDistance * Vector3.Normalize (transform.position - cameraTarget.transform.position));
						}
            
						//fix camera z slant
						if (Mathf.Abs (Vector3.Dot (Vector3.Normalize (transform.position - cameraTarget.transform.position), upVector)) < .99f) {
								transform.LookAt (cameraTarget.transform, upVector);
						} else {
								//here be usual pole gimbal lock like problem
                
								//trying to do similar to unitys preview window where it locks the rotation
								transform.rotation = originalRot;
								transform.position = originalPos;
						}
				}
		}
}
