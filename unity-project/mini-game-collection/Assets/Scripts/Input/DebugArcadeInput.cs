using MiniGameCollection;
using System;
using System.Text;
using TMPro;
using UnityEngine;

public class DebugArcadeInput : MonoBehaviour
{
    [field: SerializeField] public TMP_Text Display { get; private set; }
    [field: SerializeField, Range(0, 1)] public int PlayerID { get; private set; } = 0;
    [field: SerializeField] public int PaddingLength { get; private set; } = 0;

    private readonly StringBuilder stringBuilder = new StringBuilder();

    private readonly struct Entry
    {
        private readonly string Name;
        private readonly string Value;
        private readonly int PaddingLength;

        public Entry(string name, string value, int paddingLength)
        {
            Name = name;
            Value = value;
            PaddingLength = paddingLength;
        }

        public override string ToString()
        {
            string paddedName = Name.PadRight(PaddingLength, '.');
            string str = $"{paddedName}: {Value}";
            return str;
        }
    }

    void Update()
    {
        if (Display == null)
            return;

        ReadOnlySpan<Entry> entries = new ReadOnlySpan<Entry>(
            new Entry[]
            {
                new Entry("Joystick", Axis2ToString(ArcadeInput.Players[PlayerID].Joystick8Way), PaddingLength),
                new Entry("AxisX", AxisToString(ArcadeInput.Players[PlayerID].AxisX), PaddingLength),
                new Entry("AxisY", AxisToString(ArcadeInput.Players[PlayerID].AxisY), PaddingLength),
                new Entry("Stick Left", ButtonToString(ArcadeInput.Players[PlayerID].Left.Down), PaddingLength),
                new Entry("Stick Right", ButtonToString(ArcadeInput.Players[PlayerID].Right.Down), PaddingLength),
                new Entry("Stick Down", ButtonToString(ArcadeInput.Players[PlayerID].Down.Down), PaddingLength),
                new Entry("Stick Up", ButtonToString(ArcadeInput.Players[PlayerID].Up.Down), PaddingLength),
                new Entry("Action1", ButtonToString(ArcadeInput.Players[PlayerID].Action1.Down), PaddingLength),
                new Entry("Action2", ButtonToString(ArcadeInput.Players[PlayerID].Action2.Down), PaddingLength),
            }
        );

        stringBuilder.Clear();
        stringBuilder.AppendLine($"Player {PlayerID + 1}");
        foreach (var entry in entries)
            stringBuilder.AppendLine(entry.ToString());

        Display.text = stringBuilder.ToString();
    }

    public string AxisToString(float axisValue)
    {
        string value = axisValue.ToString("+0.000;-0.000; 0.000");
        return value;
    }
    public string Axis2ToString(Vector2 axisValues)
    {
        string x = AxisToString(axisValues.x);
        string y = AxisToString(axisValues.y);
        string value = $"X:{x}, Y:{y}";
        return value;
    }
    public string ButtonToString(bool buttonValue)
    {
        string value = buttonValue ? " ON" : " OFF";
        return value;
    }

    //private void OnValidate()
    //{
    //    if (Display == null)
    //        return;

    //    //if (string.IsNullOrEmpty(Display.text))
    //        Update();
    //}
}
