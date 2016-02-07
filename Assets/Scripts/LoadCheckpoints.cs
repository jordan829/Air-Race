using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadCheckpoints : MonoBehaviour {

    List<Vector3> checkpoints;

	void Start () {
        checkpoints = new List<Vector3>();
        // CALL readCheckpoints HERE
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
}
