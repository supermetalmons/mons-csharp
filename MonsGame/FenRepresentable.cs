// ∅ 2024 super-metal-mons

namespace MonsGame;

interface IFenRepresentable
{
    string Fen { get; }
}

public partial class Game : IFenRepresentable
{
    public string Fen
    {
        get
        {
            var fields = new string[]
            {
                WhiteScore.ToString(),
                BlackScore.ToString(),
                ActiveColor.Fen(),
                ActionsUsedCount.ToString(),
                ManaMovesCount.ToString(),
                MonsMovesCount.ToString(),
                WhitePotionsCount.ToString(),
                BlackPotionsCount.ToString(),
                TurnNumber.ToString(),
                Board.Fen
            };
            return string.Join(" ", fields);
        }
    }

}

public partial class Board : IFenRepresentable
{
    public string Fen
    {
        get
        {
            return ""; // TODO: implement
        }
    }

}
