using UnityEngine.Networking;

public class RegisterLoginMessage : MessageBase {

    public string[] userData;

    public override void Serialize(NetworkWriter writer)
    {
        var outString = string.Join("\n", userData);
        writer.Write(outString);
    }

    public override void Deserialize(NetworkReader reader)
    {
        reader.ReadString().Split('\n');
    }
}
