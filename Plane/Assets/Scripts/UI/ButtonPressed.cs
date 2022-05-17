using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressed : StateMachineBehaviour//״̬����Ϊ��
{
    public static Dictionary<string, System.Action> buttonFunctionTable;//<key:��ť����value�����ܺ���>

    private void Awake()
    {//��ʼ����ť���ܱ� �ֵ�
        buttonFunctionTable = new Dictionary<string, System.Action>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {//״̬���뺯��
        UIInput.Instance.DisableAllUIInputs();//��ֹ�ظ����� ����˲�����ui����
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {//ui�������Ž������ٴ�����������
        //animator.gameObject.name��ǰ���ڲ��Ŷ�����ť����
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
