using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1; //どの入力軸を参照するのかを決めるために     
    public Rigidbody m_Shell;  //プレハブのshellにアクセスする          
    public Transform m_FireTransform; //砲弾を発射する場所   
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio; //発射のサウンドエフェクト 
    public AudioClip m_ChargingClip; //砲撃のチャージ用と発射用に    
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 15f; //最小の発射力
    public float m_MaxLaunchForce = 30f; //最大の発射力
    public float m_MaxChargeTime = 0.75f; //最小の発射から最大の発射までどのくらいかかるのか

     
    private string m_FireButton;  //発射ボタン用       
    private float m_CurrentLaunchForce;  //今までどれだけチャージしたかを追跡
    private float m_ChargeSpeed;         
    private bool m_Fired; //すでに発射したかどうか               


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        //Player1にはFire1 
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }
    

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        //スライダーの値をデフォルトに設定
        m_AimSlider.value = m_MinLaunchForce;

        
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            //最大までチャージしていて、まだ発射していない場合
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(m_FireButton))
        {
            //ボタンが最初に押されたか
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            //チャージに関連するオーディオを再生
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        else if(Input.GetButton(m_FireButton) && !m_Fired)
        {
           //最大チャージに達しておらず、ボタンを長押した状態
            //チャージ具合を増加
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            //スライダーを更新
            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if(Input.GetButtonUp(m_FireButton) && !m_Fired)
        {
            //まだ発射していない状態で、ボタンを離したかどうか
            Fire();
        }

    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        m_Fired = true;

        //リジッドボディをシーンにインスタンス化して配置し、それを速度を与えることのできる変数に設定
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}