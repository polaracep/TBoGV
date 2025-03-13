using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

public class EntityGambler : EntityPassive, IInteractable
{
    public const string NAME = "Metoděj";
    public bool BetPlaced { get; private set; } = false;
    public bool Won = false;
    public bool ReadyToPay { get; private set; } = false;
    public bool Done { get; private set; } = false;
    private int betAmount;
    public EntityGambler(Vector2 position) : base(position, NAME) { }
    public EntityGambler() : base(NAME) { }

    public override Texture2D GetSprite()
    {
        return TextureManager.GetTexture("gambler");
    }
    public void Interact(Entity e, Place _)
    {
        if (e is not Player p)
            return;
        if (Done)
            ScreenManager.ScreenGame.OpenDialogue(new DialogueBasic(DialogueManager.GetDialogue("gamblerDone").RootElement, NAME, GetSprite()));
        else if (ReadyToPay)
        {
            Done = true;
            ScreenManager.ScreenGame.OpenDialogue(new DialogueGamblerResult(p, this));
        }
        else if (!ReadyToPay && BetPlaced)
            ScreenManager.ScreenGame.OpenDialogue(new DialogueBasic(DialogueManager.GetDialogue("gamblerWait").RootElement, NAME, GetSprite()));
        else
            ScreenManager.ScreenGame.OpenDialogue(new DialogueGambler(p, this));
    }

    public void SetBet(int amount)
    {
        if (amount <= 0)
            return;

        betAmount = amount;
        BetPlaced = true;
    }

    public void EvalBet()
    {
        ReadyToPay = true;
        // wohoo!
        if (Random.Shared.Next(3) == 0)
            Won = true;
        else
            Won = false;
    }

    public int PayoutBet()
    {
        BetPlaced = false;
        Done = true;
        if (Won)
            return betAmount * 2;
        else
            return 0;
    }

}