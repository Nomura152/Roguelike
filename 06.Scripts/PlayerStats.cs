using System;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    [Tooltip("ƒŒƒxƒ‹"), SerializeField] int level = 1;
    [Tooltip("Šl“¾ŒoŒ±’l"), SerializeField] int exp = 0;

    [Tooltip("Œ»İ‚Ì‘Ì—Í"), SerializeField] float hp = 100f;
    [Tooltip("Å‘å‘Ì—Í"), SerializeField] float maxHp = 100f;
    [Tooltip("UŒ‚—Í"), SerializeField] float strength = 1f;
    [Tooltip("ˆÚ“®‘¬“x"), SerializeField] float speed = 1f;

    [Tooltip("’e‘¬”{—¦"), SerializeField] float projectileSpeedMultiplier = 1f;
    [Tooltip("”­ËŠÔŠu”{—¦"), SerializeField] float fireIntervalMultiplier = 1f;
    [Tooltip("’eƒTƒCƒY”{—¦"), SerializeField] float projectileScaleMultiplier = 1f;

    // --- ƒvƒƒpƒeƒB ---

    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }

    public int Exp
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
        }
    }

    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            //ClampŠÖ”‚ÅA“n‚³‚ê‚½value‚Ì’l‚ğ0`maxHp‚Ì”ÍˆÍ‚Éû‚ß‚Ä‘ã“ü‚·‚é
            hp = Mathf.Clamp(value, 0f, maxHp);
        }
    }

    public float MaxHp
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
        }
    }

    public float Strength
    {
        get
        {
            return strength;
        }
        set
        {
            strength = value;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    public float ProjectileSpeedMultiplier
    {
        get
        {
            return projectileSpeedMultiplier;
        }
        set
        {
            projectileSpeedMultiplier = value;
        }
    }

    public float FireIntervalMultiplier
    {
        get
        {
            return fireIntervalMultiplier;
        }
        set
        {
            fireIntervalMultiplier = value;
        }
    }

    public float ProjectileScaleMultiplier
    {
        get
        {
            return projectileScaleMultiplier;
        }
        set
        {
            projectileScaleMultiplier = value;
        }
    }
}