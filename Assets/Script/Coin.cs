using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, 1);
        foreach(Collider c in col)
        {
            if(c.gameObject.CompareTag("Player"))
            {
                c.gameObject.GetComponent<PlayerFSM>().AddCoin();
                Destroy(gameObject);
            }
        }
    }
}
