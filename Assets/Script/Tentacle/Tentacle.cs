using UnityEngine;

public class Tentacle : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float attackRate = 4f;
    [SerializeField] private float attackTimer;//TODO private Later

    [Header("Particle System")]
    public ParticleSystem dustParticleSystem;
    public ParticleSystem stoneParticleSystem;
    public ParticleSystem boomParticleSystem;

    private void Start()
    {
        anim = GetComponent<Animator>();

        //attackTimer = attackRate;//TODO This Line will be Remove After Prefabs 4 objects 
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            anim.SetTrigger("isAttack");
            attackTimer = attackRate;

            boomParticleSystem.Play();

            stoneParticleSystem.Stop();
            dustParticleSystem.Stop();
        }
    }

    //MARKER This function will be called on the Last Frame of [the Attack AnimationClip]
    public void SpawnEffect()
    {
        dustParticleSystem.Play();
        stoneParticleSystem.Play();
    }

}

