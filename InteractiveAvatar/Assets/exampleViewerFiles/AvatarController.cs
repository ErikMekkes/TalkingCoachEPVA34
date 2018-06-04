using UnityEngine;
using System.Collections;

namespace tm
{
		public class AvatarController : MonoBehaviour
		{
				public GameObject ed_targetAvatar;

				public GameObject[] hairs;
				public GameObject[] glasses;
				public GameObject[] outfits;

				public GameObject[] hideObjects;

				private string[] animationNames;    

				private GameObject wornHair = null;
				private GameObject wornGlasses = null;
				private GameObject wornOutfit = null;

				private int animIndex = -1;
				private int hairIndex = -1;
				private int glassesIndex = -1;
				private int outfitIndex = -1; 

				void Start ()
				{
						animationNames = new string[ ed_targetAvatar.GetComponent<Animation>().GetClipCount ()];
            
						int i = 0;
						foreach (AnimationState animState in ed_targetAvatar.GetComponent<Animation>()) {
								animationNames [i] = animState.clip.name;
								i++;
						}

						playNextAnimation ();
						changeNextHair ();
						changeNextGlasses ();
						changeNextOutfit ();
				}

				public void pauseAnimation (bool value)
				{
						if (!value) {
								ed_targetAvatar.GetComponent<Animation>() [animationNames [animIndex]].speed = 0;

								if (wornHair != null)
										wornHair.GetComponent<Animation>() [animationNames [animIndex]].speed = 0;
								if (wornGlasses != null)
										wornGlasses.GetComponent<Animation>() [animationNames [animIndex]].speed = 0;
								if (wornOutfit != null)
										wornOutfit.GetComponent<Animation>() [animationNames [animIndex]].speed = 0;
						} else {
								ed_targetAvatar.GetComponent<Animation>() [animationNames [animIndex]].speed = 1;
								if (wornHair != null)
										wornHair.GetComponent<Animation>() [animationNames [animIndex]].speed = 1;
								if (wornGlasses != null)
										wornGlasses.GetComponent<Animation>() [animationNames [animIndex]].speed = 1;
								if (wornOutfit != null)
										wornOutfit.GetComponent<Animation>() [animationNames [animIndex]].speed = 1;
						}
				}

				public void playNextAnimation ()
				{
						if (animIndex != -1) {
								if (ed_targetAvatar.GetComponent<Animation>() [animationNames [animIndex]].speed == 0)
										pauseAnimation (true); 
						}

						animIndex = (animIndex + 1) % animationNames.Length;
						ed_targetAvatar.GetComponent<Animation>() [animationNames [animIndex]].wrapMode = WrapMode.Loop;
						ed_targetAvatar.GetComponent<Animation>().Play (animationNames [animIndex]);

						synchAnimation ();
				}

				public void changeNextHair ()
				{
						hairIndex = (hairIndex + 1) % hairs.Length;

						Destroy (wornHair);
            
						if (hairs [hairIndex] != null) {
								wornHair = Instantiate (hairs [hairIndex]) as GameObject;
								wornHair.transform.parent = this.transform;
								wornHair.transform.localScale = Vector3.one;
								wornHair.transform.localRotation = Quaternion.identity;
								wornHair.transform.localPosition = Vector3.zero;
                
								synchAnimation ();
						}
				}

				public void changeNextGlasses ()
				{
						glassesIndex = (glassesIndex + 1) % glasses.Length;

						Destroy (wornGlasses);
            
						if (glasses [glassesIndex] != null) {
								wornGlasses = Instantiate (glasses [glassesIndex]) as GameObject;
								wornGlasses.transform.parent = this.transform;
								wornGlasses.transform.localScale = Vector3.one;
								wornGlasses.transform.localRotation = Quaternion.identity;
								wornGlasses.transform.localPosition = Vector3.zero;
                
								synchAnimation ();
						}
				}	

				public void changeNextOutfit ()
				{
						outfitIndex = (outfitIndex + 1) % outfits.Length;

						Destroy (wornOutfit);
            
						if (outfits [outfitIndex] != null) {
								wornOutfit = Instantiate (outfits [outfitIndex]) as GameObject;
								wornOutfit.transform.parent = this.transform;
								wornOutfit.transform.localScale = Vector3.one;
								wornOutfit.transform.localRotation = Quaternion.identity;
								wornOutfit.transform.localPosition = Vector3.zero;
              
								foreach (GameObject go in hideObjects)
										go.SetActive (false);
  
								synchAnimation ();
						} else {
								foreach (GameObject go in hideObjects)
										go.SetActive (true);
						}
				} 

				public void synchAnimation ()
				{
						AnimationClip clip = ed_targetAvatar.GetComponent<Animation>() [animationNames [animIndex]].clip;
						clip.wrapMode = WrapMode.Loop;
						if (wornHair != null) {
								wornHair.GetComponent<Animation>().AddClip (clip, clip.name);
								wornHair.GetComponent<Animation>().Play (clip.name);
						}
						if (wornGlasses != null) {
								wornGlasses.GetComponent<Animation>().AddClip (clip, clip.name);
								wornGlasses.GetComponent<Animation>().Play (clip.name);
						}
						if (wornOutfit != null) {
								wornOutfit.GetComponent<Animation>().AddClip (clip, clip.name);   
								wornOutfit.GetComponent<Animation>().Play (clip.name);
						}

						StartCoroutine (waitThenSynch ());
				}

				IEnumerator waitThenSynch ()
				{
						yield return new WaitForEndOfFrame ();

						AnimationState animState = ed_targetAvatar.GetComponent<Animation>() [animationNames [animIndex]];

						if (wornHair != null) {
								wornHair.GetComponent<Animation>() [animState.clip.name].time = animState.time;
								wornHair.GetComponent<Animation>() [animState.clip.name].speed = animState.speed;
						}
						if (wornGlasses != null) {
								wornGlasses.GetComponent<Animation>() [animState.clip.name].time = animState.time;
								wornGlasses.GetComponent<Animation>() [animState.clip.name].speed = animState.speed;
						}
						if (wornOutfit != null) {
								wornOutfit.GetComponent<Animation>() [animState.clip.name].time = animState.time;
								wornOutfit.GetComponent<Animation>() [animState.clip.name].speed = animState.speed;
						}
				}

				public string getAnimName ()
				{
						return animationNames [animIndex];
				}

				public string getHairName ()
				{
						if (hairIndex == -1 || hairs [hairIndex] == null)
								return "none";
						return hairs [hairIndex].name;
				}

				public string getGlassesName ()
				{
						if (glassesIndex == -1 || glasses [glassesIndex] == null)
								return "none";
						return glasses [glassesIndex].name;
				}  

				public string getOutfitName ()
				{
						if (outfitIndex == -1 || outfits [outfitIndex] == null)
								return "none";
						return outfits [outfitIndex].name;
				}    
		}
}