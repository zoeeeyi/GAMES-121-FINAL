using Sirenix.OdinInspector;
using UnityEngine;

public interface interface_Skills
{
    public enum SkillType
    {
        DoubleJump,
        Dash,
        Parry,
        Rewind,
        SlowDescend
    }

    public void DoubleJump(float _jumpForce, ForceMode2D _jumpMode);
    public void Dash();
    public void Parry();
    public void SlowDescend();
}
