using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavemanAnim : StateMachineBehaviour
{
	[Header("References")]
	public AudioClip gruntSound = default;

	private AudioSource _soundSource;

	private AudioSource SetupAudioSource(Transform searchObject)
	{
		_soundSource = searchObject.GetComponent<AudioSource>();
		if (!_soundSource) { _soundSource = searchObject.gameObject.AddComponent<AudioSource>(); }; // Agregar si es necesario
		_soundSource.playOnAwake = false;
		_soundSource.loop = false;
		_soundSource.spatialBlend = 0;
		return _soundSource;
	}

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		if (!_soundSource)
		{
			_soundSource = SetupAudioSource(animator.transform);
		};

		if (gruntSound)
		{
			_soundSource.PlayOneShot(gruntSound);
		};

		Debug.Log("inicia estado: " + stateInfo.ToString());

	}


}
