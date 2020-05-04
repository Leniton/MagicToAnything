using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magic_Cast : MonoBehaviour
{

    [SerializeField] float Range;
    [SerializeField] MagicData magic;
    [SerializeField] int IdMmagic = 0;
    GameObject TrgtMagicTarget;

    Camera Cam;
    [SerializeField]GameObject Dot;
    int lado;
    Vector3 MousePos;
    [SerializeField] Transform axis;

    void Start()
    {
        Cam = Camera.main;
        magic = SaveMagic.saveMagic.Magics[IdMmagic];
        Cursor.visible = false;
    }

    private void OnDrawGizmos()
    {
        if (magic.Type == (int)Magic.TypeMagic.Target)
            Gizmos.DrawWireSphere(Dot.transform.position, 0.7f);
    }

    void Update()
    {
        //movimento mouse TESTE
        #region Mouse
        if (Cam.ScreenToWorldPoint(Input.mousePosition).x - axis.position.x <= 0)
        {
            lado = 1;
        }
        else
        {
            lado = -1;
        }

        MousePos = Cam.ScreenToWorldPoint(Input.mousePosition);
        //MousePos = new Vector3(MousePos.x, MousePos.y , 0);
        MousePos = new Vector3(MousePos.x - axis.position.x, MousePos.y - axis.position.y, 0);
        Dot.transform.position = MousePos;
        //transform.eulerAngles = new Vector3(0, 0, Vector3.Angle((Vector3.right), MousePos) * lado);
        /*if (Mathf.Abs(MousePos.x) > 0)
        {
            sprite.transform.localScale = new Vector3(-(MousePos.x / Mathf.Abs(MousePos.x)), 1, 1);
        }*/
        #endregion

        //marcar o alvo
        if (magic.Type == (int)Magic.TypeMagic.Target)
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(Dot.transform.position, 0.7f);
            

            //Collider2D c = Physics2D.OverlapCircle(Dot.transform.position, 0.7f);
            if (targets.Length >= 1)
            {
                Collider2D c = targets[0];
                for (int i = 1; i < targets.Length; i++)
                {
                    if (Vector2.Distance(c.transform.position, Dot.transform.position) >
                       Vector2.Distance(targets[i].transform.position, Dot.transform.position))
                    {
                        c = targets[i];
                    }

                }

                if (c.gameObject != TrgtMagicTarget)
                {
                    TrgtMagicTarget = c.gameObject;
                    Dot.transform.GetChild(0).gameObject.SetActive(true);
                }
                Dot.transform.GetChild(0).position = c.transform.position;
            }
            else
            {
                TrgtMagicTarget = null;
                Dot.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        //trocar a magia
        if (Input.mouseScrollDelta.y != 0)
        {
            //um erro pode acontecer caso o scroll delta não seja 1 e -1
            ChangeMagic(IdMmagic + (int)Input.mouseScrollDelta.y);
        }

        //usar a magia
        if (Input.GetMouseButtonDown(0))
        {
            switch (magic.Type)
            {
                case (int)Magic.TypeMagic.Trap:

                    CastTrap(MousePos);
                    break;
                case (int)Magic.TypeMagic.Ball:

                    CastBall(new Vector3(0, 0, Vector3.Angle((Vector3.up), MousePos) * lado));
                    break;
                case (int)Magic.TypeMagic.Target:
                    if(TrgtMagicTarget != null)
                    {
                        CastTarget(TrgtMagicTarget);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void ChangeMagic(int id)
    {
        if (id >= SaveMagic.saveMagic.Magics.Length)
        {
            id = 0;

        }
        else if (id < 0)
        {
            id = SaveMagic.saveMagic.Magics.Length - 1;
        }

        IdMmagic = id;
        magic = SaveMagic.saveMagic.Magics[IdMmagic];

        if (magic.Type == (int)Magic.TypeMagic.Target)
        {
            Dot.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            Dot.GetComponent<Image>().color = Color.white;
        }
    }

    void CastTrap(Vector3 info)
    {
        GameObject Createdmagic = Instantiate(SaveMagic.saveMagic.BaseMagicTypes[0], info, Quaternion.identity);

        Magic m = Createdmagic.GetComponent<Magic>();
        m.Tipo = (Magic.TypeMagic)magic.Type;
        m.TypeM = magic.TModifier;
        m.Alvo = (Magic.TargetMagic)magic.Target;
        m.Efeito = (Magic.EffectMagic)magic.Effect;
        m.EfffectM = magic.EModifier;
    }

    void CastBall(Vector3 Angle)
    {
        GameObject Createdmagic = Instantiate(SaveMagic.saveMagic.BaseMagicTypes[1], axis.position, Quaternion.identity);
        Createdmagic.transform.eulerAngles = Angle;
        print("..."+Createdmagic.transform.eulerAngles);

        Magic m = Createdmagic.GetComponent<Magic>();
        m.Tipo = (Magic.TypeMagic)magic.Type;
        m.TypeM = magic.TModifier;
        m.Alvo = (Magic.TargetMagic)magic.Target;
        m.Efeito = (Magic.EffectMagic)magic.Effect;
        m.EfffectM = magic.EModifier;
    }

    void CastTarget(GameObject trgt)
    {
        GameObject CreatedMagic = Instantiate(SaveMagic.saveMagic.BaseMagicTypes[2], trgt.transform);

        Magic m = CreatedMagic.GetComponent<Magic>();
        m.Tipo = (Magic.TypeMagic)magic.Type;
        m.TypeM = magic.TModifier;
        m.Alvo = (Magic.TargetMagic)magic.Target;
        m.Efeito = (Magic.EffectMagic)magic.Effect;
        m.EfffectM = magic.EModifier;
    }
}
