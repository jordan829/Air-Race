﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnterCheckpoint : MonoBehaviour {

    static List<Vector3> checkpoints;
    static List<Object> checkpointTrans;

    int checkNum;
    Quaternion lastOrientation;
    public static bool finished;

    float distance;

    public static bool startCD;
    float restartTime;
    int delayTime = 3;

	// Use this for initialization
	void Start () {
        checkNum = 0;
        finished = false;
        startCD = false;
        restartTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        GameObject.FindGameObjectWithTag("CheckNum").GetComponent<UnityEngine.UI.Text>().text = "Checkpoints Taken: " + checkNum;
        if (!finished)
        {
            Transform look = checkpointTrans[checkNum] as Transform;
            GameObject.FindGameObjectWithTag("Direction").transform.LookAt(look);

            distance = (transform.position - look.position).magnitude / 12.0f;
            TextMesh textObject = GameObject.Find("DistanceText").GetComponent<TextMesh>();
            textObject.text = distance.ToString() + " feet";
        }
        else
        {
            GameObject bleh = (GameObject.Find("DistanceText"));
            if(bleh != null)
                bleh.SetActive(false);
            GameObject blue = GameObject.FindGameObjectWithTag("Direction");
            if(blue != null)
                blue.SetActive(false);
        }

        if (restartTime - Time.time >= 0)
        {
            MoveScript1.CountingDown = true;
            GameObject.FindGameObjectWithTag("Countdown").GetComponent<UnityEngine.UI.Text>().text = (restartTime - Time.time).ToString();
        }
        else if (startCD)
        {
            GameObject.FindGameObjectWithTag("Countdown").GetComponent<UnityEngine.UI.Text>().text = "";
            MoveScript1.CountingDown = false;
            startCD = false;
        }
	}

    void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.layer == 8 && !finished)
        {
            if (checkNum > 0)
            {
                //GameObject x = checkpointTrans[checkNum - 1] as GameObject; // last checkpoint, should be inactive
                transform.position = checkpoints[checkNum - 1]; //x.transform.position;
                transform.rotation = lastOrientation;
                startCD = true;
                MoveScript1.CountingDown = true;
                restartTime = Time.time + delayTime;
            }
            else
            {
                transform.position = new Vector3(0, 5, 100);
                transform.rotation = Quaternion.identity;
                startCD = true;
                MoveScript1.CountingDown = true;
                restartTime = Time.time + delayTime;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // check if hit a checkpoint
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            // check if correct checkpoint
            if (other.gameObject.name.CompareTo(checkNum.ToString()) == 0)
            {
                // if yes, update checkNum and set object inactive
                checkNum++;
                other.gameObject.SetActive(false);
                lastOrientation = transform.rotation;
                if (checkNum >= checkpointTrans.Count)
                {
                    finished = true;
                }
            }
            else
            {
                // wrong checkpoint, reset position at last checkpoint
                if(checkNum > 0) {
                    //GameObject x = checkpointTrans[checkNum - 1] as GameObject; // last checkpoint, should be inactive
                    transform.position = checkpoints[checkNum - 1]; //x.transform.position;
                    transform.rotation = lastOrientation;
                    startCD = true; 
                    MoveScript1.CountingDown = true;
                    restartTime = Time.time + delayTime;
                }
                else
                {
                    transform.position = new Vector3(0, 5, 100);
                    transform.rotation = Quaternion.identity;
                    startCD = true;
                    MoveScript1.CountingDown = true;
                    restartTime = Time.time + delayTime;
                }
            }
        }
    }

    public static void loadList(List<Vector3> x, List<Object> y)
    {
        checkpoints = x;
        checkpointTrans = y;
    }
}
