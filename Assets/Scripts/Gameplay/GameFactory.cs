using UnityEngine;
using Zenject;
using PSG.IsleOfColors.Managers;
using System;
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

        public IEnumerable<Player> InitializePlayers(int numberOfPlayers)
        {
            IEnumerable<Player> result = Enumerable.Empty<Player>();
            
            if(numberOfPlayers < 0)
            {
                Debug.LogError($"[GameFactory::InitializePlayer] Number of players cannot be negative.");
                return result;
            }

            if(playerParents.Count < numberOfPlayers)
            {
                Debug.LogError($"[GameFactory::InitializePlayer] Not enough player positions, need {numberOfPlayers}, have {playerParents.Count}.");
                return result;
            }

            
            for(int i = 0; i < numberOfPlayers; i++)
            {
                Player player = _container.InstantiatePrefab(playerPrefab, playerParents[i]).GetComponent<Player>();
                player.Initialize();

                if (!String.IsNullOrWhiteSpace(_applicationManager.GameOptions.Player1Name) && i == 0)
                {
                    player.Name = _applicationManager.GameOptions.Player1Name;
                }

                if (!String.IsNullOrWhiteSpace(_applicationManager.GameOptions.Player2Name) && i == 1)
                {
                    player.Name = _applicationManager.GameOptions.Player2Name;
                }

                result = result.Append(player);
            }

            return result;
        }
    }
}