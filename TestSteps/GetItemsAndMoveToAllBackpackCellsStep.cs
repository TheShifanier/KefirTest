using System.Collections;
using System.Collections.Generic;
using Assets.UiTest.Context.Consts;
using Assets.UiTest.Results;
using UiTest.UiTest.Checker;

namespace Assets.UiTest.TestSteps
{
    // Test will fail because of bug with double itemCount and missing icon in some Backpack cells
    public class GetItemsAndMoveToAllBackpackCellsStep : UiTestStepBase
    {
        public override string Id => "get_axe_and_move_to_all_backpack_cells";
        public override double TimeOut => 200;

        protected override Dictionary<string, string> GetArgs()
        {
            return new Dictionary<string, string>();
        }
        protected override IEnumerator OnRun()
        {
            yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return DeleteAllItemsInInventory();

            var axeCount = 1;
            var coinsCount = 3;
            var woodCount = 5;

            Cheats.GetAxe(axeCount);
            yield return Context.WaitEndFrame;
            Cheats.GetCoins(coinsCount);
            yield return Context.WaitEndFrame;
            Cheats.GetWood(woodCount);
            yield return Context.WaitEndFrame;

            yield return DragAndDropToAllBackpackCells(0, "axe", axeCount);
            yield return DragAndDropToAllBackpackCells(1, "coins", coinsCount);
            yield return DragAndDropToAllBackpackCells(2, "wood", woodCount);

            yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Close, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        }
       
        private IEnumerator DragAndDropToAllBackpackCells(int cellIndex, string itemName, int itemCount)
        {
            yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.Pockets, cellIndex, Screens.Inventory.Cell.Backpack, 10, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            for (int i = 10; i < 20; i++)
            {
                yield return Commands.DragAndDropCommand(Screens.Inventory.Cell.Backpack, i, Screens.Inventory.Cell.Backpack, i+1, new ResultData<SimpleCommandResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                var cellIconChecker = new IconEmptyChecker(Context, Screens.Inventory.Cell.Backpack, i+1);
                var cellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Backpack, i+1, itemCount);
                if (cellIconChecker.Check())
                {
                    Fail("Cell should have item icon");
                }
                if (!cellCountChecker.Check())
                {
                    var cellActualCount = Cheats.CellCount(Context.Inventory.GetCells(Screens.Inventory.Cell.Backpack.Item).GetCell(i));
                    Fail($"Wrong {itemName} count at backpack cell: {i}. Actual count: {cellActualCount}, expected to be equal: {itemCount}");
                }
            }
        }

        private IEnumerator DeleteAllItemsInInventory()
        {
            for (int i = 0; i < 10; i++)
            {
                Context.Inventory.GetCells(Screens.Inventory.Cell.Pockets.Item).GetCell(i);
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                Context.Inventory.GetCells(Screens.Inventory.Cell.Pockets.Item).ClickCell(i);
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Delete, new ResultData<SimpleCommandResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            }
            for (int i = 10; i <= 20; i++)
            {
                Context.Inventory.GetCells(Screens.Inventory.Cell.Backpack.Item).GetCell(i);
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                Context.Inventory.GetCells(Screens.Inventory.Cell.Backpack.Item).ClickCell(i);
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Delete, new ResultData<SimpleCommandResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            }
        }
    }
}