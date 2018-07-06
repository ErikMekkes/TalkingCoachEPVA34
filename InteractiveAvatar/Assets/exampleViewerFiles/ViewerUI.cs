using UnityEngine;
using System.Collections;

namespace tm
{
		public class ViewerUI : MonoBehaviour
		{
				public AvatarController ed_avatarController;
				public CameraWireframe ed_camWireframe;

				private bool showWireframe = false;
				private bool playingAnimation = true;

				void OnGUI ()
				{
						GUILayout.BeginVertical ();
						{
								if (GUILayout.Button ("Toggle Wireframe"))
										ed_camWireframe.doEffect = showWireframe = !showWireframe;
    
								if (GUILayout.Button ("Toggle Pause Play Animation")) {
										playingAnimation = !playingAnimation;
										ed_avatarController.pauseAnimation (playingAnimation);
								}
    
								GUILayout.BeginHorizontal ();
								{
										if (GUILayout.Button ("next animation")) {
												playingAnimation = true;
												ed_avatarController.playNextAnimation ();
										}
										GUILayout.Label (ed_avatarController.getAnimName ());
								}
								GUILayout.EndHorizontal ();

								GUILayout.BeginHorizontal ();
								{
										if (GUILayout.Button ("next hair"))
												ed_avatarController.changeNextHair ();
										GUILayout.Label (ed_avatarController.getHairName ());
								}
								GUILayout.EndHorizontal ();

								GUILayout.BeginHorizontal ();
								{
										if (GUILayout.Button ("next glasses item"))
												ed_avatarController.changeNextGlasses ();
										GUILayout.Label (ed_avatarController.getGlassesName ());
								}
								GUILayout.EndHorizontal ();

								GUILayout.BeginHorizontal ();
								{
										if (GUILayout.Button ("next outfit"))
												ed_avatarController.changeNextOutfit ();
										GUILayout.Label (ed_avatarController.getOutfitName ());
								}
								GUILayout.EndHorizontal ();
						}
						GUILayout.EndVertical ();
				}
		}
}