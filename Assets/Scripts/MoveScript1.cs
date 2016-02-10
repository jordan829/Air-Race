using UnityEngine;
using System.Collections;
using MoveServerNS;

public class MoveScript1 : MonoBehaviour {

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

    public static bool CountingDown;
    float startTime;

	// Use this for initialization
	void Start () {
        triggerHeld = false;
        scaleFactor = 7500;
        scaleAngle = 0.03f;
        CountingDown = true;
        int delayTime = 3;
        startTime = Time.time + delayTime;
	}
	
	// Update is called once per frame
	void Update () {

        if (startTime - Time.time >= 0)
        {
            GameObject.FindGameObjectWithTag("Countdown").GetComponent<UnityEngine.UI.Text>().text = (startTime - Time.time).ToString();
            GameObject.FindGameObjectWithTag("Timer").GetComponent<UnityEngine.UI.Text>().text = "";
        }
        else
        {
            if (!EnterCheckpoint.startCD)
            {
                GameObject.FindGameObjectWithTag("Countdown").GetComponent<UnityEngine.UI.Text>().text = "";
                CountingDown = false;
            }

            if (!EnterCheckpoint.finished)
            {
                GameObject.FindGameObjectWithTag("Timer").GetComponent<UnityEngine.UI.Text>().text = (Time.time - startTime).ToString() + "s";
            }
        }

        controller = moveServer.getController(0);
        if (controller != null && !MoveScript2.secondMoveType)
        {
            triggerValue = controller.triggerValue;
            position = controller.getPositionRaw();
            rotationAngles = controller.getQuaternion();

            // check if trigger pressed
            if (triggerValue > 0)
            {
                // check if previously held
                if (triggerHeld && !CountingDown)
                {
                    positionDiff = (position - firstPosition) * scaleFactor;
                    positionDiff.z = -1 * positionDiff.z;
                    transform.gameObject.GetComponent<Rigidbody>().AddRelativeForce(positionDiff, ForceMode.VelocityChange);
                    
                    angleDiff = (rotationAngles * Quaternion.Inverse(firstAngles));
                    //transform.rotation = transform.rotation * angleDiff;
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
        else
        {

        }


	}
}
