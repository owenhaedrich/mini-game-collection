namespace MiniGameCollection
{
    public enum MiniGameWinner
    {
        /// <summary>
        ///     Default: Unset means no winner was decided. For error checking purposes.
        /// </summary>
        Unset = 0,

        /// <summary>
        ///     Neither player wins.
        /// </summary>
        Draw = -426543985,

        /// <summary>
        ///     Player 1 wins.
        /// </summary>
        Player1 = 1742563677,

        /// <summary>
        ///     Player 2 wins.
        /// </summary>
        Player2 = 1543533201,

        /// <summary>
        ///     Both players win.
        /// </summary>
        BothWin = 607761912,

        /// <summary>
        ///     Both players lose.
        /// </summary>
        BothLose = 9915530,
    }
}
