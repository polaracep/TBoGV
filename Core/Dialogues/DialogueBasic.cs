using System.Text.Json;
using Microsoft.Xna.Framework.Graphics;

class DialogueBasic : Dialogue
{
    protected override JsonElement root { get; set; }
    public override string NpcName { get; set; }
    public override Texture2D NpcSprite { get; set; }

    public DialogueBasic(JsonElement dialogue, string npcName, Texture2D npcSprite)
    {
        root = dialogue;
        NpcName = npcName;
        NpcSprite = npcSprite;

        ParseDialogue();
    }
}
