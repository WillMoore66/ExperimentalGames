using UnityEngine;

public class DogMenuAnimation : MonoBehaviour
{
	Animator animator;
	float counter;

		private void Start() {
		animator = GetComponent<Animator>();
	}

	void FixedUpdate()
    {
		float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 450;
		if (time < 370 && counter < 400 || time > 393 && counter < 400) {
			animator.Play("Dog.DogAnimations", 0, 370f / 450f);
		}
		if (counter % 400 == 0) {
			animator.Play("Dog.DogAnimations", 0, 130f / 450f);
		}
		if (counter % 430 == 0) {
			animator.speed = 0;
		}
		if (counter % 630 == 0) {
			animator.speed = 1;
			animator.Play("Dog.DogAnimations", 0, 165f / 450f);
		}
		if (counter % 650 == 0) {
			counter = 0;
		}
		counter += 0.5f;
	}
}
