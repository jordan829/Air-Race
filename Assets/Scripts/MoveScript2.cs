using UnityEngine;
using System.Collections;
using MoveServerNS;

public class MoveScript2 : MonoBehaviour {

    public MoveServer moveServer; // the server to receive controller data
    public MoveController controller;

    public int triggerValue;
    public Vector3 position;
    public Quaternion rotationAngles;

    bool triggerHeld;
    Vector3 firstPosition;
    Quaternion firstAngles;
    Vector3 positionDiff;
    Quaternion angleDiff;
    int scaleFactor;
    float scaleAngle;

    public static bool secondMoveType;

	// Use this for initialization
	void Start () {
        triggerHeld = false;
        scaleFactor = 7500;
        scaleAngle = 0.03f;
        secondMoveType = false;
	}
	
	// Update is called once per frame
	void Update () {
	    controller = moveServer.getController(0);

        if (controller.btnOnPress(MoveServerNS.MoveButton.BTN_SELECT))
        {
            secondMoveType = !secondMoveType;
        }

        if (controller != null && secondMoveType)
        {
            triggerValue = controller.triggerValue;
            position = controller.getPositionRaw();
            rotationAngles = controller.getQuaternion();

            // check if trigger pressed
            if (triggerValue > 0)
            {
                // check if previously held
                if (triggerHeld && !MoveScript1.CountingDown)
                {
                    positionDiff = ((position - firstPosition) * scaleFactor);
                    positionDiff.z = -1 * positionDiff.z;
                    transform.gameObject.GetComponent<Rigidbody>().AddRelativeForce(positionDiff, ForceMode.VelocityChange);
                    if (controller.btnPressed(MoveServerNS.MoveButton.BTN_CROSS))
                    {
                        transform.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward*2000, ForceMode.VelocityChange);
                    }
                    if (controller.btnPressed(MoveServerNS.MoveButton.BTN_CIRCLE))
                    {
                        transform.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * -2000, ForceMode.VelocityChange);
                    }

                    angleDiff = (rotationAngles * Quaternion.Inverse(firstAngles));
                    angleDiff = Quaternion.Euler(-1 * transform.rotation.eulerAngles.x, angleDiff.eulerAngles.y, -1 * transform.rotation.eulerAngles.z);
                    transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * angleDiff, scaleAngle);

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
                "Velocity: " + vel.magnitude + "units\n" +
                "Rotation: (" + transform.rotation.eulerAngles.x + ", " + transform.rotation.eulerAngles.y + ", " + transform.rotation.eulerAngles.z + ")";
        }
	}
}
