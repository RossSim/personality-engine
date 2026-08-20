namespace PersonalityEngine;

public static class ChannelKey
{
    public static string Of(string layer, string providerId, string channel) =>
        $"{layer}.{providerId}.{channel}";
}
