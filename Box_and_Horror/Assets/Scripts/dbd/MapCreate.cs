﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapCreate : MonoBehaviour
{
    [SerializeField] TextAsset textAsset;
    [SerializeField] GameObject mapObject;
    //ミニマップ用。場所は変更する
    [SerializeField] GameObject minimapCamera;
    [SerializeField] GameObject minimap;

    GameObject endPosition;

    [SerializeField] MapController mapController;
    [SerializeField] README _r;
    // Start is called before the first frame update
    void Start()
    {
        //本来ここでtextAssetの読み込み。多分managerから
        //csvの読み込み
        TextAssetReader();
        //TextAssetReader();
    }


    private void Update()
    {
        //マップ表示。書く場所はあとでplayerに行くだろうな
        if (Input.GetKey(KeyCode.Q))
        {
            minimapCamera.SetActive(true);
            minimap.SetActive(true);
        }
        else
        {
            minimapCamera.SetActive(false);
            minimap.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //なびめっしゅさがし
            _r.enabled = true;
        }
    }

    private void TextAssetReader()
    {
        StreamReader reader = new StreamReader((Application.dataPath + "/Resources/CSV/" + textAsset.name + ".csv"));
        int cnt = 0;
        int num = 0;
        GameObject obj = new GameObject();
        while (reader.Peek() > -1)
        {
            string[] str = reader.ReadLine().Split(',');
            for (int i = 0; i < str.Length; i++)
            {
                if (int.Parse(str[i]) != 1)
                {
                    GameObject a = InstanceFloor(new Vector3(i, 0, cnt), int.Parse(str[i]));
                    a.transform.parent = obj.transform;
                    a.name = cnt + ":" + i;
                }
            }
            cnt++;
            num = str.Length;
        }
        mapController.SetGlidInfo(new int[2] { cnt, num }, textAsset);
        minimapCamera.transform.localPosition = new Vector3((float)cnt / 2f, 10, (float)num / 2f);
        
        if(endPosition != null)
        {
            //Vector3 vec = endPosition.transform.localPosition - ;
        }
    }

    private GameObject InstanceFloor(Vector3 pos,int num)
    {
        //オブジェクトに持たせるかもしれないし変わる可能性アリ
        GameObject obj = Instantiate(mapObject, pos, Quaternion.identity);
        switch (num)
        {
            
            case 0:
                obj.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1);
                break;
            case 2:
                obj.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
                break;
            case 3:
                obj.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0);
                break;
            case -1:
                obj.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
                obj.transform.localScale += new Vector3(0,10,0);
                break;
            case -2:
                obj.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
                obj.GetComponent<BoxCollider>().size = new Vector3(1, 0.1f, 1);
                obj.GetComponent<BoxCollider>().center = new Vector3(0,-0.5f,0);
                obj.transform.localScale += new Vector3(0, 10, 0);
                endPosition = obj;
                break;
            case -3:
                obj.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 1);
                obj.GetComponent<BoxCollider>().size = new Vector3(1, 0.1f, 1);
                obj.GetComponent<BoxCollider>().center = new Vector3(0, -0.5f, 0);
                obj.transform.localScale += new Vector3(0, 10, 0);
                break;
                
        }
        return obj;
    }
    
    
}
