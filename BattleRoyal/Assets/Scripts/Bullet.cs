using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private float speed;
    private int attackerId;
    private bool isMine;
    

    public Rigidbody rb;

    public void Initialize(int damage,float speed, int attackerId, bool isMine)
    {
        this.damage = damage;
        this.speed = speed;
        this.attackerId = attackerId;
        this.isMine = isMine;

        Destroy(gameObject, 10);
    }
    public float GetSpeed()
    {
        return speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isMine)
        {
            PlayerController player = GameManager.instance.GetPlayer(other.gameObject);
            if (player.id != attackerId)
            {
                player.photonView.RPC("TakeDamage", player.photonPlayer, attackerId, damage);
            }
        }
        Destroy(gameObject);
    }

}
