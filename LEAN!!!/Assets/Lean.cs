using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

public class Lean : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;

   public KMSelectable[] Buttons;
   public TextMesh[] DisplayTexts;

   string ButtonOrder = "";

   List<int> ButtonsToPress = new List<int>() { };

   string[] Keywords = { "PURPLE DRANK", "SIZZURP", "PURPLE JELLY", "PURP", "OIL", "SYRUP", "DRANK", "BARRE", "WOK", "TEXAS TEA", "MEMPHIS MUD", "DIRTY SPRITE", "PURPLE OIL", "SLURP", "CODY", "LEAN!!!" };
   string[] Orders = { "R", "ABC", "G", "ACB", "BAC", "H", "CAB", "G", "CBA", "BCA", "ABC", "H", "CBA", "R", "BAC", "U" };

   string[] Sounds = { "baln_lean", "Broke", "Caleb", "JayLean", "melean", "depressolean"};

   bool[] LeanCap = new bool[4];

   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;

   void Awake () {
      ModuleId = ModuleIdCounter++;
      
      foreach (KMSelectable Button in Buttons) {
          Button.OnInteract += delegate () { ButtonPress(Button); return false; };
      }
      
   }

   void ButtonPress (KMSelectable Button) {
      Button.AddInteractionPunch();
      if (ModuleSolved) {
         Audio.PlaySoundAtTransform(Sounds[Rnd.Range(0, Sounds.Length)], Button.transform);
         return;
      }
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Button.transform);
      for (int i = 0; i < 16; i++) {
         if (Button == Buttons[i]) {
            if (ButtonsToPress[0] == i) {
               ButtonsToPress.RemoveAt(0);
            }
            else {
               Debug.Log(i);
               GetComponent<KMBombModule>().HandleStrike();
            }
         }
      }
      if (ButtonsToPress.Count() == 0) {
         GetComponent<KMBombModule>().HandlePass();
         Audio.PlaySoundAtTransform(Sounds[Rnd.Range(0, Sounds.Length)], Button.transform);
         ModuleSolved = true;
      }
   }

   void Start () {
      int Random = Rnd.Range(0, Keywords.Length);
      ButtonOrder = Orders[Random];

      DisplayTexts[0].text = "[" + Keywords[Random] + "]";
      DisplayTexts[1].text = "";

      for (int i = 0; i < 4; i++) {
         if (Rnd.Range(0, 2) == 1) {
            LeanCap[i] = true;
            DisplayTexts[1].text += "LEAN"[i].ToString();
         }
         else {
            DisplayTexts[1].text += "lean"[i].ToString();
         }
      }

      DisplayTexts[1].text = "[" + DisplayTexts[1].text + "]";

      Debug.LogFormat("[LEAN!!! #{0}] The displayed text is {1}, and the displayed variation of lean is {2}.", ModuleId, DisplayTexts[0].text, DisplayTexts[1].text);

      if (ButtonOrder == "U") {
         Debug.LogFormat("[LEAN!!! #{0}] i luv len <3. pres a1 b2 c3 smhile.", ModuleId);
         ButtonsToPress.Add(0);
         ButtonsToPress.Add(5);
         ButtonsToPress.Add(10);
         return;
      }
      if (ButtonOrder == "R") {
         Debug.LogFormat("[LEAN!!! #{0}] Press the buttons in Reading Order.", ModuleId);
      }
      else if (ButtonOrder == "H") {
         Debug.LogFormat("[LEAN!!! #{0}] Press the buttons in Chinese Reading Order.", ModuleId);
      }
      else if (ButtonOrder == "G") {
         Debug.LogFormat("[LEAN!!! #{0}] Press the buttons in Geometric Order.", ModuleId);
      }
      else {
         Debug.LogFormat("[LEAN!!! #{0}] Press the buttons in {1} Order.", ModuleId, ButtonOrder);
      }
      Calculation();
   }

   void Calculation () {
      int a = Bomb.GetSerialNumberNumbers().First();
      int b = Bomb.GetModuleNames().Count();
      int c = Bomb.GetSerialNumberNumbers().Last();

      Debug.LogFormat("[LEAN!!! #{0}] A's initial value is {1}, B is {2}, C is {3}.", ModuleId, a, b, c);

      bool L = LeanCap[0];
      bool E = LeanCap[1];
      bool A = LeanCap[2];
      bool N = LeanCap[3];

      if (A && E) {
         a = a - 5;
         a = a < 0 ? a += 16 : a;
         Debug.LogFormat("[LEAN!!! #{0}] A becomes {1} (AE).", ModuleId, a);
      }
      if (L && N && !A) {
         b = b / 3;
         Debug.LogFormat("[LEAN!!! #{0}] B becomes {1} (LaN).", ModuleId, b);
      }
      if (L && E) {
         c = (a + b) % 16;
         Debug.LogFormat("[LEAN!!! #{0}] C becomes {1} (LE).", ModuleId, c);
      }
      if (!E) {
         b -= 7;
         b = b < 0 ? b + 16 : b;
         Debug.LogFormat("[LEAN!!! #{0}] B becomes {1} (e).", ModuleId, b);
      }
      if (!L && !E && A) {
         b = b * 2 % 16;
         Debug.LogFormat("[LEAN!!! #{0}] B becomes {1} (leA).", ModuleId, b);
      }
      if (!L && !N && A) {
         c /= 2;
         Debug.LogFormat("[LEAN!!! #{0}] C becomes {1} (lAn).", ModuleId, c);
      }
      if (E && !A) {
         b = (a + c) % 16;
         Debug.LogFormat("[LEAN!!! #{0}] B becomes {1} (Ea).", ModuleId, b);
      }
      if (L && E && !A && !N) {
         a = (b + c) % 16;
         Debug.LogFormat("[LEAN!!! #{0}] A becomes {1} (LEan).", ModuleId, a);
      }
      if (L && E && A) {
         c = b * a % 16;
         Debug.LogFormat("[LEAN!!! #{0}] C becomes {1} (LEA).", ModuleId, c);
      }
      if (A) {
         a = (a + 7) % 16;
         Debug.LogFormat("[LEAN!!! #{0}] A becomes {1} (A).", ModuleId, a);
      }
      if (L && A && !E) {
         c -= 3;
         c = c < 0 ? c + 16 : c;
         Debug.LogFormat("[LEAN!!! #{0}] C becomes {1} (LeA).", ModuleId, c);
      }
      if (L && !E) {
         c = (c + 5) % 16;
         Debug.LogFormat("[LEAN!!! #{0}] C becomes {1} (Le).", ModuleId, c);
      }
      if (N && !A) {
         a /= 4;
         Debug.LogFormat("[LEAN!!! #{0}] A becomes {1} (aN).", ModuleId, a);
      }
      if (!L && !E && !A && !N) {
         a = 5;
         Debug.LogFormat("[LEAN!!! #{0}] A becomes {1} (lean).", ModuleId, a);
      }
      if (L && E && A && N) {
         b = 5;
         Debug.LogFormat("[LEAN!!! #{0}] B becomes {1} (LEAN).", ModuleId, b);
      }
      if (!E && !A && !N && L) {
         c = 5;
         Debug.LogFormat("[LEAN!!! #{0}] C becomes {1} (Lean).", ModuleId, c);
      }

      while (a == b || a == c || b == c) {
         a = (a + 2) % 16;
         b = (b + 1) % 16;
         Debug.LogFormat("[LEAN!!! #{0}] Adjusting for duplicates gives us a = {1}, b = {2}, c = {3}.", ModuleId, a, b, c);
      }

      for (int i = 0; i < 3; i++) {
         switch (ButtonOrder[i]) {
            case 'A':
               Debug.LogFormat("[LEAN!!! #{0}] Press a ({1}).", ModuleId, a + 1);
               ButtonsToPress.Add(a);
               break;
            case 'B':
               ButtonsToPress.Add(b);
               Debug.LogFormat("[LEAN!!! #{0}] Press b ({1}).", ModuleId, b + 1);
               break;
            case 'C':
               Debug.LogFormat("[LEAN!!! #{0}] Press c ({1}).", ModuleId, c + 1);
               ButtonsToPress.Add(c);
               break;
            case 'R':
               for (int j = 0; j < 16; j++) {
                  if (a == j) {
                     Debug.LogFormat("[LEAN!!! #{0}] Press a ({1}).", ModuleId, a + 1);
                     ButtonsToPress.Add(a);
                  }
                  if (b == j) {
                     Debug.LogFormat("[LEAN!!! #{0}] Press b ({1}).", ModuleId, b + 1);
                     ButtonsToPress.Add(b);
                  }
                  if (c == j) {
                     Debug.LogFormat("[LEAN!!! #{0}] Press c ({1}).", ModuleId, c + 1);
                     ButtonsToPress.Add(c);
                  }
               }
               return;
            case 'G':
               for (int k = 3; k >= 0; k--) {
                  for (int j = 0; j < 4; j++) {
                     if (a == k * 4 + j) {
                        Debug.LogFormat("[LEAN!!! #{0}] Press a ({1}).", ModuleId, a + 1);
                        ButtonsToPress.Add(a);
                     }
                     if (b == k * 4 + j) {
                        Debug.LogFormat("[LEAN!!! #{0}] Press b ({1}).", ModuleId, b + 1);
                        ButtonsToPress.Add(b);
                     }
                     if (c == k * 4 + j) {
                        Debug.LogFormat("[LEAN!!! #{0}] Press c ({1}).", ModuleId, c + 1);
                        ButtonsToPress.Add(c);
                     }
                  }
               }
               return;
            case 'H':
               for (int k = 3; k >= 0; k--) {
                  for (int j = 0; j < 4; j++) {
                     if (a == k + j * 4) {
                        Debug.LogFormat("[LEAN!!! #{0}] Press a ({1}).", ModuleId, a + 1);
                        ButtonsToPress.Add(a);
                     }
                     if (b == k + j * 4) {
                        Debug.LogFormat("[LEAN!!! #{0}] Press b ({1}).", ModuleId, b + 1);
                        ButtonsToPress.Add(b);
                     }
                     if (c == k + j * 4) {
                        Debug.LogFormat("[LEAN!!! #{0}] Press c ({1}).", ModuleId, c + 1);
                        ButtonsToPress.Add(c);
                     }
                  }
               }
               return;
         }
      }
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} A/B/C/D/1/2/3/4. Chain commands via spaces.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      Command = Command.Trim().ToUpper();
      yield return null;
      string[] Commands = Command.Split(' ');
      for (int i = 0; i < Commands.Length; i++) {
         if (!"ABCD".Contains(Commands[i][0]) || !"1234".Contains(Commands[i][1]) || Commands[i].Length != 2) {
            yield return "sendtochaterror I don't understand!";
            yield break;
         }
      }
      for (int i = 0; i < Commands.Length; i++) {
         Buttons[Array.IndexOf("ABCD".ToCharArray(), Commands[i][0]) + (int.Parse(Commands[i][1].ToString()) - 1) * 4].OnInteract();
         yield return new WaitForSeconds(.1f);
      }
   }

   IEnumerator TwitchHandleForcedSolve () {
      while (ButtonsToPress.Count() != 0) {
         Buttons[ButtonsToPress[0]].OnInteract();
         yield return new WaitForSeconds(.1f);
      }
   }
}
