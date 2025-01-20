namespace AdventOfCode.Tools;

internal static class AsciiLetterDecoder
{
    public static string DecodeAsciiMessage(this string asciiMessage)
        => asciiMessage.SplitLines().Length switch
        {
            6  => Decoder(asciiMessage, 5, SmallLettersMap()),
            10 => Decoder(asciiMessage, 8, LargeLettersMap()),
            _ => string.Empty
        };

    private static string Decoder(string asciiMessage, int colWidth, Dictionary<string, char> letterMap)
        => new(asciiMessage.SplitIntoColumns(colWidth)
                           .Select(l => letterMap.TryGetValue(l, out var c) ? c : '?').ToArray());

    private static Dictionary<string, char> SmallLettersMap() => """
          ##  ###   ##  ###  #### ####  ##  #  #  ###   ## #  # #     ##  ###  ###   ### #  # #   #####
         #  # #  # #  # #  # #    #    #  # #  #   #     # # #  #    #  # #  # #  # #    #  # #   #   #
         #  # ###  #    #  # ###  ###  #    ####   #     # ##   #    #  # #  # #  # #    #  #  # #   #
         #### #  # #    #  # #    #    # ## #  #   #     # # #  #    #  # ###  ###   ##  #  #   #   #
         #  # #  # #  # #  # #    #    #  # #  #   #  #  # # #  #    #  # #    # #     # #  #   #  #
         #  # ###   ##  ###  #### #     ### #  #  ###  ##  #  # ####  ##  #    #  # ###   ##    #  ####
         """.SplitIntoColumns(5)
            .Zip("ABCDEFGHIJKLOPQRSUYZ")
            .ToDictionary();

    private static Dictionary<string, char> LargeLettersMap() => """
           ##    #####    ####   ######  ######   ####   #    #     ###  #    #  #       #    #  #####   #####   #    #  ######
          #  #   #    #  #    #  #       #       #    #  #    #      #   #   #   #       ##   #  #    #  #    #  #    #       #
         #    #  #    #  #       #       #       #       #    #      #   #  #    #       ##   #  #    #  #    #   #  #        #
         #    #  #    #  #       #       #       #       #    #      #   # #     #       # #  #  #    #  #    #   #  #       #
         #    #  #####   #       #####   #####   #       ######      #   ##      #       # #  #  #####   #####     ##       #
         ######  #    #  #       #       #       #  ###  #    #      #   ##      #       #  # #  #       #  #      ##      #
         #    #  #    #  #       #       #       #    #  #    #      #   # #     #       #  # #  #       #   #    #  #    #
         #    #  #    #  #       #       #       #    #  #    #  #   #   #  #    #       #   ##  #       #   #    #  #   #
         #    #  #    #  #    #  #       #       #   ##  #    #  #   #   #   #   #       #   ##  #       #    #  #    #  #
         #    #  #####    ####   ######  #        ### #  #    #   ###    #    #  ######  #    #  #       #    #  #    #  ######
         """.SplitIntoColumns(8)
            .Zip("ABCEFGHJKLNPRXZ")
            .ToDictionary();

}
