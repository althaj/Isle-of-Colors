using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PSG.IsleOfColors.Gameplay
{
    public class Player : MonoBehaviour
    {
        public string Name { get => playerName; }
        [SerializeField] private string playerName;
        [SerializeField] private Map map;

        public List<PencilColor> Colors { get; private set; }
        public PlayerSheet PlayerSheet { get; private set; }

        public UnityEvent OnPlayerColorsChanged;

        public void UseColor(PencilColor color)
        {
            if(!Colors.Contains(color))
            {
                string message = $"UseColor: Player {Name} does not own the color {color}.";
                Debug.LogError(message, this);
                throw new System.ArgumentException(message);
            }

            Colors.Remove(color);
            OnPlayerColorsChanged?.Invoke();
        }

        public void AddColor(PencilColor color)
        {
            if (Colors.Contains(color))
            {
                string message = $"AddColor: Player {Name} already owns the color {color}.";
                Debug.LogError(message, this);
                throw new System.ArgumentException(message);
            }

            Colors.Add(color);
            OnPlayerColorsChanged?.Invoke();
        }

        internal void Initialize()
        {
            Colors = new List<PencilColor>();
            PlayerSheet = new PlayerSheet();
        }

        internal void GenerateMap()
        {
            PlayerSheet.GenerateMap(map);
            GetComponent<GameMap>().CreateMap();
        }
    }
}
