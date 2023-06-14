using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : StateMachineBehaviour
{

    private Rigidbody2D _rigidbody2D;
    private EnemyController controller;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<EnemyController>();
        _rigidbody2D = animator.GetComponent<Rigidbody2D>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.CalculateGravity();
        MoveCharacter(controller.CalculateRun(), _rigidbody2D.velocity.y);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }





    private void MoveCharacter(float horizontalSpeed, float verticalSpeed)
    {
        //RawMovement = new Vector2(_currentHorizontalSpeed, _currentVerticalSpeed);
        _rigidbody2D.velocity = new Vector2(horizontalSpeed, verticalSpeed);
    }
}
