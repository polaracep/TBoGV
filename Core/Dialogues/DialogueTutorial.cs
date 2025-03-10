using System.Text.Json;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

abstract class DialogueTutorial : Dialogue
{
    public override string NpcName { get; set; } = "Schovánek";
    public override Texture2D NpcSprite { get; set; } = TextureManager.GetTexture("wiseman");
}

class DialogueTutorial1 : DialogueTutorial
{
    protected override JsonElement root { get; set; } = DialogueManager.GetDialogue("tutorial1").RootElement;
}

class DialogueTutorial2 : DialogueTutorial
{
    protected override JsonElement root { get; set; } = DialogueManager.GetDialogue("tutorial2").RootElement;
}
class DialogueTutorial3 : DialogueTutorial
{
    protected override JsonElement root { get; set; } = DialogueManager.GetDialogue("tutorial3").RootElement;
}

class DialogueTutorial4 : DialogueTutorial
{
    protected override JsonElement root { get; set; } = DialogueManager.GetDialogue("tutorial4").RootElement;
}

