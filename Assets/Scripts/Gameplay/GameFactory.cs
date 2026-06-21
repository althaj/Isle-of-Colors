using UnityEngine;
using Zenject;
using PSG.IsleOfColors.Managers;
using System.Linq;
using System.Collections.Generic;

namespace PSG.IsleOfColors.Gameplay
{
    public class GameFactory : MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;
        [SerializeField] private List<Transform> playerParents = new();

        [Inject] private ApplicationManager _applicationManager;
        [Inject] private DiContainer _container;

        public IEnumerable<Player> InitializePlayers()
        {
            IEnumerable<Player> result = Enumerable.Empty<Player>();

            if(_applicationManager.GameOptions.Players.Count < 2 || _applicationManager.GameOptions.Players.Count > 4)
            {
                Debug.LogError($"[GameFactory::InitializePlayer] Number of players must be between 2 and 4, but is {_applicationManager.GameOptions.Players.Count}.");
                return result;
            }

            if(playerParents.Count < _applicationManager.GameOptions.Players.Count)
            {
                Debug.LogError($"[GameFactory::InitializePlayer] Not enough player positions, need {_applicationManager.GameOptions.Players.Count}, have {playerParents.Count}.");
                return result;
            }
            
            for(int i = 0; i < _applicationManager.GameOptions.Players.Count; i++)
            {
                Player player = _container.InstantiatePrefab(playerPrefab, playerParents[i]).GetComponent<Player>();
                player.Initialize(_applicationManager.GameOptions.Players[i]);

                result = result.Append(player);
            }

            return result;
        }
    }
}