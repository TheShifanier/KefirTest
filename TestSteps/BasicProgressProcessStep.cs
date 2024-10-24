using System.Collections;
using System.Collections.Generic;
using Assets.UiTest.Context.Consts;
using Assets.UiTest.Results;
using UiTest.UiTest.Checker;

namespace Assets.UiTest.TestSteps
{
    public class BasicProgressProcessStep : UiTestStepBase
    {
        public override string Id => "basic_progress_process";
        public override double TimeOut => 100;

        protected override Dictionary<string, string> GetArgs()
        {
            return new Dictionary<string, string>();
        }

        protected override IEnumerator OnRun()
        {
            yield return Commands.FindAndGoToSingleObjectCommand(Locations.Home.WorkbenchSawmill, new ResultData<PlayerMoveResult>());
            yield return Commands.UseButtonClickCommand(Screens.Main.Button.Use, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());

            var initialProgressBarAmount = Context.ProgressBarAmount();
            if (initialProgressBarAmount != 0f)
            {
                Fail("Progress bar initial value not equals 0");
            }
            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.Pockets, 0, Screens.Inventory.Cell.WorkbenchRow, 0, new ResultData<SimpleCommandResult>());
            
            var iconEmptyChecker = new IconEmptyChecker(Context, Screens.Inventory.Cell.WorkbenchRow, 0);
            if (iconEmptyChecker.Check())
            {
                Fail("Missed axe icon in the WorkbenchRow cell");
            }
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            var currentProgressBarAmmount = Context.ProgressBarAmount();
            if (currentProgressBarAmmount != initialProgressBarAmount)
            {
                Fail("Progress process should not start with axe in WorkbenchRow Cell");
            }
            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.WorkbenchRow, 0, Screens.Inventory.Cell.Pockets, 0, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            
            yield return BasicWoodProcessing();
            yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Close, new ResultData<SimpleCommandResult>());
        }

        private IEnumerator BasicWoodProcessing()
        {
            Cheats.GetWood(1);
            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.Pockets, 4, Screens.Inventory.Cell.WorkbenchRow, 0, new ResultData<SimpleCommandResult>());
            yield return Context.WaitEndFrame;
            if (Context.ProgressBarAmount() == 0f)
            {
                Fail("Workbench progress process did not start after wood drag to WorkbenchRow cell");
            }
            yield return Commands.WaitForSecondsCommand(9, new ResultData<SimpleCommandResult>());
            var woodPlankIconEmptyChecker = new IconEmptyChecker(Context, Screens.Inventory.Cell.WorkbenchResult, 0);
            if (woodPlankIconEmptyChecker.Check())
            {
                Fail("Workbench progress process does not end in the right timing");
            }
            var woodIconEmptyChecker = new IconEmptyChecker(Context, Screens.Inventory.Cell.WorkbenchRow, 0);
            if (!woodIconEmptyChecker.Check())
            {
                Fail("Wood item does not removed after progress process ended");
            }
            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.WorkbenchResult, 0, Screens.Inventory.Cell.Pockets, 4, new ResultData<SimpleCommandResult>());
            yield return Context.WaitEndFrame;
        }
    }
}