using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : StateMachineBehaviour
{
    private EnemyController controller;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<EnemyController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.RunCollisionChecks();

        animator.SetBool("targetSpotted", controller.SearchingForTarget(animator));

        if (!controller.FollowingTarget(animator))
        {
            controller.Stop();
            return;
        }

        controller.CalculateGravity();
        controller.MoveCharacterX(controller.CalculateRun());

        if (controller.ShouldJumpY() || controller.ShouldJumpX())
        {
            controller.Stop();
            controller.Jump();
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
