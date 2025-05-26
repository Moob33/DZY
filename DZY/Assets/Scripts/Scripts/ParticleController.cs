using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] ParticleSystem ps;
    [Range(0f, 10f)]
    [SerializeField] float minPlaySpeed;
    [Range(0f, 0.2f)]
    [SerializeField] float dustInterval;
    float counter;
    private void Update()
    {
        counter += Time.deltaTime;
        if (Mathf.Abs(rb.velocity.x) > minPlaySpeed && counter > dustInterval)
        {
            ps.Play();
            counter = 0;
        }

    }
    // Start is called before the first frame update
    void Start()
    {

    }
}
    // Update is called once per frame
    
