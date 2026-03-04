using System.Collections.Generic;
using UnityEngine;

public static class MidiParser
{
    public static List<NoteData> Parse(string rawData)
    {
        var notes = new List<NoteData>();

        string[] entries = rawData.Split(',');

        foreach (var entry in entries)
        {
            int id = 0;
            float ta = 0f;
            float duration = 0f;
            int pid = 0;

            string[] parts = entry.Split('-');

            foreach (var part in parts)
            {
                if (part.StartsWith("id:"))
                    id = int.Parse(part.Replace("id:", ""));

                else if (part.StartsWith("ta:"))
                    ta = float.Parse(part.Replace("ta:", ""));

                else if (part.StartsWith("d:"))
                    duration = float.Parse(part.Replace("d:", ""));

                else if (part.StartsWith("pid:"))
                    pid = int.Parse(part.Replace("pid:", ""));
            }

            notes.Add(new NoteData(id, ta, duration, pid));
        }

        return notes;
    }
}