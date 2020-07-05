/*

-- On the Subject of Forget Us Not --
- This one likes attention, but not *too* much attention. -

Complete a different module to unlock each stage.
Each stage will provide a different letter.
Using the rules below, change each letter to the appropriate number.
Once all letters have been displayed, the display will go blank. Enter the correct numbers.

*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using KModkit;



public class AdvancedMemory : MonoBehaviour
{
    private const int ADDED_STAGES = 0;
    private const bool PERFORM_AUTO_SOLVE = false;
    private const float STAGE_DELAY = 1.5f;

    public ToneGenerator Tone;
    public static string[] ignoredModules = null;

    public static int loggingID = 1;
    public int thisLoggingID;

    public KMBombInfo BombInfo;
    public KMAudio Sound;

    public KMSelectable Button0, Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8, Button9;
    private KMSelectable[] Buttons;
    public TextMesh DisplayMesh, DisplayMeshBig;

    private bool outputS;
    private string[] oldSolved;
    private int[] Display;
    private int[] Solution;
    private int Position;
    private int currentSolves = 0;
    private int[] stages;
    private int sNums, batts;
    private int[] solvedOrder;

    private bool forcedSolve = false;


    //Codes stored in a switch case in the form 'case(module name):r = code;break;'
    private int getCode(string s)
    {
        int r;
        switch (s)
        {
            case ("Wires"): r = 000; break;
            case ("The Button"): r = 100; break;
            case ("The Button Translated"): r = 100; break;
            case ("Big Button Translated"): r = 100; break;
            case ("Keypad"): r = 200; break;
            case ("Maze"): r = 300; break;
            case ("Memory"): r = 400; break;
            case ("Morse Code"): r = 500; break;
            case ("Morse Code Translated"): r = 500; break;
            case ("Password"): r = 600; break;
            case ("Passwords"): r = 600; break;
            case ("Password Translated"): r = 600; break;
            case ("Passwords Translated"): r = 600; break;
            case ("Simon Says"): r = 800; break;
            case ("Wire Sequence"): r = 001; break;
            case ("Who’s on First"): r = 123; break;
            case ("Who's on First"): r = 123; break;
            case ("Who’s on First Translated"): r = 123; break;
            case ("Who's on First Translated"): r = 123; break;
            case ("Complicated Wires"): r = 002; break;
            case ("Colour Flash"): r = 380; break;
            case ("Piano Keys"): r = 140; break;
            case ("Semaphore"): r = 604; break;
            case ("Emoji Math"): r = 700; break;
            case ("Switches"): r = 058; break;
            case ("Two Bits"): r = 578; break;
            case ("Word Scramble"): r = 150; break;
            case ("Anagrams"): r = 151; break;
            case ("Combination Lock"): r = 286; break;
            case ("Square Button"): r = 102; break;
            case ("Simon States"): r = 801; break;
            case ("Round Keypad"): r = 201; break;
            case ("Listening"): r = 789; break;
            case ("Foreign Exchange Rates"): r = 202; break;
            case ("Orientation Cube"): r = 610; break;
            case ("Morsematics"): r = 501; break;
            case ("Connection Check"): r = 240; break;
            case ("Letter Keys"): r = 203; break;
            case ("Astrology"): r = 770; break;
            case ("Logic"): r = 110; break;
            case ("Crazy Talk"): r = 496; break;
            case ("Adventure Game"): r = 622; break;
            case ("Turn The Keys"): r = 901; break;
            case ("Mystic Square"): r = 682; break;
            case ("Plumbing"): r = 301; break;
            case ("Cruel Piano Keys"): r = 141; break;
            case ("Safety Safe"): r = 601; break;
            case ("Cryptography"): r = 195; break;
            case ("Chess"): r = 630; break;
            case ("Mouse In The Maze"): r = 302; break;
            case ("3D Maze"): r = 303; break;
            case ("Silly Slots"): r = 777; break;
            case ("Number Pad"): r = 204; break;
            case ("Probing"): r = 850; break;
            case ("Resistors"): r = 851; break;
            case ("Simon Shrieks"): r = 805; break;
            case ("Skewed Slots"): r = 074; break;
            case ("Complex Keypad"): r = 206; break;
            case ("Subways"): r = 763; break;
            case ("Lasers"): r = 861; break;
            case ("Turtle Robot"): r = 855; break;
            case ("Guitar Chords"): r = 145; break;
            case ("Calendar"): r = 931; break;
            case ("USA Maze"): r = 307; break;
            case ("Binary Tree"): r = 013; break;
            case ("Lightspeed"): r = 664; break;
            case ("Black Hole"): r = 773; break;
            case ("Simon’s Star"): r = 774; break;
            case ("Simon's Star"): r = 774; break;
            case ("Morse War"): r = 504; break;
            case ("The Stock Market"): r = 556; break;
            case ("Mineseeker"): r = 372; break;
            case ("Maze Scrambler"): r = 308; break;
            case ("The Number Cipher"): r = 986; break;
            case ("Alphabet Numbers"): r = 982; break;
            case ("British Slang"): r = 764; break;
            case ("Double Color"): r = 383; break;
            case ("Maritime Flags"): r = 605; break;
            case ("Equations"): r = 706; break;
            case ("Pattern Cube"): r = 616; break;
            case ("Know Your Way"): r = 321; break;
            case ("Splitting The Loot"): r = 721; break;
            case ("Simon Samples"): r = 807; break;
            case ("Character Shift"): r = 160; break;
            case ("Uncolored Squares"): r = 831; break;
            case ("Dragon Energy"): r = 970; break;
            case ("Flashing Lights"): r = 373; break;
            case ("3D Tunnels"): r = 309; break;
            case ("Synchronization"): r = 920; break;
            case ("The Switch"): r = 057; break;
            case ("Reverse Morse"): r = 505; break;
            case ("Manometers"): r = 872; break;
            case ("Shikaku"): r = 687; break;
            case ("Wire Spaghetti"): r = 006; break;
            case ("Tennis"): r = 625; break;
            case ("Module Homework"): r = 732; break;
            case ("Benedict Cumberbatch"): r = 971; break;
            case ("Signals"): r = 862; break;
            case ("Horrible Memory"): r = 401; break;
            case ("Boggle"): r = 636; break;
            case ("Boolean Maze"): r = 014; break;
            case ("Sonic & Knuckles"): r = 648; break;
            case ("Quintuples"): r = 981; break;
            case ("Red Arrows"): r = 050; break;
            case ("Yellow Arrows"): r = 052; break;
            case ("Blue Arrows"): r = 053; break;
            case ("Sticky Notes"): r = 558; break;
            case ("Unordered Keys"): r = 962; break;
            case ("Orange Arrows"): r = 054; break;
            case ("Hyperactive Numbers"): r = 989; break;
            case ("Reordered Keys"): r = 963; break;
            case ("Button Grid"): r = 118; break;
            case ("Find The Date"): r = 932; break;
            case ("Misordered Keys"): r = 964; break;
            case ("The Matrix"): r = 675; break;
            case ("Purple Arrows"): r = 055; break;
            case ("Bordered Keys"): r = 965; break;
            case ("The Dealmaker"): r = 526; break;
            case ("Seven Deadly Sins"): r = 666; break;
            case ("The Ultracube"): r = 826; break;
            case ("Symbolic Colouring"): r = 593; break;
            case ("Recorded Keys"): r = 966; break;
            case ("The Deck of Many Things"): r = 546; break;
            case ("Disordered Keys"): r = 967; break;
            case ("Character Codes"): r = 162; break;
            case ("Raiding Temples"): r = 527; break;
            case ("Bomb Diffusal"): r = 430; break;
            case ("Double Expert"): r = 431; break;
            case ("Calculus"): r = 715; break;
            case ("Boolean Keypad"): r = 015; break;
            case ("Toon Enough"): r = 658; break;
            case ("Pictionary"): r = 453; break;
            case ("Qwirkle"): r = 639; break;
            case ("Antichamber"): r = 659; break;
            case ("Simon Simons"): r = 814; break;
            case ("Lucky Dice"): r = 865; break;
            case ("Constellations"): r = 776; break;
            case ("Prime Checker"): r = 723; break;
            case ("Cruel Digital Root"): r = 464; break;
            case ("Faulty Digital Root"): r = 474; break;
            case ("Boot Too Big"): r = 782; break;
            case ("Book of Mario"): r = 422; break;
            case ("Vigenère Cipher"): r = 159; break;
            case ("Langton’s Ant"): r = 340; break;
            case ("Langton's Ant"): r = 340; break;
            case ("Old Fogey"): r = 341; break;
            case ("Insanagrams"): r = 163; break;
            case ("Snakes and Ladders"): r = 691; break;
            case ("Module Movements"): r = 292; break;
            case ("Bamboozled Again"): r = 129; break;
            case ("The Hangover"): r = 672; break;
            case ("Binary Puzzle"): r = 690; break;
            case ("Coffeebucks"): r = 524; break;
            case ("Colorful Madness"): r = 384; break;
            case ("Alliances"): r = 072; break;
            case ("Dungeon"): r = 080; break;
            case ("Unicode"): r = 869; break;
            case ("Password Generator"): r = 608; break;
            case ("Baccarat"): r = 082; break;
            case ("Guess Who?"): r = 677; break;
            case ("Reverse Alphabetize"): r = 165; break;
            case ("Alphabetize"): r = 164; break;
            case ("Gatekeeper"): r = 073; break;
            case ("Light Bulbs"): r = 490; break;
            case ("1000 Words"): r = 230; break;
            case ("Five Letter Words"): r = 231; break;
            case ("Settlers of KTaNE"): r = 083; break;
            case ("The Hidden Value"): r = 996; break;
            case ("Directional Button"): r = 126; break;
            case ("…?"): r = 754; break;
            case ("...?"): r = 754; break;
            case ("The Simpleton"): r = 098; break;
            case ("Misery Squares"): r = 911; break;
            case ("Not Wiresword"): r = 030; break;
            case ("Not Wire Sequence"): r = 031; break;
            case ("Not Who’s on First"): r = 032; break;
            case ("Not Who's on First"): r = 032; break;
            case ("Not Simaze"): r = 033; break;
            case ("Not Password"): r = 034; break;
            case ("Not Morse Code"): r = 035; break;
            case ("Not Memory"): r = 036; break;
            case ("Not Maze"): r = 037; break;
            case ("Not Keypad"): r = 038; break;
            case ("Not Complicated Wires"): r = 039; break;
            case ("Not the Button"): r = 040; break;
            case ("Sequences"): r = 021; break;
            case ("Dungeon 2nd Floor"): r = 081; break;
            case ("Wire Ordering"): r = 020; break;
            case ("Vcrcs"): r = 084; break;
            case ("Quaternions"): r = 619; break;
            case ("Abstract Sequences"): r = 022; break;
            case ("osu!"): r = 676; break;
            case ("Shifting Maze"): r = 328; break;
            case ("Sorting"): r = 075; break;
            case ("Role Reversal"): r = 076; break;
            case ("Placeholder Talk"): r = 492; break;
            case ("Art Appreciation"): r = 071; break;
            case ("Answer to…"): r = 679; break;
            case ("Shell Game"): r = 086; break;
            case ("Pattern Lock"): r = 617; break;
            case ("Quick Arithmetic"): r = 727; break;
            case ("Minecraft Cipher"): r = 181; break;
            case ("Cheat Checkout"): r = 571; break;
            case ("The Samsung"): r = 875; break;
            case ("Etterna"): r = 196; break;
            case ("Caesar Cipher"): r = 152; break;
            case ("Perspective Pegs"): r = 611; break;
            case ("Microcontroller"): r = 852; break;
            case ("Murder"): r = 633; break;
            case ("The Gamepad"): r = 640; break;
            case ("Tic Tac Toe"): r = 620; break;
            case ("Monsplode, Fight!"): r = 641; break;
            case ("Shape Shift"): r = 840; break;
            case ("Follow The Leader"): r = 432; break;
            case ("Follow the Leader"): r = 432; break;
            case ("Friendship"): r = 660; break;
            case ("The Bulb"): r = 491; break;
            case ("Alphabet"): r = 205; break;
            case ("Blind Alley"): r = 750; break;
            case ("Sea Shells"): r = 124; break;
            case ("English Test"): r = 730; break;
            case ("Rock-Paper-Scissors-L.-Sp."): r = 621; break;
            case ("Hexamaze"): r = 304; break;
            case ("Bitmaps"): r = 748; break;
            case ("Colored Squares"): r = 830; break;
            case ("Adjacent Letters"): r = 095; break;
            case ("Third Base"): r = 125; break;
            case ("Word Search"): r = 680; break;
            case ("Broken Buttons"): r = 103; break;
            case ("Simon Screams"): r = 802; break;
            case ("Laundry"): r = 550; break;
            case ("Modules Against Humanity"): r = 631; break;
            case ("Complicated Buttons"): r = 104; break;
            case ("Battleship"): r = 632; break;
            case ("Text Field"): r = 241; break;
            case ("Symbolic Password"): r = 590; break;
            case ("Wire Placement"): r = 003; break;
            case ("Double-Oh"): r = 661; break;
            case ("Cheap Checkout"): r = 551; break;
            case ("Coordinates"): r = 720; break;
            case ("Light Cycle"): r = 960; break;
            case ("Rhythms"): r = 142; break;
            case ("Color Math"): r = 701; break;
            case ("Only Connect"): r = 662; break;
            case ("Creation"): r = 642; break;
            case ("Rubik’s Cube"): r = 681; break;
            case ("Rubik's Cube"): r = 681; break;
            case ("FizzBuzz"): r = 983; break;
            case ("Neutralization"): r = 740; break;
            case ("Web Design"): r = 851; break;
            case ("The Clock"): r = 940; break;
            case ("LED Encryption"): r = 370; break;
            case ("Bitwise Operations"): r = 010; break;
            case ("Fast Math"): r = 702; break;
            case ("Lion’s Share"): r = 665; break;
            case ("Lion's Share"): r = 665; break;
            case ("Snooker"): r = 626; break;
            case ("Blackjack"): r = 543; break;
            case ("Party Time"): r = 649; break;
            case ("Accumulation"): r = 710; break;
            case ("The Plunger Button"): r = 106; break;
            case ("The Digit"): r = 980; break;
            case ("The Jack-O’-Lantern"): r = 990; break;
            case ("The Jack-O'-Lantern"): r = 990; break;
            case ("T-Words"): r = 161; break;
            case ("Divided Squares"): r = 910; break;
            case ("Connection Device"): r = 856; break;
            case ("Instructions"): r = 733; break;
            case ("Valves"): r = 863; break;
            case ("Encrypted Morse"): r = 506; break;
            case ("The Crystal Maze"): r = 667; break;
            case ("Cruel Countdown"): r = 480; break;
            case ("Countdown"): r = 668; break;
            case ("Catchphrase"): r = 669; break;
            case ("Blockbusters"): r = 670; break;
            case ("IKEA"): r = 972; break;
            case ("Retirement"): r = 525; break;
            case ("Periodic Table"): r = 741; break;
            case ("101 Dalmatians"): r = 101; break;
            case ("Schlag den Bomb"): r = 671; break;
            case ("Mahjong"): r = 637; break;
            case ("Kudosudoku"): r = 688; break;
            case ("The Radio"): r = 882; break;
            case ("Modulo"): r = 711; break;
            case ("Number Nimbleness"): r = 712; break;
            case ("Challenge & Contact"): r = 627; break;
            case ("The Triangle"): r = 823; break;
            case ("Sueet Wall"): r = 544; break;
            case ("Christmas Presents"): r = 991; break;
            case ("Hieroglyphics"): r = 973; break;
            case ("Functions"): r = 707; break;
            case ("Scripting"): r = 857; break;
            case ("Simon Spins"): r = 807; break;
            case ("Ten-Button Color Code"): r = 385; break;
            case ("Cursed Double-Oh"): r = 481; break;
            case ("Crackbox"): r = 689; break;
            case ("Street Fighter"): r = 650; break;
            case ("The Labyrinth"): r = 310; break;
            case ("Spinning Buttons"): r = 107; break;
            case ("The Festive Jukebox"): r = 883; break;
            case ("Skinny Wires"): r = 007; break;
            case ("Factory Maze"): r = 311; break;
            case ("Greek Calculus"): r = 705; break;
            case ("Bases"): r = 722; break;
            case ("Safety Square"): r = 743; break;
            case ("Treasure Hunt"): r = 331; break;
            case ("Broken Guitar Chords"): r = 146; break;
            case ("Colo(u)r Talk"): r = 493; break;
            case ("The Fortnight"): r = 423; break;
            case ("Double Arrows"): r = 060; break;
            case ("Boolean Wires"): r = 016; break;
            case ("Partial Derivatives"): r = 716; break;
            case ("Vectors"): r = 724; break;
            case ("Yoinking E-Bucks"): r = 424; break;
            case ("Caesar Cycle"): r = 250; break;
            case ("Affine Cycle"): r = 251; break;
            case ("Pigpen Cycle"): r = 252; break;
            case ("Flower Patch"): r = 280; break;
            case ("Playfair Cycle"): r = 253; break;
            case ("Jumble Cycle"): r = 254; break;
            case ("Alpha-Bits"): r = 166; break;
            case ("Jack Attack"): r = 692; break;
            case ("Ultimate Cycle"): r = 255; break;
            case ("Need. Complicated Button"): r = 117; break;
            case ("Hill Cycle"): r = 256; break;
            case ("Binary"): r = 017; break;
            case ("Chord Progressions"): r = 149; break;
            case ("Matchematics"): r = 725; break;
            case ("Bob Barks"): r = 791; break;
            case ("Simon’s On First"): r = 122; break;
            case ("Simon's On First"): r = 122; break;
            case ("Weird Al Yankovic"): r = 881; break;
            case ("Forget Me Now"): r = 900; break;
            case ("Simon Selects"): r = 811; break;
            case ("Cryptic Cycle"): r = 257; break;
            case ("Bone Apple Tea"): r = 425; break;
            case ("Robot Programming"): r = 858; break;
            case ("Masyu"): r = 950; break;
            case ("Hold Ups"): r = 342; break;
            case ("Red Cipher"): r = 170; break;
            case ("Flash Memory"): r = 403; break;
            case ("A-maze-ing Buttons"): r = 315; break;
            case ("Orange Cipher"): r = 171; break;
            case ("Unown Cipher"): r = 693; break;
            case ("TetraVex"): r = 842; break;
            case ("Meter"): r = 560; break;
            case ("The Modkit"): r = 827; break;
            case ("Red Buttons"): r = 116; break;
            case ("The Rule"): r = 828; break;
            case ("Bamboozling Button Grid"): r = 128; break;
            case ("Footnotes"): r = 272; break;
            case ("Lousy Chess"): r = 614; break;
            case ("Recolored Switches"): r = 068; break;
            case ("Cruel Garfield Kart"): r = 288; break;
            case ("1D Maze"): r = 327; break;
            case ("Reverse Polish Notation"): r = 728; break;
            case ("Snowflakes"): r = 951; break;
            case ("Funny Numbers"): r = 997; break;
            case ("Label Priorities"): r = 921; break;
            case ("Numbered Buttons"): r = 097; break;
            case ("Exoplanets"): r = 765; break;
            case ("Simon Stages"): r = 815; break;
            case ("Stock Images"): r = 572; break;
            case ("Forget Infinity"): r = 903; break;
            case ("Roger"): r = 562; break;
            case ("Malfunctions"): r = 697; break;
            case ("Minecraft Parody"): r = 333; break;
            case ("Strike/Solve"): r = 415; break;
            case ("Minecraft Survival"): r = 343; break;
            case ("NumberWang"): r = 232; break;
            case ("Fencing"): r = 744; break;
            case ("Uncolored Switches"): r = 067; break;
            case ("Name Changes"): r = 609; break;
            case ("Just Numbers"): r = 978; break;
            case ("Chord Qualities"): r = 143; break;
            case ("Minesweeper"): r = 683; break;
            case ("Zoo"): r = 552; break;
            case ("Binary LEDs"): r = 011; break;
            case ("Boolean Venn Diagram"): r = 012; break;
            case ("Point of Order"): r = 540; break;
            case ("Ice Cream"): r = 580; break;
            case ("The Screw"): r = 442; break;
            case ("Yahtzee"): r = 634; break;
            case ("X-Ray"): r = 520; break;
            case ("Color Morse"): r = 502; break;
            case ("Mastermind Simple"): r = 460; break;
            case ("Mastermind Cruel"): r = 470; break;
            case ("Gridlock"): r = 320; break;
            case ("Big Circle"): r = 820; break;
            case ("Morse-A-Maze"): r = 503; break;
            case ("Colored Switches"): r = 059; break;
            case ("Perplexing Wires"): r = 004; break;
            case ("Monsplode Trading Cards"): r = 541; break;
            case ("Game of Life Simple"): r = 461; break;
            case ("Game of Life Cruel"): r = 471; break;
            case ("Nonogram"): r = 684; break;
            case ("S.E.T."): r = 635; break;
            case ("Painting"): r = 360; break;
            case ("Color Generator"): r = 381; break;
            case ("Symbol Cycle"): r = 591; break;
            case ("Hunting"): r = 510; break;
            case ("Extended Password"): r = 602; break;
            case ("Curriculum"): r = 731; break;
            case ("Braille"): r = 751; break;
            case ("Mafia"): r = 623; break;
            case ("Festive Piano Keys"): r = 144; break;
            case ("Flags"): r = 760; break;
            case ("Timezone"): r = 930; break;
            case ("Polyhedral Maze"): r = 305; break;
            case ("Symbolic Coordinates"): r = 592; break;
            case ("Poker"): r = 542; break;
            case ("Sonic the Hedgehog"): r = 643; break;
            case ("Poetry"): r = 644; break;
            case ("Button Sequence"): r = 105; break;
            case ("Algebra"): r = 703; break;
            case ("Visual Impairment"): r = 371; break;
            case ("The Jukebox"): r = 880; break;
            case ("Identity Parade"): r = 553; break;
            case ("Maintenance"): r = 521; break;
            case ("Blind Maze"): r = 306; break;
            case ("Backgrounds"): r = 462; break;
            case ("Mortal Kombat"): r = 645; break;
            case ("Regular Crazy Talk"): r = 497; break;
            case ("Hogwarts"): r = 673; break;
            case ("Dominoes"): r = 864; break;
            case ("Simon Speaks"): r = 808; break;
            case ("Discolored Squares"): r = 832; break;
            case ("Krazy Talk"): r = 498; break;
            case ("Numbers"): r = 987; break;
            case ("Varicolored Squares"): r = 833; break;
            case ("Free Parking"): r = 638; break;
            case ("Cookie Jars"): r = 583; break;
            case ("Alchemy"): r = 512; break;
            case ("Zoni"): r = 974; break;
            case ("Unrelated Anagrams"): r = 207; break;
            case ("Mad Memory"): r = 402; break;
            case ("Bartending"): r = 651; break;
            case ("Question Mark"): r = 652; break;
            case ("Shapes And Bombs"): r = 451; break;
            case ("Flavor Text"): r = 494; break;
            case ("Flavor Text EX"): r = 495; break;
            case ("Decolored Squares"): r = 834; break;
            case ("Homophones"): r = 781; break;
            case ("DetoNATO"): r = 975; break;
            case ("SYNC-125 [3]"): r = 976; break;
            case ("Westeros"): r = 674; break;
            case ("Pigpen Rotations"): r = 155; break;
            case ("LED Math"): r = 708; break;
            case ("Simon Sounds"): r = 809; break;
            case ("Simon Scrambles"): r = 810; break;
            case ("Harmony Sequence"): r = 147; break;
            case ("Unfair Cipher"): r = 156; break;
            case ("Melody Sequencer"): r = 148; break;
            case ("Colorful Insanity"): r = 386; break;
            case ("Passport Control"): r = 653; break;
            case ("Left and Right"): r = 322; break;
            case ("Gadgetron Vendor"): r = 654; break;
            case ("The Hexabutton"): r = 108; break;
            case ("Genetic Sequence"): r = 742; break;
            case ("Micro-Modules"): r = 992; break;
            case ("Module Maze"): r = 312; break;
            case ("Elder Futhark"): r = 977; break;
            case ("Tasha Squeals"): r = 790; break;
            case ("Digital Cipher"): r = 157; break;
            case ("Subscribe to Pewdiepie"): r = 421; break;
            case ("Grocery Store"): r = 557; break;
            case ("Simon Sends"): r = 804; break;
            case ("Fruits"): r = 244; break;
            case ("Module Listening"): r = 788; break;
            case ("Garfield Kart"): r = 287; break;
            case ("Yellow Cipher"): r = 172; break;
            case ("Kooky Keypad"): r = 209; break;
            case ("Green Cipher"): r = 173; break;
            case ("RGB Maze"): r = 316; break;
            case ("Blue Cipher"): r = 174; break;
            case ("The Legendre Symbol"): r = 829; break;
            case ("Keypad Lock"): r = 210; break;
            case ("Heraldry"): r = 290; break;
            case ("Faulty RGB Maze"): r = 317; break;
            case ("Indigo Cipher"): r = 175; break;
            case ("Violet Cipher"): r = 176; break;
            case ("Encryption Bingo"): r = 374; break;
            case ("Color Addition"): r = 389; break;
            case ("Chinese Counting"): r = 628; break;
            case ("Tower of Hanoi"): r = 283; break;
            case ("Keypad Combinations"): r = 294; break;
            case ("UltraStores"): r = 783; break;
            case ("Kanji"): r = 271; break;
            case ("Geometry Dash"): r = 274; break;
            case ("Ternary Converter"): r = 273; break;
            case ("N&Ms"): r = 247; break;
            case ("The Colored Maze"): r = 318; break;
            case ("White Cipher"): r = 177; break;
            case ("Gray Cipher"): r = 178; break;
            case ("The Hyperlink"): r = 890; break;
            case ("Black Cipher"): r = 179; break;
            case ("Loopover"): r = 248; break;
            case ("Divisible Numbers"): r = 997; break;
            case ("Corners"): r = 282; break;
            case ("The High Score"): r = 891; break;
            case ("Ingredients"): r = 245; break;
            case ("Jenga"): r = 291; break;
            case ("Intervals"): r = 293; break;
            case ("Cruel Boolean Maze"): r = 319; break;
            case ("Cheep Checkout"): r = 561; break;
            case ("Spelling Bee"): r = 615; break;
            case ("Memorable Buttons"): r = 127; break;
            case ("Thinking Wires"): r = 018; break;
            case ("Seven Choose Four"): r = 284; break;
            case ("Object Shows"): r = 281; break;
            case ("Lunchtime"): r = 942; break;
            case ("Natures"): r = 246; break;
            case ("Neutrinos"): r = 285; break;
            case ("Musical Transposition"): r = 130; break;
            case ("Scavenger Hunt"): r = 332; break;
            case ("Polygons"): r = 755; break;
            case ("Ultimate Cipher"): r = 169; break;
            case ("Mashematics"): r = 704; break;
            case ("Faulty Backgrounds"): r = 472; break;
            case ("Radiator"): r = 870; break;
            case ("Modern Cipher"): r = 153; break;
            case ("LED Grid"): r = 242; break;
            case ("Sink"): r = 463; break;
            case ("The iPhone"): r = 871; break;
            case ("The Swan"): r = 663; break;
            case ("Waste Management"): r = 522; break;
            case ("Human Resources"): r = 554; break;
            case ("Skyrim"): r = 646; break;
            case ("Burglar Alarm"): r = 874; break;
            case ("Press X"): r = 647; break;
            case ("European Travel"): r = 761; break;
            case ("Error Codes"): r = 404; break;
            case ("LEGOs"): r = 860; break;
            case ("Rubik’s Clock"): r = 685; break;
            case ("Rubik's Clock"): r = 685; break;
            case ("Font Select"): r = 841; break;
            case ("The Stopwatch"): r = 941; break;
            case ("Pie"): r = 581; break;
            case ("The Wire"): r = 005; break;
            case ("The London Underground"): r = 762; break;
            case ("Logic Gates"): r = 111; break;
            case ("Grid Matching"): r = 330; break;
            case ("Color Decoding"): r = 382; break;
            case ("The Sun"): r = 771; break;
            case ("Playfair Cipher"): r = 154; break;
            case ("Tangrams"): r = 854; break;
            case ("The Number"): r = 284; break;
            case ("Cooking"): r = 582; break;
            case ("Superlogic"): r = 112; break;
            case ("The Moon"): r = 772; break;
            case ("The Cube"): r = 821; break;
            case ("Dr. Doctor"): r = 523; break;
            case ("Tax Returns"): r = 555; break;
            case ("The Jewel Vault"): r = 511; break;
            case ("Digital Root"): r = 999; break;
            case ("Graffiti Numbers"): r = 361; break;
            case ("Marble Tumble"): r = 686; break;
            case ("X01"): r = 624; break;
            case ("Logical Buttons"): r = 113; break;
            case ("The Code"): r = 985; break;
            case ("Tap Code"): r = 752; break;
            case ("Simon Sings"): r = 803; break;
            case ("Burger Alarm"): r = 584; break;
            case ("Synonyms"): r = 780; break;
            case ("Mega Man 2"): r = 655; break;
            case ("Lombax Cubes"): r = 612; break;
            case ("The Stare"): r = 993; break;
            case ("Graphic Memory"): r = 362; break;
            case ("Quiz Buzz"): r = 943; break;
            case ("Wavetapping"): r = 452; break;
            case ("The Hypercube"): r = 842; break;
            case ("Stack’em"): r = 158; break;
            case ("Stack'em"): r = 158; break;
            case ("Seven Wires"): r = 008; break;
            case ("Colored Keys"): r = 208; break;
            case ("Planets"): r = 775; break;
            case ("The Necronomicon"): r = 994; break;
            case ("Four-Card Monte"): r = 545; break;
            case ("The Witness"): r = 656; break;
            case ("The Giant’s Drink"): r = 613; break;
            case ("The Giant's Drink"): r = 613; break;
            case ("Digit String"): r = 988; break;
            case ("Hidden Colors"): r = 387; break;
            case ("Colour Code"): r = 388; break;
            case ("Vexillology"): r = 363; break;
            case ("Brush Strokes"): r = 364; break;
            case ("Odd One Out"): r = 243; break;
            case ("The Triangle Button"): r = 109; break;
            case ("Mazematics"): r = 313; break;
            case ("Equations X"): r = 709; break;
            case ("Maze³"): r = 314; break;
            case ("Gryphons"): r = 270; break;
            case ("Arithmelogic"): r = 114; break;
            case ("Roman Art"): r = 365; break;
            case ("Faulty Sink"): r = 473; break;
            case ("Simon Stops"): r = 812; break;
            case ("Morse Buttons"): r = 507; break;
            case ("Baba Is Who?"): r = 657; break;
            case ("Simon Stores"): r = 813; break;
            case ("Risky Wires"): r = 009; break;
            case ("Modulus Manipulation"): r = 713; break;
            case ("Daylight Directions"): r = 873; break;
            case ("Cryptic Password"): r = 603; break;
            case ("Stained Glass"): r = 366; break;
            case ("The Block"): r = 825; break;
            case ("Bamboozling Button"): r = 119; break;
            case ("Insane Talk"): r = 499; break;
            case ("Transmitted Morse"): r = 508; break;
            case ("A Mistake"): r = 753; break;
            case ("Encrypted Equations"): r = 714; break;
            case ("Ordered Keys"): r = 961; break;
            case ("The Sphere"): r = 822; break;
            case ("Green Arrows"): r = 051; break;
            case ("Codenames"): r = 606; break;
            case ("Blinkstop"): r = 190; break;
            case ("Hinges"): r = 756; break;
            case ("❖"): r = 410; break;
            case ("Answering Can Be Fun"): r = 629; break;
            case ("15 Mystic Lights"): r = 411; break;
            case ("Rainbow Arrows"): r = 056; break;
            case ("Time Signatures"): r = 131; break;
            case ("Multi-Colored Switches"): r = 069; break;
            case ("Digital Dials"): r = 167; break;
            case ("Passcodes"): r = 607; break;
            case ("Hereditary Base Notation"): r = 718; break;
            case ("Lines of Code"): r = 859; break;
            case ("The cRule"): r = 838; break;
            case ("Prime Encryption"): r = 734; break;
            case ("Encrypted Dice"): r = 866; break;
            case ("Colorful Dials"): r = 390; break;
            case ("Following Orders"): r = 513; break;
            case ("Atbash Cipher"): r = 180; break;
            case ("Addition"): r = 719; break;
            case ("Binary Grid"): r = 039; break;
            case ("Matrices"): r = 726; break;
            case ("Cruel Keypads"): r = 211; break;
            case ("The Black Page"): r = 839; break;
            case ("64"): r = 412; break;
            case ("% Grey"): r = 413; break;
            case ("Greek Letter Grid"): r = 694; break;
            case ("Going Backwards"): r = 323; break;
            case ("Scalar Dials"): r = 132; break;
            case ("The World’s Largest Button"): r = 099; break;
            case ("The World's Largest Button"): r = 099; break;
            case ("Keywords"): r = 212; break;
            case ("State of Aggregation"): r = 192; break;
            case ("Dreamcipher"): r = 189; break;
            case ("Echolocation"): r = 778; break;
            case ("Boxing"): r = 618; break;
            case ("Topsy Turvy"): r = 193; break;
            case ("Railway Cargo Loading"): r = 440; break;
            case ("Conditional Buttons"): r = 115; break;
            case ("ASCII Art"): r = 070; break;
            case ("Semamorse"): r = 509; break;
            case ("Symbolic Tasha"): r = 594; break;
            case ("Alphabetical Ruling"): r = 194; break;
            case ("Microphone"): r = 779; break;
            case ("Widdershins"): r = 450; break;
            case ("The Cruel Code"): r = 995; break;
            case ("Lockpick Maze"): r = 329; break;
            case ("Dimension Disruption"): r = 441; break;
            case ("V"): r = 414; break;
            case ("A Message"): r = 570; break;
            case ("Lying Indicators"): r = 563; break;
            case ("Training Text"): r = 514; break;
            case ("Wonder Cipher"): r = 168; break;
            case ("Caesar’s Math’s"): r = 122; break;
            case ("Caesar's Math's"): r = 122; break;
            case ("Random Access Memory"): r = 573; break;
            case ("Triamonds"): r = 843; break;
            case ("Stars"): r = 766; break;
            case ("Button Order"): r = 096; break;
            case ("Jukebox.WAV"): r = 884; break;
            case ("Elder Password"): r = 599; break;
            case ("Switching Maze"): r = 326; break;
            case ("Mystery Module"): r = 902; break;
            case ("Ladder Lottery"): r = 234; break;
            case ("Co-op Harmony Sequence"): r = 133; break;
            case ("Standard Crazy Talk"): r = 220; break;
            case ("Quote Crazy Talk End Quote"): r = 221; break;
            case ("Kilo Talk"): r = 222; break;
            case ("Kay-Mazey Talk"): r = 223; break;
            case ("Jaden Smith Talk"): r = 224; break;
            case ("Deck Creating"): r = 547; break;
            case ("Crazy Talk With A K"): r = 225; break;
            case ("BoozleTalk"): r = 226; break;
            case ("Arrow Talk"): r = 227; break;
            case ("Siffron"): r = 233; break;
            case ("Red Herring"): r = 420; break;
            case ("Audio Morse"): r = 519; break;
            case ("Palindromes"): r = 979; break;
            case ("Pow"): r = 260; break;
            case ("Type Racer"): r = 792; break;
            case ("Chicken Nuggets"): r = 585; break;
            case ("Badugi"): r = 548; break;
            case ("Tetriamonds"): r = 844; break;
            case ("Spot The Difference"): r = 422; break;
            case ("Negativity"): r = 235; break;
            case ("Masher The Bottun"): r = 423; break;
            case ("Yes and No"): r = 236; break;
            case ("M&Ns"): r = 237; break;
            case ("Plant Identification"): r = 530; break;
            case ("Integer Trees"): r = 717; break;
            case ("Goofy’s Game"): r = 424; break;
            case ("Module Rick"): r = 426; break;
            case ("Pickup Identification"): r = 531; break;
            case ("Earthbound"): r = 120; break;
            case ("3 LEDs"): r = 416; break;
            case ("Life Iteration"): r = 121; break;
            case ("Thread The Needle"): r = 077; break;
            case ("Encrypted Hangman"): r = 876; break;
            case ("Accelerando"): r = 087; break;
            case ("Color Braille"): r = 757; break;
            case ("Remote Math"): r = 729; break;
            case ("Reflex"): r = 933; break;
            case ("hexOS"): r = 868; break;
            case ("Simon Stashes"): r = 816; break;
            case ("Kyudoku"): r = 678; break;
            case ("Brawler Database"): r = 454; break;
            case ("Shortcuts"): r = 867; break;
            case ("More Code"): r = 518; break;
            case ("7"): r = 417; break;
            case ("Mental Math"): r = 699; break;
            case ("Dictation"): r = 793; break;
            case ("Bloxx"): r = 922; break;
            case ("Basic Morse"): r = 517; break;
            case ("The Arena"): r = 892; break;
            case ("IPA"): r = 952; break;
            case ("Emotiguy Identification"): r = 532; break;
            case ("Cookie Clicker"): r = 586; break;
            case ("Audio Keypad"): r = 213; break;
            case ("100 Levels of Defusal"): r = 418; break;
            case ("Jailbreak"): r = 923; break;
            case ("Dumb Waiters"): r = 238; break;
            case ("DACH Maze"): r = 325; break;
            case ("Match ‘em"): r = 427; break;
            case ("Match 'em"): r = 427; break;
            case ("Match ’em"): r = 427; break;
            case ("Navinums"): r = 924; break;
            case ("Gnomish Puzzle"): r = 433; break;
            case ("Bridges"): r = 443; break;
            case ("A>N<D"): r = 419; break;
            case ("Juxtacolored Squares"): r = 835; break;
            default: r = 998; break;
        }
        return r;
    }

    private void Start()
    {
        batts = BombInfo.GetBatteryCount();
        oldSolved = new string[0];
        GetComponent<KMBombModule>().OnActivate += ActivateModule;
        char[] serial = BombInfo.GetSerialNumber().ToArray();
        int count = 0;
        foreach (char i in serial)
        {
            if (System.Char.IsDigit(i))
            {
                count++;
            }
        }
        sNums = count;
    }

    void Awake()
    {
        outputS = false;
        
        if (ignoredModules == null)
            ignoredModules = GetComponent<KMBossModule>().GetIgnoredModules("Forget Us Not", new string[]{
                "Forget Us Not"
            });

        thisLoggingID = loggingID++;

        Buttons = new KMSelectable[]{Button0, Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8, Button9};
        
        transform.Find("Background").GetComponent<MeshRenderer>().material.color = new Color(1, 0.1f, 0.1f);

        MeshRenderer mr = transform.Find("Wiring").GetComponent<MeshRenderer>();
        mr.materials[0].color = new Color(0.1f, 0.1f, 0.1f);
        mr.materials[1].color = new Color(0.3f, 0.3f, 0.3f);
        mr.materials[2].color = new Color(0.1f, 0.4f, 0.8f);

        transform.Find("Main Display").Find("Edge").GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
        //transform.Find("Stage Display").Find("Edge").GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);

        Button0.OnInteract += Handle0;
        Button1.OnInteract += Handle1;
        Button2.OnInteract += Handle2;
        Button3.OnInteract += Handle3;
        Button4.OnInteract += Handle4;
        Button5.OnInteract += Handle5;
        Button6.OnInteract += Handle6;
        Button7.OnInteract += Handle7;
        Button8.OnInteract += Handle8;
        Button9.OnInteract += Handle9;

        Color c = new Color(.71f, .70f, .68f); //new Color(0.91f, 0.88f, 0.86f);
        Button0.GetComponent<MeshRenderer>().material.color = c;
        Button1.GetComponent<MeshRenderer>().material.color = c;
        Button2.GetComponent<MeshRenderer>().material.color = c;
        Button3.GetComponent<MeshRenderer>().material.color = c;
        Button4.GetComponent<MeshRenderer>().material.color = c;
        Button5.GetComponent<MeshRenderer>().material.color = c;
        Button6.GetComponent<MeshRenderer>().material.color = c;
        Button7.GetComponent<MeshRenderer>().material.color = c;
        Button8.GetComponent<MeshRenderer>().material.color = c;
        Button9.GetComponent<MeshRenderer>().material.color = c;

        
    }

    private void ActivateModule()
    {
        int count = BombInfo.GetSolvableModuleNames().Where(x => !ignoredModules.Contains(x)).Count() + ADDED_STAGES;
        Display = new int[count];
        Solution = new int[count];
        stages = new int[count];
        solvedOrder = new int[count];

        //if (count == 0) { GetComponent<KMBombModule>().HandlePass(); } //Causes error in test harness since it is called too early
        
            for (int i = 0; i < count; i++)
            {
                stages[i] = i;
            }
            for (int i = 0; i < count; i++)
            {
                int pos = Random.Range(0, count - 1);
                int temp = stages[pos];
                stages[pos] = stages[i];
                stages[i] = temp;
            }
            Display = stages.ToArray();

        Debug.Log("[Forget Us Not #"+thisLoggingID+"] Non-FUN modules: " + count);
        string displayText = "";
        string solutionText = "";
        for (int a = 0; a < count; a++)
        {
            displayText += (Display[a] + 1);
            if (a != count - 1)
            {
                displayText += ",";
            }
            solutionText += Solution[a];
        }
        Debug.Log("[Forget Us Not #"+thisLoggingID+"] Stage order: " + displayText);
        //Debug.Log("[Forget Us Not #"+thisLoggingID+"] Solution: " + solutionText);

        if(PERFORM_AUTO_SOLVE) {
            TwitchHandleForcedSolve();
        }
    }

    int ticker = 0;
    bool done = false;

    float displayTimer = 1;
    int displayCurStage = 0;
    void FixedUpdate()
    {
        if(forcedSolve) return;


        if(displayTimer > 0) displayTimer -= Time.fixedDeltaTime;

        ticker++;
        if (ticker == 5)
        {
            int count = BombInfo.GetSolvableModuleNames().Where(x => !ignoredModules.Contains(x)).Count() + ADDED_STAGES;
            if (count == 0) { GetComponent<KMBombModule>().HandlePass(); }
            ticker = 0;
            if (Display == null)
            {
                DisplayMesh.text = "";
                DisplayMeshBig.text = "";
            }
            else
            {
                int progress = BombInfo.GetSolvedModuleNames().Where(x => !ignoredModules.Contains(x)).Count() + ADDED_STAGES;
                if(progress > currentSolves) {
                    currentSolves++;
                    var newSolved = BombInfo.GetSolvedModuleNames().Where(x => !ignoredModules.Contains(x)).ToList();
                    var newSolvedCopy = BombInfo.GetSolvedModuleNames().Where(x => !ignoredModules.Contains(x)).ToArray();
                    foreach(var module in oldSolved)
                    {
                        newSolved.Remove(module);
                    }
                    oldSolved = newSolvedCopy;
                    int code = -1;
                    string newModuleName = "";
                    foreach (var module in newSolved)
                    {
                        code = getCode(module);
                        newModuleName = module;
                    }
                    int a, b, c;
                    a = code / 100;
                    b = (code % 100) / 10;
                    c = code % 10;
                    double result = -1;
                    solvedOrder[stages[progress - 1]] = code;
                    if (sNums == 2)
                    {
                        a = System.Math.Abs(a - batts);
                        result = (a + System.Math.Abs(b - c)) % 10;
                    }
                    if (sNums == 3)
                    {
                        b = System.Math.Abs(b - batts);
                        result = (b + System.Math.Abs(a - c)) % 10;
                    }
                    if (sNums == 4)
                    {
                        c = System.Math.Abs(c - batts);
                        result = (c + System.Math.Abs(a - b)) % 10;
                    }
                    Debug.Log("[Forget Us Not #" + thisLoggingID + "] Stage = " + (stages[progress - 1] + 1));
                    Debug.Log("[Forget Us Not #" + thisLoggingID + "] Previously Solved = " + newModuleName);
                    Debug.Log("[Forget Us Not #" + thisLoggingID + "] Code Used = " + (code / 100).ToString() + ((code % 100) / 10).ToString() + (code % 10).ToString());
                    Debug.Log("[Forget Us Not #" + thisLoggingID + "] Stage " + (stages[progress - 1] + 1) + " = " + result);
                    Solution[stages[progress-1]] = System.Convert.ToInt32(result);
                }
                if (progress >= Display.Length)
                {
                    if (outputS == false)
                    {
                        outputS = true;
                        string solText = "";
                        foreach(var digit in Solution)
                        {
                            solText += digit;
                        }
                        Debug.Log("[Forget Us Not #" + thisLoggingID + "] Solution = " + solText);
                    }
                    if (!done) {
                        UpdateDisplayMesh(-1);
                        done = true;
                    }
                }
                else {
                    int stage = stages[progress];
                    if(stage < 10) {
                        if (Display.Length < 10) DisplayMesh.text = "" + stage;
                        else DisplayMesh.text = "0" + System.Convert.ToString(stage+1);
                    }
                    else DisplayMesh.text = "" + (stage+1);

                    UpdateDisplayMesh(progress);
                }
            }
        }
    }

    private int litButton = -1; 
    private bool Handle(int val)
    {
        if (Solution == null || Position >= Solution.Length) return false;

        int progress = BombInfo.GetSolvedModuleNames().Where(x => !ignoredModules.Contains(x)).Count() + ADDED_STAGES;
        if (progress < Solution.Length && !forcedSolve) {
            Debug.Log("[Forget Us Not #"+thisLoggingID+"] Tried to enter a value before solving all other modules.");
            GetComponent<KMBombModule>().HandleStrike();
            return false;
        }
        else if (val == Solution[Position])
        {
            if (litButton != -1)
            {
                Buttons[litButton].transform.Find("LED").GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
                litButton = -1;
            }
            Position++;
            UpdateDisplayMesh(-1);
            if (Position == Solution.Length) {
                Debug.Log("[Forget Us Not #"+thisLoggingID+"] Module solved.");
                GetComponent<KMBombModule>().HandlePass();
            }
            Sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, gameObject.transform);
            //Tone.SetTone(500 + Position * 1200 / Solution.Length);
            return true;
        }
        else
        {
            Debug.Log("[Forget Us Not #"+thisLoggingID+"] Stage " + (Position+1) + ": Pressed " + val + " instead of " + Solution[Position]);
            GetComponent<KMBombModule>().HandleStrike();
            /*
            if (litButton == -1)
            {
                litButton = Solution[Position];
                Buttons[litButton].transform.Find("LED").GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0);
            }
            */
            DisplayMeshBig.text = (solvedOrder[Position] / 100).ToString() + ((solvedOrder[Position] % 100) / 10).ToString() + (solvedOrder[Position] % 10).ToString(); 
            return false;
        }
    }

    private void UpdateDisplayMesh(int solved)
    {
        if(solved == -1) {
            //New method: Scroll small display as needed.
            DisplayMeshBig.text = "";

            string text = "";

            int PositionModified = Position;
            int Offset = 0;
            while(PositionModified > 24) {
                PositionModified -= 12;
                Offset += 12;
            }

            for(int a = Offset; a < Mathf.Min(Offset + 24, Solution.Length); a++) {
                string val = "-";
                if (a < Position) val = "" + Solution[a];

                if(a > Offset) {
                    if (a % 3 == 0) {
                        if (a % 12 == 0) text += "\n";
                        else text += " ";
                    }
                }
                text += val;
            }

            //Old method: Use small for first 24, switch to XXX:YYY after.
            /*if(Position > 24) {
                DisplayMesh.text = "";
                string sum = ""+Solution.Length;
                string pos = ""+Position;
                while(pos.Length < sum.Length) pos = "0"+pos;
                DisplayMeshBig.text = pos + "/" + sum;
            }
            else {
                DisplayMeshBig.text = "";

                string text = "";

                for(int a = 0; a < Solution.Length; a++) {
                    string val = "-";
                    if (a < Position) val = "" + Solution[a];

                    if(a > 0) {
                        if (a % 3 == 0) {
                            if (a % 12 == 0) text += "\n";
                            else text += " ";
                        }
                    }
                    text += val;

                    if(a == 23) break;
                }

                DisplayMesh.text = text;
            }*/
            DisplayMesh.text = text;
        }
        else {
            DisplayMesh.text = "";
            DisplayMeshBig.text = "" + (Display[solved] + 1);
        }
    }

    private bool Handle0()
    {
        if (Solution == null || Position == Solution.Length) return false;
        Button0.AddInteractionPunch(0.2f);
        Handle(0);
        return false;
    }

    private bool Handle1()
    {
        if (Solution == null || Position == Solution.Length) return false;
        Button1.AddInteractionPunch(0.2f);
        Handle(1);
        return false;
    }

    private bool Handle2()
    {
        if (Solution == null || Position == Solution.Length) return false;
        Button2.AddInteractionPunch(0.2f);
        Handle(2);
        return false;
    }

    private bool Handle3()
    {
        if (Solution == null || Position == Solution.Length) return false;
        Button3.AddInteractionPunch(0.2f);
        Handle(3);
        return false;
    }

    private bool Handle4()
    {
        if (Solution == null || Position == Solution.Length) return false;
        Button4.AddInteractionPunch(0.2f);
        Handle(4);
        return false;
    }

    private bool Handle5()
    {
        if (Solution == null || Position == Solution.Length) return false;
        Button5.AddInteractionPunch(0.2f);
        Handle(5);
        return false;
    }

    private bool Handle6()
    {
        if (Solution == null || Position == Solution.Length) return false;
        Button6.AddInteractionPunch(0.2f);
        Handle(6);
        return false;
    }

    private bool Handle7()
    {
        if (Solution == null || Position == Solution.Length) return false;
        Button7.AddInteractionPunch(0.2f);
        Handle(7);
        return false;
    }

    private bool Handle8()
    {
        if (Solution == null || Position == Solution.Length) return false;
        Button8.AddInteractionPunch(0.2f);
        Handle(8);
        return false;
    }

    private bool Handle9()
    {
        if (Solution == null || Position == Solution.Length) return false;
        Button9.AddInteractionPunch(0.2f);
        Handle(9);
        return false;
    }

    private int GetDigit(char c)
    {
        switch(c)
        {
            case '0': return 0;
            case '1': return 1;
            case '2': return 2;
            case '3': return 3;
            case '4': return 4;
            case '5': return 5;
            case '6': return 6;
            case '7': return 7;
            case '8': return 8;
            case '9': return 9;
            default: return -1;
        }
    }

    //Twitch Plays support

    #pragma warning disable 0414
    string TwitchHelpMessage = "Enter the Forget Us Not sequence with \"!{0} press 531820...\". The sequence length depends on how many modules were on the bomb. You may use spaces and commas in the digit sequence.";
    #pragma warning restore 0414

    public void TwitchHandleForcedSolve() {
        Debug.Log("[Forget Us Not #"+thisLoggingID+"] Module forcibly solved.");
        forcedSolve = true;
        StartCoroutine(Solver());
    }

    private IEnumerator Solver() {
        while(Position < Solution.Length) {
            yield return new WaitForSeconds(0.05f);
            Handle(Solution[Position]);
        }
    }

    public IEnumerator ProcessTwitchCommand(string cmd) {
        if(Solution == null || Position >= Solution.Length) yield break;
        cmd = cmd.ToLowerInvariant();

        int cut;
        if(cmd.StartsWith("submit ")) cut = 7;
        else if (cmd.StartsWith("press ")) cut = 6;
        else {
            yield return "sendtochaterror Use either 'submit' or 'press' followed by a number sequence.";
            yield break;
        }

        List<int> digits = new List<int>();
        char[] strSplit = cmd.Substring(cut).ToCharArray();
        foreach(char c in strSplit) {
            if(!"0123456789 ,".Contains(c)) {
                yield return "sendtochaterror Invalid character in number sequence: '" + c + "'.\nValid characters are 0-9, space, and comma.";
                yield break;
            }

            int d = GetDigit(c);
            if(d != -1) digits.Add(d);
        }
        if(digits.Count == 0) yield break;
        if(digits.Count > (Solution.Length - Position)) {
            yield return "sendtochaterror Too many digits submitted.";
            yield break;
        }

        int progress = BombInfo.GetSolvedModuleNames().Where(x => !ignoredModules.Contains(x)).Count();
        if(progress < Solution.Length) {
            yield return "Forget Us Not";
            yield return "sendtochat DansGame A little early, don't you think?";
            Handle(digits[0]);
            yield break;
        }
        yield return "Forget Us Not";
        yield return "sendtochat PogChamp Here we go!";
        yield return "multiple strikes"; //Needed for fake solve.

        SolveType solve = pickSolveType(digits.Count, Solution.Length - Position);

        foreach(int d in digits) {
            Button5.AddInteractionPunch(0.2f);
            bool valid = Handle(d);
            if(!valid) {
                if(solve == SolveType.REGULAR && BombInfo.GetTime() >= 45 && Random.value > 0.95) {
                    yield return new WaitForSeconds(2);
                    yield return "sendtochat Kreygasm We did it reddit!";
                    yield return new WaitForSeconds(1);
                    yield return "sendtochat Kappa Nope, just kidding.";
                }
                else yield return "sendtochat DansGame This isn't correct...";
                yield return "sendtochat Correct digits entered: " + Position;
                break;
            }
            if(Position >= Solution.Length) {
                yield return "sendtochat Kreygasm We did it reddit!";
                break;
            }

            if(getMusicToggle(solve, Position, digits.Count, Solution.Length - Position)) yield return "toggle waiting music";
            yield return new WaitForSeconds(getDelay(solve, Position, digits.Count, Solution.Length - Position));
        }
        yield return "end multiple strikes";
        yield break;
    }

    public enum SolveType {
        REGULAR, ACCELERATOR, SLOWSTART
    }

    public static SolveType pickSolveType(int dlen, int slen) {
        if(dlen > slen) dlen = slen;

        if(dlen > 12 && Random.value > 0.9) return SolveType.SLOWSTART;
        if(dlen > 4 && Random.value > 0.75) return SolveType.ACCELERATOR;
        return SolveType.REGULAR;
    }

    public static float getDelay(SolveType type, int curpos, int dlen, int slen) {
        switch(type) {
            case SolveType.SLOWSTART: {
                if(curpos < 8) return 0.5f + Random.value * 2.5f;
                return 0.05f;
            }
            case SolveType.ACCELERATOR: return Mathf.Max(3f / (float)(curpos+1), 0.05f);
            default: return 0.05f;
        }
    }

    public static bool getMusicToggle(SolveType type, int curpos, int dlen, int slen) {
        if(type == SolveType.SLOWSTART) return (curpos == 1) || (curpos == 8);
        return false;
    }
}
