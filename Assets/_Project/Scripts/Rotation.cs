using UnityEngine;

public class Rotation
{
    private int currentRotation = 0;

    public int GetRotationValue()
    {
        return currentRotation;
    }
    public void Rotate(RotationDirection direction, int rotationAmount)
    {
        if (direction == RotationDirection.Clockwise) currentRotation += rotationAmount;
        else currentRotation -= rotationAmount;
        ClampRotation();
    }

    private void ClampRotation()
    {
        currentRotation = GameUtils.ModPositive(currentRotation, 4);
    }
}
public enum RotationDirection
{
    Clockwise,
    Counterclockwise
}