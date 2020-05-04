using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveMagic : MonoBehaviour
{
    public static SaveMagic saveMagic;

    public MagicData[] Magics;
    [Space]
    public GameObject[] BaseMagicTypes;

    void Awake()
    {
        if(saveMagic != this)
        {
            if(saveMagic != null)
            {
                Destroy(this);
            }
            else
            {
                saveMagic = this;
                Magics = LoadAll();
            }
        }
    }

    public void Save(MagicData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Configs"))
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Configs");
        }
        string caminho = Directory.GetCurrentDirectory() + "\\Configs\\" + data.Name + ".mgc";
        print(caminho);
        FileStream file = new FileStream(caminho, FileMode.Create);
        formatter.Serialize(file, data);
        file.Close();

        //adicionar magia
        Magics = LoadAll();
    }

    //Directory.GetFiles(string caminho, "*.extensão");

    public MagicData[] LoadAll()
    {
        List<MagicData> magicDatas = new List<MagicData>();
        string caminho = Directory.GetCurrentDirectory() + "\\Configs";

        if (!Directory.Exists(caminho))
        {
            Directory.CreateDirectory(caminho);
        }

        string[] files = Directory.GetFiles(caminho, "*.mgc");

        for (int i = 0; i < files.Length; i++)
        {
            //print(files[i]);
            magicDatas.Add(Load(files[i]));
        }

        return magicDatas.ToArray();
    }

    public MagicData Load(string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);

            MagicData data = formatter.Deserialize(file) as MagicData;
            file.Close();

            return data;
        }
        else
        {
            Debug.LogError("não encontrado");
            return null;
        }
    }
}