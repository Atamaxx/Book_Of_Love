using System;
using UnityEngine;

namespace MainController
{
    public struct FrameInput
    {
        public float X;
        public bool JumpDown;
        public bool JumpUp;
        public bool Sprint;
    }

    public interface IPlayerController
    {
        public Vector3 Velocity { get; }
        public FrameInput Input { get; }
        public bool JumpingThisFrame { get; }
        public bool LandingThisFrame { get; }
        public Vector2 RawMovement { get; }
        public bool Grounded { get; }
       // public Vector2 InputAnim { get; }
        //public Vector2 Speed { get; }
        public bool Crouching { get; }

        public event Action<bool, float> GroundedChanged; // Grounded - Impact force
        public event Action Jumped;
        public event Action Attacked;
        public event Action Sprinting;

    }

    //public struct RayRange
    //{
    //    public RayRange(float x1, float y1, float x2, float y2, Vector2 dir)
    //    {
    //        Start = new Vector2(x1, y1);
    //        End = new Vector2(x2, y2);
    //        Dir = dir;
    //    }

    //    public readonly Vector2 Start, End, Dir;
    //}
}