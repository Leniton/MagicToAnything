using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{

    public enum TypeMagic { Trap, Ball, Target }
    public enum TargetMagic { Self, Ally, Enemy, Any }
    public enum EffectMagic { Wind, Damage, Heal, Poison }

    public TypeMagic Tipo;
    public float TypeM;

    public TargetMagic Alvo;

    public EffectMagic Efeito;
    public float EfffectM = 1;

    bool Moving;

    void Start()
    {
        Activate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            transform.Translate(transform.forward * 1 * TypeM * Time.deltaTime);
            Debug.DrawLine(transform.position, transform.forward * 8, Color.white, 0.1f);
        }
    }

    public void Activate()
    {
        //tipo
        switch (Tipo)
        {
            case TypeMagic.Trap:
                StartCoroutine(MagicTrap());
                break;
            case TypeMagic.Ball:
                MagicBall();
                break;
            case TypeMagic.Target:
                MagicTarget();
                break;
            default:
                break;
        }
    }

    #region Type
    IEnumerator MagicTrap()
    {
        //print("Trap setled");
        float time = 0;
        SpriteRenderer r = GetComponent<SpriteRenderer>();
        //Material m = GetComponent<MeshRenderer>().material;
        Color c = r.color;
        do
        {
            if(TypeM != 0 && time != 0)
            c.a += (TypeM/time)/1000;

            r.color = c;
            //print(c.a);
            if(TypeM - time < 0.5f)
            {
                c.a = 0.9f;
                r.color = c;
            }
            yield return null;
            time += Time.deltaTime;
        } while (time < TypeM);
        
        GetComponent<Collider>().enabled = true;
        Destroy(gameObject, 0.5f);
    }

    void MagicBall()
    {
        //PRECISA DE TESTE!!
        //gameObject.AddComponent<SphereCollider>();
        //transform.Rotate(45, 0, 0);
        Moving = true;
    }

    void MagicTarget()
    {

    }
    #endregion

    void OnTriggerEnter2D(Collider2D c)
    {
        print("boom");
        print(c);
        //checar o alvo
        //ativar efeito
        switch (Efeito)
        {
            case EffectMagic.Wind:
                Wind(c.GetComponent<Rigidbody>());
                break;
            case EffectMagic.Damage:
                break;
            case EffectMagic.Heal:
                break;
            case EffectMagic.Poison:
                break;
            default:
                break;
        }
    }

    #region Effect

    void Wind(Rigidbody rb)
    {
        rb.AddForce(transform.up * 10 * EfffectM, ForceMode.Impulse);
    }


    #endregion

}