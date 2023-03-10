using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    static GameStateManager _instance;
    public static GameStateManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("ERROR: instance not found");
            return _instance;
        }
    }
    [SerializeField] int coinPerRun;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] List<Vector3> spawnPoint = new List<Vector3>();
    public readonly List<GameObject> coins = new List<GameObject>();

    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, 1);
        foreach (Collider c in col)
        {
            if (c.gameObject.CompareTag("Player"))
            {
                c.gameObject.GetComponent<PlayerFSM>().InHome = true;
            }
        }

        for(int i = coins.Count - 1; i >= 0; i--)
        {
            if (coins[i] == null)
            {
                coins.RemoveAt(i);
            }
        }
    }

    public void SpawnNewCoin()
    {
        foreach(GameObject c in coins)
        {
            Destroy(c);
        }
        coins.Clear();
        for (int i = 0; i < spawnPoint.Count; i++)//shuffle spawn point
        {
            Vector3 temp = spawnPoint[i];
            temp.y = 0.5f;
            int randomIndex = Random.Range(i, spawnPoint.Count);
            spawnPoint[i] = spawnPoint[randomIndex];
            spawnPoint[randomIndex] = temp;
        }

        for(int i = 0; i < coinPerRun; i++)
        {
            coins.Add(Instantiate(coinPrefab));
            coins[i].transform.position = spawnPoint[i];
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach(Vector3 point in spawnPoint)
        {
            Gizmos.DrawSphere(point, 1);
        }
    }

}
