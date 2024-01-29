// ∅ 2024 super-metal-mons

namespace MonsGame;
public class Game
{
    public Board Board { get; private set; }

    public int WhiteScore { get; private set; }
    public int BlackScore { get; private set; }
    public Color ActiveColor { get; private set; }

    public int ActionsUsedCount { get; private set; }
    public int ManaMovesCount { get; private set; }
    public int MonsMovesCount { get; private set; }

    public int WhitePotionsCount { get; private set; }
    public int BlackPotionsCount { get; private set; }

    public int TurnNumber { get; private set; }

    public Game()
    {
        Board = new Board();
        WhiteScore = 0;
        BlackScore = 0;
        ActiveColor = Color.White; // Assuming Color.White is defined
        ActionsUsedCount = 0;
        ManaMovesCount = 0;
        MonsMovesCount = 0;
        WhitePotionsCount = 0;
        BlackPotionsCount = 0;
        TurnNumber = 1;
    }

    public Game(Board board,
                    int whiteScore,
                    int blackScore,
                    Color activeColor,
                    int actionsUsedCount,
                    int manaMovesCount,
                    int monsMovesCount,
                    int whitePotionsCount,
                    int blackPotionsCount,
                    int turnNumber)
    {
        Board = board;
        WhiteScore = whiteScore;
        BlackScore = blackScore;
        ActiveColor = activeColor;
        ActionsUsedCount = actionsUsedCount;
        ManaMovesCount = manaMovesCount;
        MonsMovesCount = monsMovesCount;
        WhitePotionsCount = whitePotionsCount;
        BlackPotionsCount = blackPotionsCount;
        TurnNumber = turnNumber;
    }
}
