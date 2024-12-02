using System;
using UnityEngine;

public static class AudioClipConverter
{
    public static string ConvertToBase64(AudioClip clip)
    {
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        byte[] audioData = new byte[samples.Length * 2];
        for (int i = 0; i < samples.Length; i++)
        {
            short sample = (short)(samples[i] * 32768);
            audioData[i * 2] = (byte)(sample & 0xFF);
            audioData[i * 2 + 1] = (byte)((sample >> 8) & 0xFF);
        }

        return Convert.ToBase64String(audioData);
    }
}
