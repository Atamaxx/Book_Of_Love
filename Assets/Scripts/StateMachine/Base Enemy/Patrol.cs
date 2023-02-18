using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : StateMachineBehaviour
{
    private EnemyController controller;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<EnemyController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.RunCollisionChecks();
        controller.Patroling();
        controller.CalculateGravity();
        controller.MoveCharacterX(controller.CalculateRun());

        if (controller.ShouldJump())
        {
            controller.MoveCharacterY(controller.CalculateJump());
        }

        animator.SetBool("targetSpotted", controller.SearchingForTarget(animator));
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}
