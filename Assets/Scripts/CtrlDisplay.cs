using UnityEngine;
using System.Collections;
using MoveServerNS;

public class CtrlDisplay : MonoBehaviour {

    public MoveServer moveServer; // the server to receive controller data
    public MoveController controller;
    public int triggerValue;
    public Vector3 position;
    public Vector3 rotationAngles;

	// Use this for initialization
	void Start () {
        controller = moveServer.getController(0);
	}
	
	// Update is called once per frame
	void Update () {
        controller = moveServer.getController(0);
        if (controller != null) {
            triggerValue = controller.triggerValue;
            position = controller.getPositionRaw();
            rotationAngles = controller.getQuaternion().eulerAngles;

            GameObject.FindGameObjectWithTag("ControllerInfo").GetComponent<UnityEngine.UI.Text>().text = "Controller " + controller.controllerNumber + "\n" +
                "Position: " + "(" + position.x + ", " + position.y + ", " + position.z + ")\n" +
                "Rotation (by axes): " + "(" + rotationAngles.x + ", " + rotationAngles.y + ", " + rotationAngles.z + ")\n" +
                "Trigger Value: " + triggerValue;
        }
        else {
            GameObject.FindGameObjectWithTag("ControllerInfo").GetComponent<UnityEngine.UI.Text>().text = "Controller not connected.";
        }
        
	}
}
