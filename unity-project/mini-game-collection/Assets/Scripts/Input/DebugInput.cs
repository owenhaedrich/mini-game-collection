using System.Text;
using TMPro;
using UnityEngine;

public class DebugInput : MonoBehaviour
{
    [field: SerializeField]
    public TMP_Text Display { get; private set; }
    
    public string[] Inputs { get; private set; } =
    {
        "P1_AxisX",
        "P1_AxisY",
        "P1_Action1",
        "P1_Action2",
        "P2_AxisX",
        "P2_AxisY",
        "P2_Action1",
        "P2_Action2",
        "Submit",
        "Cancel",
    };

    private readonly StringBuilder stringBuilder = new StringBuilder();


    void Update()
    {
        if (Display == null)
            return;

        stringBuilder.Clear();
        int max = GetMaxLength();

        foreach (var input in Inputs)
        {
            string inputDisplay = input.PadRight(max, '.');
            string inputValue = input.Contains("Axis")
                ? Input.GetAxis(input).ToString("+0.000;-0.000; 0.000") // axis
                : Input.GetButton(input) ? " ON" : " OFF"; // button
            string value = $"{inputDisplay}: {inputValue}\n";
            stringBuilder.Append(value);
        }

        Display.text = stringBuilder.ToString();
    }

    private int GetMaxLength()
    {
        int max = 0;
        foreach (var input in Inputs)
        {
            if (input.Length > max)
                max = input.Length;
        }
        return max;
    }

    private void OnValidate()
    {
        if (Display == null)
            return;

        if (string.IsNullOrEmpty(Display.text))
            Update();
    }
}
