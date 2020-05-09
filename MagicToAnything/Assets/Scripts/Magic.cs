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
        /*if (Moving)
        {
            transform.Translate(transform.forward * 1 * TypeM * Time.deltaTime);
            Debug.DrawLine(transform.position, transform.forward * 8, Color.white, 0.1f);
        }*/
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
                StartCoroutine(MagicBall());
                break;
            case TypeMagic.Target:
                StartCoroutine(MagicTarget());
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
        
        GetComponent<Collider2D>().enabled = true;
        Destroy(gameObject, 0.5f);
    }

    IEnumerator MagicBall()
    {
        //PRECISA DE TESTE!!
        //gameObject.AddComponent<SphereCollider>();
        //transform.Rotate(45, 0, 0);
        //Moving = true;
        yield return null;

        //print("ball cast");
        //print(transform.eulerAngles);

        GetComponent<Rigidbody2D>().AddForce(transform.up * TypeM, ForceMode2D.Impulse);

        Destroy(gameObject, 3);
    }

    IEnumerator MagicTarget()
    {
        yield return null;
        print("got you!! " + transform.parent);
        OnTriggerEnter2D(transform.parent.GetComponent<Collider2D>());
        yield return new WaitForSeconds(1);
        //aplicar efeito

        Destroy(gameObject);
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
                Wind(c.GetComponent<Rigidbody2D>());
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
        if(Tipo == TypeMagic.Ball)
        {
            Destroy(gameObject);
        }
    }

    
    #region Effect

    void Wind(Rigidbody2D rb)
    {
        rb.AddForce(transform.up * 10 * EfffectM, ForceMode2D.Impulse);
    }


    #endregion

}