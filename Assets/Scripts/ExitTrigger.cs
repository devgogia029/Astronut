using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public float leftCameraSize; //the size the camera will zoom to when the player enters the trigger; set in inspector
    public float rightCameraSize; //the size the camera will zoom to when the player exits the trigger; set in inspector
    public CameraController cam; //reference to the camera controller script; drag it in via inspector
    public Vector3 leftCameraPosition; //the position the camera will move to when the player enters the trigger; set in inspector
    public Vector3 rightCameraPosition; //the position the camera will move to when the player exits the trigger; set in inspector

    public Transform leftRespawnPosition; //spawn marker for Part1
    public Transform rightRespawnPosition; //spawn marker for Part2

    void OnTriggerExit2D(Collider2D other) //called when another collider exits this trigger
    {
        if (other.CompareTag("Player")) //if the other collider is the player
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>(); //get the PlayerRespawn script from the player
            if (other.transform.position.x > transform.position.x) //if the player is to the right of the trigger
            {
                cam.MoveTo(rightCameraPosition, rightCameraSize); //move the camera to the right position
                respawn.SetCheckpoint(rightRespawnPosition.position); //set the respawn position to the right spawn marker
            }

            else //if the player is to the left of the trigger
            {
                cam.MoveTo(leftCameraPosition, leftCameraSize); //move the camera to the left position
                respawn.SetCheckpoint(leftRespawnPosition.position); //set the respawn position to the left spawn marker
            }
        }
    }
}

