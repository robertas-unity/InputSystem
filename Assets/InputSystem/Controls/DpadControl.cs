using System;
using ISX.LowLevel;
using UnityEngine;

namespace ISX
{
    /// <summary>
    /// A control made up of four discrete, directional buttons. Forms a vector
    /// but can also be addressed as individual buttons.
    /// </summary>
    /// <remarks>
    /// Is stored as four bits by default.
    ///
    /// The vector that is aggregated from the button states is normalized. I.e.
    /// even if pressing diagonally, the vector will have a length of 1 (instead
    /// of reading something like <c>(1,1)</c> for example).
    /// </remarks>
    public class DpadControl : InputControl<Vector2>
    {
        public enum ButtonBits
        {
            Up,
            Down,
            Left,
            Right,
        }

        /// <summary>
        /// The button representing the vertical upwards state of the D-Pad.
        /// </summary>
        [InputControl(bit = (int)ButtonBits.Up)]
        public ButtonControl up { get; private set; }

        /// <summary>
        /// The button representing the vertical downwards state of the D-Pad.
        /// </summary>
        [InputControl(bit = (int)ButtonBits.Down)]
        public ButtonControl down { get; private set; }

        /// <summary>
        /// The button representing the horizontal left state of the D-Pad.
        /// </summary>
        [InputControl(bit = (int)ButtonBits.Left)]
        public ButtonControl left { get; private set; }

        /// <summary>
        /// The button representing the horizontal right state of the D-Pad.
        /// </summary>
        [InputControl(bit = (int)ButtonBits.Right)]
        public ButtonControl right { get; private set; }

        ////TODO: should have X and Y child controls as well

        public DpadControl()
        {
            m_StateBlock.sizeInBits = 4;
            m_StateBlock.format = InputStateBlock.kTypeBit;
        }

        protected override void FinishSetup(InputControlSetup setup)
        {
            up = setup.GetControl<ButtonControl>(this, "up");
            down = setup.GetControl<ButtonControl>(this, "down");
            left = setup.GetControl<ButtonControl>(this, "left");
            right = setup.GetControl<ButtonControl>(this, "right");
            base.FinishSetup(setup);
        }

        protected override Vector2 ReadRawValueFrom(IntPtr statePtr)
        {
            var upIsPressed = up.ReadValueFrom(statePtr) >= up.pressPointOrDefault;
            var downIsPressed = down.ReadValueFrom(statePtr) >= down.pressPointOrDefault;
            var leftIsPressed = left.ReadValueFrom(statePtr) >= left.pressPointOrDefault;
            var rightIsPressed = right.ReadValueFrom(statePtr) >= right.pressPointOrDefault;

            var upValue = upIsPressed ? 1.0f : 0.0f;
            var downValue = downIsPressed ? -1.0f : 0.0f;
            var leftValue = leftIsPressed ? -1.0f : 0.0f;
            var rightValue = rightIsPressed ? 1.0f : 0.0f;

            var result = new Vector2(leftValue + rightValue, upValue + downValue);

            // If press is diagonal, adjust coordinates to produce vector of length 1.
            // pow(0.707107) is roughly 0.5 so sqrt(pow(0.707107)+pow(0.707107)) is ~1.
            const float diagonal = 0.707107f;
            if (result.x != 0 && result.y != 0)
                return new Vector2(result.x * diagonal, result.y * diagonal);

            return result;
        }
    }
}
