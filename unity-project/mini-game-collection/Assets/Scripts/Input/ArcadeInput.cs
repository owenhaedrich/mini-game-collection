using System;
using UnityEngine;

namespace MiniGameCollection
{
    public class ArcadeInput : MonoBehaviour
    {
        public static Player Player1 { get; private set; } = new Player(1);
        public static Player Player2 { get; private set; } = new Player(2);
        public static Player[] Players => new Player[] { Player1, Player2 };
        private static ArcadeInput Instance { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        private void Update()
        {
            foreach (var player in Players)
            {
                // 8-way joystick
                foreach (var button in player.Buttons)
                {
                    // Apply old input
                    button.PreviousState = button.CurrentState;
                    // Record current input for next update
                    bool state = button.Down;
                    button.CurrentState = state;
                }

                // Buttons
                foreach (var button in player.Buttons)
                {
                    // Apply old input
                    button.PreviousState = button.CurrentState;
                    // Record current input for next update
                    bool state = button.Down;
                    button.CurrentState = state;
                }
            }
        }

        public class Player
        {
            public Player(int id)
            {
                ID = id;
                // Axis
                AxisX = new Axis($"P{id}_AxisX");
                AxisY = new Axis($"P{id}_AxisY");
                Joystick8Way = new Axis2(AxisX, AxisY);
                // Buttons
                Action1 = new Button($"P{id}_Action1");
                Action2 = new Button($"P{id}_Action2");
                // Axis as button
                Left = new AxisButton($"P{id}_AxisX", false);
                Right = new AxisButton($"P{id}_AxisX", true);
                Up = new AxisButton($"P{id}_AxisY", true);
                Down = new AxisButton($"P{id}_AxisY", false);
            }

            public Button[] Buttons => new Button[] { Action1, Action2 };

            public static int ID { get; private set; }
            public Axis2 Joystick8Way { get; private set; }
            public Axis AxisX { get; private set; }
            public Axis AxisY { get; private set; }
            public Button Action1 { get; private set; }
            public Button Action2 { get; private set; }
            public AxisButton Up { get; private set; }
            public AxisButton Down { get; private set; }
            public AxisButton Left { get; private set; }
            public AxisButton Right { get; private set; }
        }

        public class Axis
        {
            public Axis(string inputName)
            {
                InputName = inputName;
            }

            public string InputName { get; private set; }
            public float Value => Input.GetAxis(InputName);


            public static implicit operator float(Axis axis)
            {
                return axis.Value;
            }
        }

        public class Axis2
        {
            public Axis2(Axis x, Axis y)
            {
                X = x;
                Y = y;
            }

            public Axis X { get; private set; }
            public Axis Y{ get; private set; }

            public static implicit operator Vector2(Axis2 axis2)
            {
                Vector2 value = new Vector2(axis2.X.Value, axis2.Y.Value);
                return value;
            }
        }

        public class AxisButton
        {
            public AxisButton(string inputName, bool isPositive)
            {
                InputName = inputName;
                AxisButtonDownFunction = isPositive ? CheckPositive : CheckNegative;
            }

            private readonly Func<bool> AxisButtonDownFunction;
            private bool CheckPositive() => Input.GetAxis(InputName) > +0.5f;
            private bool CheckNegative() => Input.GetAxis(InputName) < -0.5f;
            private bool AxisButtonUp() => AxisButtonDownFunction();
            private bool AxisButtonDown() => AxisButtonUp();
            private bool AxisButtonPressed() => AxisButtonDown() && !PreviousState;
            private bool AxisButtonReleased() => AxisButtonUp() && PreviousState;


            public string InputName { get; private set; }
            public bool PreviousState { get; internal set; }
            public bool CurrentState { get; internal set; }
            public bool Down => AxisButtonDown();
            public bool Up => AxisButtonUp();
            public bool Pressed => AxisButtonPressed();
            public bool Released  => AxisButtonReleased();
        }

        public class Button
        {
            public Button(string inputName)
            {
                InputName = inputName;
            }

            public string InputName { get; private set; }
            public bool PreviousState { get; internal set; }
            public bool CurrentState { get; internal set; }
            public bool Down => Input.GetAxis(InputName) > 0.5f;
            public bool Up => Input.GetAxis(InputName) < 0.5f;
            public bool Pressed => Down && !PreviousState;
            public bool Released => Up && PreviousState;
        }

    }
}
