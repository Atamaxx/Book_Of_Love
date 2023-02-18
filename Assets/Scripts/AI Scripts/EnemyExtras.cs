using System;
using UnityEngine;

namespace BaseEnemy
{
    public interface IEnemyController
    {
        public float Speed { get; }
        public Vector3 Velocity { get; }
        public bool JumpingThisFrame { get; }
        public bool LandingThisFrame { get; }
        public Vector2 RawMovement { get; }
        public bool Grounded { get; }
        // public Vector2 InputAnim { get; }
        //public Vector2 Speed { get; }

        public event Action<bool, float> GroundedChanged; // Grounded - Impact force
        public event Action Jumped;
        public event Action Attacked;
        public event Action Sprinting;
        public event Action TargetSpotted;

    }
}