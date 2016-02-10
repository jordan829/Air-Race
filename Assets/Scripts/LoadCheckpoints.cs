using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadCheckpoints : MonoBehaviour {

    List<Vector3> checkpoints;
    public Transform checkpointFab;
    List<Object> checkpointTrans;

	void Start () {
        checkpoints = new List<Vector3>();
        checkpointTrans = new List<Object>();
        // CALL readCheckpoints HERE
        readCheckpoints("Assets/race1.txt");
        printCheckpoints();
        setUpCheckpoints();
        EnterCheckpoint.loadList(checkpoints, checkpointTrans);
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
            checkpointTrans.Add(x);
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject check1 = (checkpointTrans[0] as Transform).gameObject;
        GameObject check2 = (checkpointTrans[1] as Transform).gameObject;
        if (check1 == null || check2 == null)
            print("ajskdlf;ajkdfl;aj ksfjaklf;ajksd");
        player.transform.position = check1.transform.position;
        player.transform.LookAt(check2.transform);
    }
}
