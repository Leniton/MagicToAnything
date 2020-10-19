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

    //para magias que precisam segurar

    bool FirstClick = true;
    Vector3 pointAxis;
    int SidePoint;

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
        //transform.eulerAngles = new Vector3(0, 0, Vector3.Angle((Vector3.right), MousePos - axis.position) * lado);
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

                if (c.gameObject != TrgtMagicTarget && c.GetComponent<TopDown_Movement>() != null)
                {
                    TrgtMagicTarget = c.gameObject;
                    Dot.transform.GetChild(0).gameObject.SetActive(true);
                }
                Dot.transform.GetChild(0).position = TrgtMagicTarget.transform.position;
            }
            else
            {
                if(magic.Effect != (int)Magic.EffectMagic.Wind || FirstClick)
                {
                    TrgtMagicTarget = null;
                }

                if(TrgtMagicTarget != null)
                {

                    Dot.transform.GetChild(0).position = TrgtMagicTarget.transform.position;
                }
                else
                {
                    Dot.transform.GetChild(0).gameObject.SetActive(false);
                }

            }
        }

        //trocar a magia
        if (Input.mouseScrollDelta.y != 0)
        {
            //um erro pode acontecer caso o scroll delta não seja 1 e -1
            ChangeMagic(IdMmagic + (int)Input.mouseScrollDelta.y);
        }

        //caso não seja vento, magia de vento precisa segurar para escolher a direção
        if (magic.Effect != (int)Magic.EffectMagic.Wind || magic.Type == (int)Magic.TypeMagic.Ball)
        {
            //usar a magia
            if (Input.GetMouseButtonDown(0))
            {
                switch (magic.Type)
                {
                    case (int)Magic.TypeMagic.Circle:

                        CastCircle(MousePos);
                        break;
                    case (int)Magic.TypeMagic.Ball:

                        CastBall(new Vector3(0, 0, Vector3.Angle((Vector3.up), MousePos) * lado));
                        break;
                    case (int)Magic.TypeMagic.Target:
                        if (TrgtMagicTarget != null)
                        {
                            CastTarget(TrgtMagicTarget);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                if (FirstClick)
                {
                    pointAxis = MousePos;

                    Dot.GetComponent<Image>().enabled = false;
                    Dot.transform.GetChild(1).gameObject.SetActive(true);

                    FirstClick = false;
                }

                if (Cam.ScreenToWorldPoint(Input.mousePosition).y - pointAxis.y <= 0)
                {
                    SidePoint = -1;
                }
                else
                {
                    SidePoint = 1;
                }

                Dot.transform.GetChild(1).position = pointAxis;
                //Dot.transform.GetChild(1).eulerAngles = new Vector3(0, 0, Vector3.Angle((Vector3.right), MousePos) * SidePoint);
                Dot.transform.GetChild(1).eulerAngles = new Vector3(0, 0, Vector3.Angle((Vector3.right), MousePos - pointAxis) * SidePoint);
            }

            if (Input.GetMouseButtonUp(0))
            {
                switch (magic.Type)
                {
                    case (int)Magic.TypeMagic.Circle:

                        CastCircle(pointAxis);
                        break;
                    case (int)Magic.TypeMagic.Ball:

                        CastBall(new Vector3(0, 0, Vector3.Angle((Vector3.up), MousePos) * lado));
                        break;
                    case (int)Magic.TypeMagic.Target:
                        if (TrgtMagicTarget != null)
                        {
                            CastTarget(TrgtMagicTarget);

                            TrgtMagicTarget = null;
                            Dot.transform.GetChild(0).gameObject.SetActive(false);
                        }
                        break;
                    default:
                        break;
                }

                Dot.GetComponent<Image>().enabled = true;
                Dot.transform.GetChild(1).gameObject.SetActive(false);

                FirstClick = true;
                //TrgtMagicTarget = null;
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
            Dot.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void CastCircle(Vector3 info)
    {
        GameObject Createdmagic = Instantiate(SaveMagic.saveMagic.BaseMagicTypes[0], info, Quaternion.identity);

        if(magic.Effect == (int)Magic.EffectMagic.Wind)
        {
            Createdmagic.transform.eulerAngles = Dot.transform.GetChild(1).eulerAngles;
            Createdmagic.transform.Rotate(new Vector3(0, 0, -90));
            Createdmagic.transform.GetChild(0).transform.up = Vector3.up;
        }

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

        if (magic.Effect == (int)Magic.EffectMagic.Wind)
        {
            CreatedMagic.transform.eulerAngles = Dot.transform.GetChild(1).eulerAngles;
            CreatedMagic.transform.Rotate(new Vector3(0, 0, -90));
        }

        Magic m = CreatedMagic.GetComponent<Magic>();
        m.Tipo = (Magic.TypeMagic)magic.Type;
        m.TypeM = magic.TModifier;
        m.Alvo = (Magic.TargetMagic)magic.Target;
        m.Efeito = (Magic.EffectMagic)magic.Effect;
        m.EfffectM = magic.EModifier;
    }
}
