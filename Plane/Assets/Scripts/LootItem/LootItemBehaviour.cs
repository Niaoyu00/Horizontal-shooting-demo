using UnityEngine;

public class LootItemBehaviour : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {//播放结束,禁用物品
        animator.gameObject.SetActive(false);
    }


}
