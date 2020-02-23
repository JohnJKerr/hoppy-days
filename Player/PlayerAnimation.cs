using Godot;

namespace hoppydays.Player
{
    public class PlayerAnimation : AnimatedSprite
    {
        public override void _Ready()
        {
            base._Ready();
            Play(AnimationState.Idle.Name);
        }

        public void OnPlayerAnimate(Vector2 motion)
        {
            Animate(new Player.MovementState(motion));
        }

        private void Animate(Player.MovementState state)
        {
            AnimationState GetAnimation()
            {
            	if (state.IsJumping) return AnimationState.Jump;
            	return state.IsMoving ? AnimationState.Walk : AnimationState.Idle;
            }
            
            Play(GetAnimation().Name);
            FlipH = state.CurrentDirection.Name == Player.MovementState.Direction.Left.Name;
        }
        
        private class AnimationState
        {
            private AnimationState(string name)
            {
                Name = name;
            }
			
            public string Name { get; }
			
            public static AnimationState Idle => new AnimationState("idle");
            public static AnimationState Walk => new AnimationState("walk");
            public static AnimationState Jump => new AnimationState("jump");
        }
    }
}
