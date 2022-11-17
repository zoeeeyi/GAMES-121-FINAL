using Sirenix.OdinInspector;
using UnityEngine;

public interface Skills
{
    public enum SkillType
    {
        DoubleJump,
        Dash,
        Parry,
        Rewind,
        SlowDescend
    }

    protected void DoubleJump(float _jumpForce, ForceMode2D _jumpMode);
    protected void Dash();
    protected void Parry();
    protected void SlowDescend();
}
