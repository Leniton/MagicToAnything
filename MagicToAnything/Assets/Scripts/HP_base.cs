using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_base : MonoBehaviour
{
    public int MAX_HP;
    public int HP;

    [SerializeField] bool Invicible;
    [SerializeField] bool BlockHeal;
 
    public void Damage(int dmg)
    {
        if (Invicible) return;

        HP -= dmg;
    }

    public void Heal(int hl)
    {
        if (BlockHeal) return;

        HP += hl;
        if(HP > MAX_HP)
        {
            HP = MAX_HP;
        }
    }
}
