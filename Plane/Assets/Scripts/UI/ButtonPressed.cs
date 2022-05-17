using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressed : StateMachineBehaviour//状态机行为类
{
    public static Dictionary<string, System.Action> buttonFunctionTable;//<key:按钮名，value：功能函数>

    private void Awake()
    {//初始化按钮功能表 字典
        buttonFunctionTable = new Dictionary<string, System.Action>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {//状态进入函数
        UIInput.Instance.DisableAllUIInputs();//防止重复按键 按下瞬间禁用ui输入
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {//ui动画播放结束后再创建函数功能
        //animator.gameObject.name当前正在播放动画按钮名字
        buttonFunctionTable[animator.gameObject.name].Invoke();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
