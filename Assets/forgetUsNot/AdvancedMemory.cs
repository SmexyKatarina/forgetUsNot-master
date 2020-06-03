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
using System.IO;



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
    public TextAsset codes;

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
    private int getCode(string  s)
    {
        int r = 998;
        foreach (string code in codes.text.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)) {
            string[] split = code.Trim().Split(new char[] { ':' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (s == split[0]) {
                r = int.Parse(split[1].Trim());
                break;
            }
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
