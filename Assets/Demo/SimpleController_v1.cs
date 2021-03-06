using ISX;
using UnityEngine;

// Using state of gamepad device directly.
public class SimpleController_v1 : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;

    private Vector2 m_Rotation;

    public void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return;

        var leftStick = gamepad.leftStick.value;
        var rightStick = gamepad.rightStick.value;

        Move(leftStick);
        Look(rightStick);
    }

    private void Move(Vector2 direction)
    {
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        var move = transform.TransformDirection(direction.x, 0, direction.y);
        transform.localPosition += move * scaledMoveSpeed;
    }

    private void Look(Vector2 rotate)
    {
        var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
        m_Rotation.y += rotate.x * scaledRotateSpeed;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
        transform.localEulerAngles = m_Rotation;
    }
}
