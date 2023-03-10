using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : StateMachine
{
    public State_Standby standbyState;
    public State_Return returnState;
    public State_FindCoin findState;

    public float speed;
    public int CoinCount = 0;
    
    public void AddCoin() { 
        CoinCount++;
        GameStateManager.Instance.coins.Sort(delegate (GameObject v1, GameObject v2) {
            return Vector3.Distance(v1.transform.position, transform.position)
              .CompareTo(Vector3.Distance(v2.transform.position, transform.position));
        });
    }

    public Vector3 origin { get; private set; }

    public bool InHome = true;

    private void Awake()
    {
        standbyState = new State_Standby(this);
        returnState = new State_Return(this);
        findState = new State_FindCoin(this);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        origin = transform.position;
    }

    protected override BaseState GetInitialState()
    {
        return standbyState;
    }
}
