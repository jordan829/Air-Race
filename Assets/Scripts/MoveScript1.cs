using UnityEngine;
using System.Collections;
using MoveServerNS;

public class MoveScript1 : MonoBehaviour {

    public MoveServer moveServer; // the server to receive controller data
    public MoveController controller;

    public int triggerValue;
    public Vector3 position;
    public Vector3 rotationAngles;

    bool triggerHeld;
    Vector3 firstPosition;
    Vector3 firstAngles;
    Vector3 positionDiff;
    int scaleFactor;

	// Use this for initialization
	void Start () {
        triggerHeld = false;
        scaleFactor = 100;
	}
	
	// Update is called once per frame
	void Update () {
        controller = moveServer.getController(0);
        if (controller != null)
        {
            triggerValue = controller.triggerValue;
            position = controller.getPositionRaw();
            rotationAngles = controller.getQuaternion().eulerAngles;

            // check if trigger pressed
            if (triggerValue > 0)
            {
                // check if previously held
                if (triggerHeld)
                {
                    positionDiff = (position - firstPosition) * scaleFactor;
                    positionDiff.z = -1 * positionDiff.z;
                    Vector3 forward = transform.forward.normalized;
                    transform.gameObject.GetComponent<Rigidbody>().AddRelativeForce(positionDiff, ForceMode.VelocityChange);
                    //transform.position = transform.position + (positionDiff * scaleFactor);
                }
                // else, set the standard values and work with them until not held
                else
                {
                    triggerHeld = true;
                    firstPosition = position;
                    firstAngles = rotationAngles;
                }
            }
            // if not, set triggerHeld to false
            else
            {
                triggerHeld = false;
                transform.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }

            Vector3 vel = transform.gameObject.GetComponent<Rigidbody>().velocity;

            GameObject.FindGameObjectWithTag("DebugText").GetComponent<UnityEngine.UI.Text>().text = "TriggerHeld: " + triggerHeld + "\n" +
                "Current Position: (" + transform.position.x + ", " + transform.position.y + ", " + transform.position.z + ")\n" +
                "Velocity: (" + vel.x + ", " + vel.y + ", " + vel.z + ")";
        }
        else
        {

        }


	}
}
