using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiMagicCreation : MonoBehaviour
{
    [SerializeField] TMP_Dropdown TypeDropdown;
    [SerializeField] TextMeshProUGUI ModifierText;
    [SerializeField] TMP_InputField ModifierValue;

    MagicData NMagic = new MagicData("Magic 0", 0, 1, 3, 0, 1);

    void Start()
    {
        ChangeModifier(TypeDropdown.value);
    }

    public void MagicName(string name)
    {
        NMagic.Name = name;
    }

    public void ChangeModifier(int m)
    {
        NMagic.Type = m;
        switch (m)
        {
            case (int)Magic.TypeMagic.Circle:
                ModifierText.text = "Activation delay:";
                ModifierValue.placeholder.GetComponent<TextMeshProUGUI>().text = "Default value: 1";
                ValueModifier("1");
                break;
            case (int)Magic.TypeMagic.Ball:
                ModifierText.text = "Flight speed:";
                ModifierValue.placeholder.GetComponent<TextMeshProUGUI>().text = "Default value: 10";
                ValueModifier("10");
                break;
            case (int)Magic.TypeMagic.Target:
                ModifierText.text = "Target distance:";
                ModifierValue.placeholder.GetComponent<TextMeshProUGUI>().text = "Default value: 30";
                ValueModifier("30");
                break;
            default:
                break;
        }
    }

    public void ValueModifier(string valor)
    {
        if (valor == "") return;

        valor = valor.Replace('.', ',');
        NMagic.TModifier = float.Parse(valor);
        print("change mod: " + NMagic.TModifier);
    }

    public void CreateMagic()
    {
        SaveMagic.saveMagic.Save(NMagic);
        print("criar");
    }
}