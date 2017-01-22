using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : Character
{
    public float playerLookAtRange = 4.0f;
    protected override void Init()
    {
        base.Init();

    }

    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();
        foreach (Player player in GameManager.Instance.players)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < playerLookAtRange)
            {
                Quaternion lookat = Quaternion.LookRotation((player.transform.position - transform.position).normalized, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookat, Time.time * 0.1f);
                break;
            }
        }
    }

}
