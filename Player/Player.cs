using System;
using System.Threading.Tasks;
using Godot;

namespace hoppydays.Player
{
	public class Player : KinematicBody2D
	{
		private const int Speed = 1500;
		private const int Gravity = 150;
		private const int JumpSpeed = 3500;
		private const int WorldLimit = 4000;
		private const float BoostMultiplier = 1.5F;
		private Vector2 _motion = new Vector2(0, 0);
		private readonly Vector2 _up = new Vector2(0, -1);
		
		[Signal]
		public delegate void Animate(Vector2 motion);

		[Signal]
		public delegate void OutOfBounds();

		private AudioStreamPlayer2D JumpAudio => GetNode<AudioStreamPlayer2D>("JumpSFX");
		private AudioStreamPlayer2D PainAudio => GetNode<AudioStreamPlayer2D>("PainSFX");

		public override void _PhysicsProcess(float delta)
		{
			ApplyGravity();
			Jump();
			Move();
			AnimatePlayer();
			MoveAndSlide(_motion, _up);
			base._PhysicsProcess(delta);
		}

		private void Move()
		{
			_motion.x = 0;
			if (Input.IsActionPressed(Action.Left.Value)) _motion.x -= Speed;
			if (Input.IsActionPressed(Action.Right.Value)) _motion.x += Speed;
		}

		private void Jump()
		{
			if (!Input.IsActionJustPressed(Action.Jump.Value) || !IsOnFloor()) return;
			_motion.y -= JumpSpeed;
			JumpAudio.Play();
		}

		public async Task Boost()
		{
			await IgnoreGravity();
			_motion.y -= JumpSpeed * BoostMultiplier;
		}

		internal async Task Hurt()
		{
			await IgnoreGravity();
			_motion.y = -JumpSpeed;
			PainAudio.Play();
		}

		private async Task IgnoreGravity()
		{
			Position = new Vector2(Position.x, Position.y - 1);
			await ToSignal(GetTree(), "idle_frame");
		}

		private void AnimatePlayer()
		{
			EmitSignal(nameof(Animate), _motion);
		}
		
		private void ApplyGravity()
		{
			if (Position.y > WorldLimit)
			{
				EmitSignal(nameof(OutOfBounds));
				return;
			}

			if (IsOnFloor() && _motion.y > 0)
			{
				_motion.y = 0;
				return;
			}
			
			if (IsOnCeiling())
			{
				_motion.y = 1;
				return;
			}

			_motion.y += Gravity;
		}

		public class MovementState
		{
			private readonly Vector2 _motion;

			public MovementState(Vector2 motion)
			{
				_motion = motion;
			}
			
			public bool IsJumping => _motion.y < 0;
			public bool IsMoving => Math.Abs(_motion.x) > 0.01;
			public Direction CurrentDirection => Direction.FromMotion(_motion);
		
			public class Direction
			{
				private Direction(string name)
				{
					Name = name;
				}
				
				public string Name { get; }
				
				public static Direction Left => new Direction("left");
				public static Direction Right => new Direction("right");
		
				public static Direction FromMotion(Vector2 motion)
					=> motion.x < 0 ? Left : Right;
			}
		}

		private class Action
		{
			private Action(string value)
			{
				Value = value;
			}
			
			public string Value { get; }
			
			public static Action Left => new Action("left");
			public static Action Right => new Action("right");
			public static Action Jump => new Action("jump");
		}
	}
}
