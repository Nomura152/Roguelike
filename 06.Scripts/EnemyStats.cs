using System;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    [Tooltip("現在の体力"), SerializeField] float hp = 10;
    [Tooltip("最大体力"), SerializeField] float maxHp = 10;
    [Tooltip("攻撃力"), SerializeField] float strength = 1f;
    [Tooltip("移動速度"), SerializeField] float speed = 0.3f;

    // --- プロパティ ---

    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
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
}