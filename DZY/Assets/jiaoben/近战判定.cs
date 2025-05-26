using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 近战判定 : MonoBehaviour
{

    public bool gj=false;
    public Animator animator;
    public GameObject jzpd;
    public GameObject ccpd;
    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.J))
        {
            animator.SetTrigger("gjz");
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.L))
        {
            animator.SetTrigger("cc");
        }
    }
    private void gjz()
    {
        jzpd.SetActive(true);
    }
    private void gjz2()
    {
        jzpd.SetActive(false);
    }

    private void cc()
    {
        ccpd.SetActive(true);
    }

    private void ccjs()
    {
        ccpd.SetActive(false);
    }
}
