using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    private List<GameObject> checkpointsList = new List<GameObject>();

    [SerializeField] private GameObject spawn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        SetCheckpointsList(spawn);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCheckpointsList(GameObject checkpoint)
    {
        if (checkpointsList != null)
        {
            foreach (GameObject obj in  checkpointsList)
            {
                if (obj ==  checkpoint)
                {
                    return;
                }
            }
        }

        checkpointsList.Add(checkpoint);

        print("Checkpoint Add");
    }

    public GameObject GetLastCheckpoint()
    {
        return checkpointsList.Last();
    }
}
