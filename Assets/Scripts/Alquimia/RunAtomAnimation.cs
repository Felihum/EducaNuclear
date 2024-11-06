using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alquimia
{
    public class RunAtomAnimation : MonoBehaviour
    {
        [SerializeField] private Animator atomAnimator;
        [SerializeField] private Animator modalAnimator;

        private void Start()
        {
            StartCoroutine(RunAnimations());
        }

        private IEnumerator RunAnimations()
        {
            // Wait until the initial animation is complete
            // You can either wait for the animation's length or check for a specific state in the Animator
            yield return new WaitUntil(() => IsAnimationComplete(atomAnimator, "FadeInAtom"));

            modalAnimator.SetBool("fadeInModal", true);
        }

        private bool IsAnimationComplete(Animator animator, string animationName)
        {
            // This method checks if the specified animation has finished playing
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 0.5f;
        }
    }
}
