using System.Collections.Generic;
using System.Globalization;

public static class MidiParser
{
    public static List<NoteData> Parse(string rawData)
    {
        var notes = new List<NoteData>();

        rawData = rawData.Replace("\n", "")
                         .Replace("\r", "")
                         .Replace(" ", "");

        string[] entries = rawData.Split(',');

        foreach (var entry in entries)
        {
            if (string.IsNullOrEmpty(entry))
                continue;

            int id = 0;
            float ta = 0f;
            float duration = 0f;
            int pid = 0;

            string[] parts = entry.Split('-');

            foreach (var part in parts)
            {
                string[] kv = part.Split(':');

                if (kv.Length != 2)
                    continue;

                string key = kv[0];
                string value = kv[1];

                switch (key)
                {
                    case "id":
                        int.TryParse(value, out id);
                        break;

                    case "ta":
                        float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out ta);
                        break;

                    case "d":
                        float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out duration);
                        break;

                    case "pid":
                        int.TryParse(value, out pid);
                        break;
                }
            }

            notes.Add(new NoteData(id, ta, duration, pid));
        }

        return notes;
    }
}