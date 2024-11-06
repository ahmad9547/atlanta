using UnityEngine;
using UnityEngine.UI;
using Core.UI;
using System;

namespace PlayerUIScene
{
	public sealed class MenuOptionButton : MonoBehaviour
	{
		[SerializeField] private Button _menuOptionButton;
		[SerializeField] private UIController _menuOptionController;

		public event Action<MenuOptionButton> OnMenuOptionSelected;

		private void OnEnable()
		{
			_menuOptionButton.onClick.AddListener(OnMenuOptionButtonClick);
		}

		private void OnDisable()
		{
			_menuOptionButton.onClick.RemoveListener(OnMenuOptionButtonClick);
		}		

		public void SetActive(bool state)
		{
			(state ? (Action)_menuOptionController.Show : _menuOptionController.Hide).Invoke();
		}

		private void OnMenuOptionButtonClick()
		{
			OnMenuOptionSelected?.Invoke(this);
		}
	}
}
