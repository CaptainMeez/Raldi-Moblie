using System.Collections;
using GameJolt.API;
using UnityEngine;
using UnityEngine.UI;

namespace GameJolt.Demo.UI {
	public class UserInfoBox : MonoBehaviour {
		public Image Avatar;
		public Text Name;
		public Text UserAt;
		public Text AchivementText;

		public GameObject logIn;
		public GameObject logOut;

		private void Start() {
			StartCoroutine(UpdateRoutine());
		}

		private IEnumerator UpdateRoutine() {
			var wait = new WaitForSeconds(1f);
			
			yield return wait;

			while(enabled) {
				UpdateInfos();
				yield return wait;
			}
		}

		private void UpdateInfos() 
		{
			var user = GameJoltAPI.Instance.CurrentUser;

			if (user != null)
			{
				if (user.Avatar == null)
					user.DownloadAvatar();
					
				Avatar.sprite = user.Avatar;
				Name.text = user.DeveloperName;
				UserAt.text = "@" + user.Name;

				logIn.SetActive(false);
				logOut.SetActive(true);
			}
			else if (user == null)
			{
				Avatar.sprite = null;
				Name.text = "";

				logIn.SetActive(true);
				logOut.SetActive(false);
			}
		}
	}
}
