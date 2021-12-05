using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    public Color m_PlayerColor; //プレイヤーが生成時に得る色            
    public Transform m_SpawnPoint; //生成位置        
    [HideInInspector] public int m_PlayerNumber;             
    [HideInInspector] public string m_ColoredPlayerText; 
    [HideInInspector] public GameObject m_Instance;          
    [HideInInspector] public int m_Wins;                     


    private TankMovement m_Movement;       
    private TankShooting m_Shooting;
    private GameObject m_CanvasGameObject;


    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        m_Movement.m_PlayerNumber = m_PlayerNumber;
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        //テキストを特定の色に色付けする
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        //オブジェクトについているすべてのRendererのコンポーネントを取得し、特定の色に色付け
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }

    
    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;

        //スクリプトの無効とキャンバスの無効
        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;

        //スクリプトの有効とキャンバスの有効
        m_CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        //インスタンスを生成位置に戻す
        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
