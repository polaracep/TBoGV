using System;
using System.Text.Json;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;
class DialogueSkolnik : Dialogue
{
    public override string NpcName { get; set; } = EntitySkolnik.NAME;
    public override Texture2D NpcSprite { get; set; } = new EntitySkolnik().GetSprite();
    protected override JsonElement root { get; set; } = DialogueManager.GetDialogue("skolnik").RootElement;
    public DialogueSkolnik()
    {
        Actions.Add(1, () =>
        {
            Respond("fail");
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

        ParseDialogue();
    }

    protected void TryPersuade()
    {
        ItemContainerable item = GameManager.Player.Inventory.ItemContainers.Find(x => x.ContainerType == ItemTypes.ARMOR).Item;

        if (item is ItemFancyShoes or ItemFlipFlop)
            Respond("success");
        else if (Random.Shared.Next(2) == 0)
            Respond("success");
        else
            Respond("fail");
    }
    protected void FancyCheck()
    {
        ItemContainerable item = GameManager.Player.Inventory.ItemContainers.Find(x => x.ContainerType == ItemTypes.ARMOR).Item;
        if (item is ItemFancyShoes or ItemFlipFlop)
            Respond("success");
        else
            Respond("fail");
    }

}
