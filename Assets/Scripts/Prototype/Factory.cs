using PSG.IsleOfColors.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.IsleOfColors.Prototype
{
    public class Factory : MonoBehaviour
    {
        [SerializeField] private List<Player> players;
        [SerializeField] private UI ui;

        void Start()
        {
            players.ForEach(p => p.Initialize());

            ui.Initialize();
        }
    }
}
