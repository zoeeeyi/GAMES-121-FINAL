using UnityEngine;

public interface MovementSkill_Interface
{
    public void DoubleJump(float _jumpForce, ForceMode2D _jumpMode);
    public void Dash();
    public void Parry();
    public void SlowDescend();
}
