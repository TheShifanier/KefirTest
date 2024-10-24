using System.Collections;
using System.Collections.Generic;
using Assets.UiTest.Context.Consts;
using Assets.UiTest.Results;
using UiTest.UiTest.Checker;

namespace Assets.UiTest.TestSteps
{
    public class FillInventoryAndStackStep : UiTestStepBase
    {
        public override string Id => "fill_inventory_and_stack";
        public override double TimeOut => 100;

        protected override Dictionary<string, string> GetArgs()
        {
            return new Dictionary<string, string>();
        }

        protected override IEnumerator OnRun()
        {
            yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return DeleteAllItemsInPockets();

            Cheats.GetAxe(2);
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return CheckStackSystemAxe(0, 1);

            Cheats.GetCoins(50);
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            Cheats.GetCoins(50);
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return CheckStackSystemCoins(2, 3);

            Cheats.GetWood(10);
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            Cheats.GetWood(10);
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return CheckStackSystemWoodAndPlank(4, 5, "wood");
            

            Cheats.GetWoodPlank(10);
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            Cheats.GetWoodPlank(10);
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return CheckStackSystemWoodAndPlank(6, 7, "plank");
        }

        private IEnumerator CheckStackSystemAxe(int firstAxeCellIndex, int secondAxeCellIndex)
        {
            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.Pockets, 0, Screens.Inventory.Cell.Pockets, 0, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            var firstCellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, 0, 1);
            var secondCellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, 1, 1);
            if ((!firstCellCountChecker.Check()) || (!secondCellCountChecker.Check()))
            {
                Fail($"Axe should not stack or moved when connected with another axe");
            }
        }
        
        private IEnumerator CheckStackSystemCoins(int stackCellIndex, int nonStackCellIndex)
        {
            var stackCellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, stackCellIndex, 100);
            if (!stackCellCountChecker.Check())
            {
                Fail("Coins should stack and have count: 100");
            }
            Cheats.GetCoins(5);
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            stackCellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, stackCellIndex, 100);
            var nonStacCellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, nonStackCellIndex, 5);
            if ((!stackCellCountChecker.Check()) && (!nonStacCellCountChecker.Check()))
            {
                Fail("Coins should not stack");
            }
        }
        
        private IEnumerator CheckStackSystemWoodAndPlank(int stackCellIndex, int nonStackCellIndex, string woodOrPlank)
        {
            var stackCellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, stackCellIndex, 20);
            if (!stackCellCountChecker.Check())
            {
                Fail("Item should stack and have count: 20");
            }
            if (woodOrPlank == "wood") Cheats.GetWood(1);
            else if (woodOrPlank == "plank") Cheats.GetWoodPlank(1);
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            stackCellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, stackCellIndex, 20);
            var nonStacCellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, nonStackCellIndex, 1);
            if ((!stackCellCountChecker.Check()) || (!nonStacCellCountChecker.Check()))
            {
                Fail("Item should not stack");
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