using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using Assets.UiTest.Context.Consts;
using Assets.UiTest.Results;
using UiTest.UiTest.Checker;

namespace Assets.UiTest.TestSteps
{
    public class ProgressWithSkipStep : UiTestStepBase
    {
        public override string Id => "progress_with_skip";
        public override double TimeOut => 100;

        protected override Dictionary<string, string> GetArgs()
        {
            return new Dictionary<string, string>();
        }

        private const int SKIP_COINS_COST = 5;

        protected override IEnumerator OnRun()
        {
            yield return Commands.FindAndGoToSingleObjectCommand(Locations.Home.WorkbenchSawmill, new ResultData<PlayerMoveResult>());
            yield return Commands.UseButtonClickCommand(Screens.Main.Button.Use, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return DeleteAllItemsInPockets();

            yield return CheckOneTimeSkip();

            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.WorkbenchResult, 0, Screens.Inventory.Cell.Pockets, 9, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());

            yield return CheckMultipleTimesSkip();

            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.WorkbenchRow, 0, Screens.Inventory.Cell.Pockets, 7, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.WorkbenchResult, 0, Screens.Inventory.Cell.Pockets, 8, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.Pockets, 7, Screens.Inventory.Cell.WorkbenchRow, 0, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());

            yield return CheckSkipWithZeroCoins();
        }

        private IEnumerator UseCoinsToSkipOnce(int initialCoinsCount, bool ProcessItemPlaced)
        {
            if (ProcessItemPlaced)
            {
                yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Skip, new ResultData<SimpleCommandResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                var skippedCoinsIconChecker = new IconEmptyChecker(Context, Screens.Inventory.Cell.Pockets, 1);
                if ((initialCoinsCount - SKIP_COINS_COST == 0) && (!skippedCoinsIconChecker.Check()))
                {
                    Fail("Coins Icon expected to be removed since count  has reached zero");
                }
                var skippedCoinsCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, 1, initialCoinsCount - SKIP_COINS_COST);
                if ((initialCoinsCount - SKIP_COINS_COST != 0) && (!skippedCoinsCountChecker.Check()))
                {
                    Fail($"Wrong count of coins left after skip with initial count {initialCoinsCount}");
                }
            }
            else
            {
                yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Skip, new ResultData<SimpleCommandResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                var skippedCoinsCount = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, 1, initialCoinsCount);
                if (!skippedCoinsCount.Check())
                {
                    Fail($"Coins count should not be changed because item for processing not placed");
                }
            }
        }
        private IEnumerator CheckSkipWithZeroCoins()
        {
            yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Skip, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.WorkbenchRow, 0, Screens.Inventory.Cell.Pockets, 7, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());

            var cellresultGo = Context.Inventory.GetCells(Screens.Inventory.Cell.WorkbenchResult.Item).GetCell(0);
            var cellResultCount = Context.Cheats.CellCount(cellresultGo);
            var woodPlankIconChecker = new IconEmptyChecker(Context, Screens.Inventory.Cell.WorkbenchResult, 0);
            if ((!woodPlankIconChecker.Check()) || (cellResultCount > 0))
            {
                Fail("Skip should not work since 0 coins in inventory");
            }
        }

        private IEnumerator CheckOneTimeSkip()
        {
            Cheats.GetWood(1);
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            Cheats.GetCoins(5);
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());

            yield return UseCoinsToSkipOnce(5, false);

            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.Pockets, 0, Screens.Inventory.Cell.WorkbenchRow, 0, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return UseCoinsToSkipOnce(5, true);

            var woodPlankIconEmptyChecker = new IconEmptyChecker(Context, Screens.Inventory.Cell.WorkbenchResult, 0);
            if (woodPlankIconEmptyChecker.Check())
            {
                Fail("Workbench progress process does not end correctly");
            }
            var woodIconEmptyChecker = new IconEmptyChecker(Context, Screens.Inventory.Cell.WorkbenchRow, 0);
            if (!woodIconEmptyChecker.Check())
            {
                Fail("Wood item does not removed after progress process ended");
            }
        }

        private IEnumerator CheckMultipleTimesSkip()
        {
            Cheats.GetWood(20);
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            Cheats.GetCoins(20);
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());

            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.Pockets, 0, Screens.Inventory.Cell.WorkbenchRow, 0, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return UseCoinsToSkipOnce(20, true);
            yield return UseCoinsToSkipOnce(15, true);
            yield return UseCoinsToSkipOnce(10, true);
            yield return UseCoinsToSkipOnce(5, true);

            var cellresultGo = Context.Inventory.GetCells(Screens.Inventory.Cell.WorkbenchResult.Item).GetCell(0);
            var cellResultCount = Context.Cheats.CellCount(cellresultGo);
            var woodPlankIconChecker = new IconEmptyChecker(Context, Screens.Inventory.Cell.WorkbenchResult, 0);
            if ((woodPlankIconChecker.Check()) || (cellResultCount < 4))
            {
                Fail("Multiple skip progress end with error");
            }
        }

        private IEnumerator DeleteAllItemsInPockets()
        {
            for (int i = 0; i < 10; i++)
            {
                Context.Inventory.GetCells(Screens.Inventory.Cell.Pockets.Item).GetCell(i);
                yield return Context.WaitEndFrame;
                Context.Inventory.GetCells(Screens.Inventory.Cell.Pockets.Item).ClickCell(i); 
                yield return Context.WaitEndFrame;
                yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Delete, new ResultData<SimpleCommandResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            }
        }
    }
}