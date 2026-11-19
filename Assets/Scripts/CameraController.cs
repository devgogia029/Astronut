using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera; //reference to the main camera; private so can't be changed in inspector
    public float targetSize; //the size the camera will zoom to; public so can be changed in the inspector
    public Vector3 targetPosition; //the position the camera is moving towards; public so can be changed in the inspector
    public float smoothSpeed = 3f; //how fast the camera moves; public so can be changed in the inspector

    void Start()
    {
        mainCamera = GetComponent<Camera>(); //gets the camera component on this object and stores it in mainCamera
        targetSize = mainCamera.orthographicSize; //sets the target size to the cameras current size
        targetPosition = transform.position; //sets the target position to the cameras current position
    }
    
    void LateUpdate() //runs after all other updates; used for camera movement so it doesn't jitter
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime); //moves the camera towards the target position smoothly
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, smoothSpeed * Time.deltaTime); //zooms the camera towards the target size smoothly
    }
    
    public void MoveTo(Vector3 newTarget, float newSize) //called by other scripts to move the camera to a new position
    {
        targetPosition = newTarget; //sets the target position to the new position
        targetSize = newSize; //sets the target size to the new size
    }
}
