using System.Collections.Generic;
using Assets.UiTest.TestSteps;

namespace Assets.UiTest.Runner;

public class CheckMapBoundariesTestCase : UiStepsTestCase
{
    protected override IEnumerator<IUiTestStepBase> Condition()
    {
        /* This testcase check the behaviour when the player moves out of the boundaries
         * Since the game in the current version does not take into account
         * going beyond the boundaries of the world and does not react to this, that is, we might check solely 
         * the fact that the hero does not lose his position (does not fall under the stage). 
         */
        yield return Steps.WaitStartLoadingStep();
        yield return Steps.MoveAcrossBoundariesAndCheckPlayerPostitionStep();
        /* After fixing the bug of possibility going out of boundaties
         * We might check what developers decided to do in that scenario:
         * 1) It might be a teleportation to some position in the boundaries:
         *      yield return Steps.VerifyPlayerTeleportedToSpawn();
         * 2) Or Warning dialog and death like:
         *      yield return Steps.WaitForWarningDialog();
         *      yield return Steps.ConfirmPlayerDeath();
         *      yield return Steps.GetWastedMessageAndRestartGame();
         */
    }
}