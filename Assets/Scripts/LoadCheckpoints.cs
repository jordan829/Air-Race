using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadCheckpoints : MonoBehaviour {

    List<Vector3> checkpoints;
    public Transform checkpointFab;
    List<Object> checkpointGOs;

	void Start () {
        checkpoints = new List<Vector3>();
        checkpointGOs = new List<Object>();
        // CALL readCheckpoints HERE
        readCheckpoints("Assets/testpoints.xyz");
        printCheckpoints();
        setUpCheckpoints();
        EnterCheckpoint.loadList(checkpoints, checkpointGOs);
	}

    public void readCheckpoints(string filename)
    {
        string line;

        System.IO.StreamReader file = new System.IO.StreamReader(filename);

        while ((line = file.ReadLine()) != null)
        {
            char[] delims = {' '};
            string[] words = line.Split(delims);
            float[] values = new float[3];

            for (int i = 0; i < words.Length; i++)
            {
                if (i >= 3)
                {
                    print("Error in XYZ file.");
                    break;
                }
                string word = words[i];
                values[i] = float.Parse(word);
            }

            checkpoints.Add(new Vector3(values[0], values[1], values[2]));
        }
    }

    public void printCheckpoints()
    {
        for (int i = 0; i < checkpoints.Count; i++)
        {
            print("(" + checkpoints[i].x + ", " + checkpoints[i].y + ", " + checkpoints[i].z + ")");
        }
    }

    public void setUpCheckpoints()
    {
        for (int i = 0; i < checkpoints.Count; i++)
        {
            Object x = GameObject.Instantiate(checkpointFab, new Vector3(checkpoints[i].x, checkpoints[i].y, checkpoints[i].z), Quaternion.identity);
            x.name = i.ToString();
            checkpointGOs.Add(x);

        }
    }
}
