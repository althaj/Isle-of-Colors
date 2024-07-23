namespace PSG.IsleOfColors.Gameplay.Scoring
{
    public interface IScoring
    {
        public string GetName();
        public PencilColor GetColor();
        public string GetDescription();
        public int GetScore(PlayerSheet playerSheet);
    }
}