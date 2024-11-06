using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayCanVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem sprayVfx;
    [SerializeField] private MeshRenderer sprayCan;
    public Animator animator;

    public void SetColor(Color32 color)
    {
        sprayCan.materials[1].color = color;
#pragma warning disable CS0618 // Type or member is obsolete
        sprayVfx.startColor = color;
#pragma warning restore CS0618 // Type or member is obsolete
    }

    public void ShowFingerAnim()
    {
        animator.SetBool("SprayBool", true);
		animator.Play("Spray");
	}

	public void EnableSprayVFX()
    {
        sprayVfx.Play();
    }

    public void DisableSprayVFX()
    {
        sprayVfx.Stop();
        animator.SetBool("SprayBool", false);
	}
}