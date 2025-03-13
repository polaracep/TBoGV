using System.Text.Json;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;
class DialogueGambler : Dialogue
{
    public override string NpcName { get; set; } = EntityGambler.NAME;
    public override Texture2D NpcSprite { get; set; } = new EntityGambler().GetSprite();
    protected override JsonElement root { get; set; } = DialogueManager.GetDialogue("gambler").RootElement;

    public DialogueGambler(Player player, EntityGambler gambler)
    {
        Actions.Add(1, () =>
        {
            ScreenManager.ScreenGame.OpenMenu(new InGameMenuBet(GameManager.Viewport, player, gambler));
        });

        ParseDialogue();
    }

}

class DialogueGamblerResult : Dialogue
{
    public override string NpcName { get; set; } = EntityGambler.NAME;
    public override Texture2D NpcSprite { get; set; } = new EntityGambler().GetSprite();
    protected override JsonElement root { get; set; } = DialogueManager.GetDialogue("gamblerResult").RootElement;
    public DialogueGamblerResult(Player player, EntityGambler gambler)
    {
        Actions.Add(1, () =>
        {
            if (gambler.Won)
                Respond("won");
            else
                Respond("lost");
        });
        Actions.Add(2, () =>
        {
            player.Coins += gambler.PayoutBet();
        });

        ParseDialogue();
    }
}