using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask; //Playerの中からタンクだけを参照
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f; //最大ダメージ                 
    public float m_ExplosionForce = 1000f; //           
    public float m_MaxLifeTime = 2f; //砲弾の存続時間                 
    public float m_ExplosionRadius = 5f; //爆発の際に砲弾から影響が及ぶ範囲             


    private void Start()
    {
        //地面などに当たらず存続している場合、破棄する
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.

        //架空の球の内側、または重なったすべてのコライダーを取得
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        //コライダーの配列を見つけたコライダーの数だけループする
        for(int i = 0; i < colliders.Length; i++)
        {
            //衝突したオブジェクトのリジッドボディを取得
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            //実際にリジッドボディがあったかどうかを確認する
            if (!targetRigidbody)
            
                continue;

                //力を加える
                targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

                //タンクにダメージを与えるためにタンクの体力を管理するスクリプトを取得
                TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

                if (!targetHealth)
                    continue;
                
                //与えるダメージを取得
                float damage = CalculateDamage(targetRigidbody.position);

                //ダメージを与える
                targetHealth.TakeDamage(damage);
            
        }

        //砲弾を破棄したあとも子オブジェクトのパーティクルとオーディオが再生されるために、砲弾のオブジェクトとパーティクル・オーディオを選別する = 親から解除する
        m_ExplosionParticles.transform.parent = null;

        m_ExplosionParticles.Play();

        m_ExplosionAudio.Play();

        //パーティクルが終了するのを待ってから破棄する
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);
        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.

        //ターゲットから砲弾までのベクトルを作成（ターゲットの位置 - 砲弾の位置）、
        Vector3 explosionToTarget = targetPosition - transform.position;

        //ベクトルの長さ・大きさを求める = 0と半径の間
        float explosionDistance = explosionToTarget.magnitude;

        //相対的な距離　0~半径の間に見つけた距離ではなく、0~1までの数で表したい
        float rerativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        float damage = rerativeDistance * m_MaxDamage;

        //damageがマイナス値なら0に設定して、それ以外は何もしない
        damage = Mathf.Max(0f, damage);

        
        return damage;
    }
}