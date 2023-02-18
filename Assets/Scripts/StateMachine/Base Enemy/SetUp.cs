using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUp : StateMachineBehaviour
{
    private BaseEnemyStats baseEnemy;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        baseEnemy = animator.GetComponent<BaseEnemyStats>();
        animator.SetBool("canPatrol", baseEnemy.CanPatrol);
        animator.SetBool("hasTarget", baseEnemy.HasTarget);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
