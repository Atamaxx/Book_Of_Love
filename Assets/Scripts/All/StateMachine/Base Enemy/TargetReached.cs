using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetReached : StateMachineBehaviour
{
    private EnemyController controller;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<EnemyController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.RunCollisionChecks();
        controller.FollowingTarget(animator);
        animator.SetBool("targetSpotted", controller.SearchingForTarget(animator));
        controller.CalculateGravity();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
