using UnityEngine;

namespace Platformer
{
    public class FireState : BaseState
    {
        private FireStrategy _fireStrategy;
        public FireState(PlayerController player, Animator animator, FireStrategy fireStrategy) : base(player, animator)
        {
            _fireStrategy = fireStrategy;
        }

        public override void OnEnter()
        {
            animator.CrossFade(FireHash, crossFadeDuration);
            _fireStrategy.Fire(player.gameObject, player.FirePoint);
        }
    }
}
