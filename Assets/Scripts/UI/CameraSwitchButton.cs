using PSG.IsleOfColors.Gameplay;
using UnityEngine;
using Zenject;

public class CameraSwitchButton : MonoBehaviour
{
    [Inject] private GameManager _gameManager;

    public void SwitchPlayer()
    {
        _gameManager.SwitchPlayer();
    }
}
