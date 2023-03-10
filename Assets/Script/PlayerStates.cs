using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Return : BaseState
{
    public State_Return(PlayerFSM stateMachine) : base("Return", stateMachine) { }
    public override void Enter()
    {
        stateMachine.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ((PlayerFSM)stateMachine).InHome = false;
    }
    public override void UpdateLogic()
    {
        if(((PlayerFSM)stateMachine).InHome)
        {
            stateMachine.ChangeState(((PlayerFSM)stateMachine).standbyState);
        }
    }
    public override void UpdatePhysics() 
    {
        stateMachine.GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(((PlayerFSM)stateMachine).transform.position, 
            ((PlayerFSM)stateMachine).origin, ((PlayerFSM)stateMachine).speed * Time.deltaTime));
    }
    public override void Exit() { }
}

public class State_FindCoin : BaseState
{
    public State_FindCoin(PlayerFSM stateMachine) : base("FindCoin", stateMachine) { }
    Vector3 target;
    public override void Enter()
    {
        GameStateManager.Instance.coins.Sort(delegate (GameObject v1, GameObject v2)
        {
            return Vector3.Distance(v1.transform.position, stateMachine.transform.position).CompareTo(Vector3.Distance(v2.transform.position, stateMachine.transform.position));
        });
    }
    public override void UpdateLogic() 
    {

        if(((PlayerFSM)stateMachine).CoinCount < 5)
        {

            //((PlayerFSM)stateMachine)
            if(GameStateManager.Instance.coins[0] != null)
            {
                target = GameStateManager.Instance.coins[0].transform.position;
                target = new Vector3(target.x, stateMachine.transform.position.y, target.z);
            }
        }
        else
        {
            stateMachine.ChangeState(((PlayerFSM)stateMachine).returnState);
        }
    }
    public override void UpdatePhysics() 
    {
        stateMachine.GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(((PlayerFSM)stateMachine).transform.position,
            target, ((PlayerFSM)stateMachine).speed * Time.deltaTime));
    }
    public override void Exit() { }
}

public class State_Standby : BaseState
{
    public State_Standby(PlayerFSM stateMachine) : base("Standby", stateMachine) { }
    float timer = 0;
    float flashTimer = 0.2f;
    Color originalMat;
    bool yellow = false;
    public override void Enter()
    {
        originalMat = stateMachine.GetComponent<MeshRenderer>().material.color;
        stateMachine.GetComponent<Rigidbody>().velocity = Vector3.zero;
        timer = 0;
    }
    public override void UpdateLogic()
    {
        if (timer < 2)
        {
            if(flashTimer <= 0)
            {
                if(yellow)
                    stateMachine.GetComponent<MeshRenderer>().material.color = Color.yellow;
                else
                    stateMachine.GetComponent<MeshRenderer>().material.color = originalMat;
                yellow = !yellow;
                flashTimer = 0.2f;
            }
            flashTimer -= Time.deltaTime;
            timer += Time.deltaTime;
        }
        else
            stateMachine.ChangeState(((PlayerFSM)stateMachine).findState);

    }
    public override void UpdatePhysics() { }
    public override void Exit() { 
        stateMachine.GetComponent<MeshRenderer>().material.color = originalMat;
        GameStateManager.Instance.SpawnNewCoin();
        ((PlayerFSM)stateMachine).CoinCount = 0;
    }

}