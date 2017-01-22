using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Movement")]
    public bool swimming = false;

    protected Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        anim.speed = Random.Range(0.5f, 2.0f);
    }

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {

    }

    protected virtual void UpdateCharacter()
    {
        if (swimming) anim.SetBool("Help", true);
        else anim.SetBool("Help", false);
    }

    void Update()
    {
        UpdateCharacter();
    }

}
