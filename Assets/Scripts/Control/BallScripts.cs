using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallScripts : MonoBehaviour
{
    [SerializeField] private GameObject[] theGoal;
    [SerializeField] private Rigidbody ballRB;
    [SerializeField] private GameObject particle;
    private void OnCollisionEnter(Collision collision)
    {
        foreach (GameObject go in theGoal)
        {
            if(collision.gameObject == go)
            {                
                ballRB.velocity = Vector3.zero;
                Vector3 goalPos = go.transform.position + Vector3.up * 2f;
                SpawnParticle(goalPos);
            }
        }
    }
    public void SpawnParticle(Vector3 position)
    {
        GameObject fx = Instantiate(particle, position, Quaternion.identity);

        // Play all particle systems inside (including root and children)
        ParticleSystem[] allParticles = fx.GetComponentsInChildren<ParticleSystem>();
        float maxDuration = 0f;

        foreach (var ps in allParticles)
        {
            ps.Play();
            float duration = ps.main.duration + ps.main.startLifetime.constantMax;
            if (duration > maxDuration)
                maxDuration = duration;
        }

        Destroy(fx, maxDuration + 0.5f);
    }
}
