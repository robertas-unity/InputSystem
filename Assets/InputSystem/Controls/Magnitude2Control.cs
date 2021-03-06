using System;
using ISX.LowLevel;
using UnityEngine;

////TODO: support vector of shorts

namespace ISX
{
    // Computes the magnitude of a Vector2.
    // You can add this as a child control of a 2D vector, for example, so as to get a magnitude
    // input control in addition to the X and Y component controls. The state is shared with the
    // vector itself.
    public class Magnitude2Control : AxisControl
    {
        public Magnitude2Control()
        {
            m_StateBlock.format = InputStateBlock.kTypeVector2;
        }

        protected override unsafe float ReadRawValueFrom(IntPtr statePtr)
        {
            var valuePtr = (float*)new IntPtr(statePtr.ToInt64() + (int)m_StateBlock.byteOffset);
            var x = valuePtr[0];
            var y = valuePtr[1];
            return new Vector2(x, y).magnitude;
        }
    }
}
