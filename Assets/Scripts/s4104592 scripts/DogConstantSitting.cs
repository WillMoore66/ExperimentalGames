using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogConstantSitting : MonoBehaviour
{
	Animator animator;

    void Start()
    {
		animator = GetComponent<Animator>();
		animator.Play("Dog.DogAnimations", 0, 165f / 450f);
		StartCoroutine("StopAnim");
	}

	IEnumerator StopAnim() {
		yield return new WaitForSeconds(0.05f);
		animator.speed = 0;
	}
}
