using System;
using System.Text.Json;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;
class DialogueSkolnik : Dialogue
{
    public override string NpcName { get; set; } = EntitySkolnik.NAME;
    public override Texture2D NpcSprite { get; set; } = new EntitySkolnik().GetSprite();
    protected override JsonElement root { get; set; } = DialogueManager.GetDialogue("skolnik").RootElement;

    public DialogueSkolnik() : base()
    {
        Actions.Add(1, () =>
        {
            ScreenManager.ScreenGame.OpenDialogue(new DialogueSkolnikF());
        });
        Actions.Add(2, () =>
        {
            TryPersuade();
        });
        Actions.Add(3, () =>
        {
            FancyCheck();
        });
        Actions.Add(4, () =>
        {
            GameManager.Player.Inventory.AddEffect(new EffectPrezuvky());
        });

    }

    protected void TryPersuade()
    {
        ItemContainerable item = GameManager.Player.Inventory.ItemContainers.Find(x => x.ContainerType == ItemTypes.ARMOR).Item;

        if (item is ItemFancyShoes or ItemFlipFlop)
            ScreenManager.ScreenGame.OpenDialogue(new DialogueSkolnikS());
        else if (Random.Shared.Next(2) == 0)
            ScreenManager.ScreenGame.OpenDialogue(new DialogueSkolnikS());
        else
            ScreenManager.ScreenGame.OpenDialogue(new DialogueSkolnikF());
    }
    protected void FancyCheck()
    {
        ItemContainerable item = GameManager.Player.Inventory.ItemContainers.Find(x => x.ContainerType == ItemTypes.ARMOR).Item;
        if (item is ItemFancyShoes or ItemFlipFlop)
            ScreenManager.ScreenGame.OpenDialogue(new DialogueSkolnikS());
        else
            ScreenManager.ScreenGame.OpenDialogue(new DialogueSkolnikF());
    }

}

class DialogueSkolnikS : DialogueSkolnik
{
    protected override JsonElement root { get; set; } = DialogueManager.GetDialogue("skolnikSuccess").RootElement;
    public DialogueSkolnikS() : base() { }
}
class DialogueSkolnikF : DialogueSkolnik
{
    protected override JsonElement root { get; set; } = DialogueManager.GetDialogue("skolnikFailed").RootElement;
    public DialogueSkolnikF() : base() { }
}