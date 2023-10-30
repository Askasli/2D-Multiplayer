using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class SparklePoolManager : MonoBehaviourPun
{
    public GameObject sparklePrefab;
    public int poolSize = 20;

    private Queue<GameObject> pooledSparkles = new Queue<GameObject>();

    // Photon Singleton pattern
    public static SparklePoolManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
      //  DontDestroyOnLoad(gameObject);

        if (PhotonNetwork.IsMasterClient)
        {
            // Instantiate the initial pool of sparkles on the master client
            for (int i = 0; i < poolSize; i++)
            {
                GameObject sparkle = PhotonNetwork.InstantiateSceneObject(sparklePrefab.name, Vector3.zero, Quaternion.identity);
                sparkle.SetActive(false);
                pooledSparkles.Enqueue(sparkle);
            }
        }
    }

    public GameObject GetSparkle()
    {
        if (pooledSparkles.Count > 0)
        {
            GameObject sparkle = pooledSparkles.Dequeue();
            sparkle.SetActive(true);
            return sparkle;
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameObject newSparkle = PhotonNetwork.InstantiateSceneObject(sparklePrefab.name, Vector3.zero, Quaternion.identity);
                newSparkle.SetActive(true);
                return newSparkle;
            }
        }

        return null;
    }

    public void ReturnSparkle(GameObject sparkle)
    {
        sparkle.SetActive(false);
        pooledSparkles.Enqueue(sparkle);
    }
}