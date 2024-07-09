using RNGManager;
using System.Collections.Generic;
using UnityEngine;

namespace PSG.IsleOfColors.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Player player1;
        [SerializeField] private Player player2;
        [SerializeField] private List<PencilColor> colors;

        public Player Player1 { get => player1; }
        public Player Player2 { get => player2; }
        public List<PencilColor> Colors { get => colors; set => colors = value; }

        private void Awake()
        {
            RNGManager.RNGManager.Manager.AddInstance(new RNGInstance(title: "Game"));
        }

        public bool IsGameFinished()
        {
            return false;
        }
    }
}
