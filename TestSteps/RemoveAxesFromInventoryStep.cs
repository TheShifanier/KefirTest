using System.Collections;
using System.Collections.Generic;
using Assets.UiTest.Context.Consts;
using Assets.UiTest.Results;
using UiTest.UiTest.Checker;

namespace Assets.UiTest.TestSteps
{
    public class RemoveAxesFromInventoryStep : UiTestStepBase
    {
        public override string Id => "remove_axes_from_inventory";
        public override double TimeOut => 100;

        protected override Dictionary<string, string> GetArgs()
        {
            return new Dictionary<string, string>();
        }

        protected override IEnumerator OnRun()
        {
            yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory,
                new ResultData<SimpleCommandResult>());

            yield return Commands.WaitForSecondsCommand(2, new ResultData<SimpleCommandResult>());
            for (int i = 0; i <= 2; i++) CheckItemCountAtInventory(1, i);
            yield return DeleteAllAxesInInventory();
            CheckAxesIconInInventory();
            yield return DeleteAllAxesInInventory();
            
        }
        private void CheckAxesIconInInventory()
        {
            for (int i = 0; i <= 2; i++)
            {
                var cellIconChecker = new IconEmptyChecker(Context, Screens.Inventory.Cell.Pockets, i);
                if (!cellIconChecker.Check())
                {
                    Fail($"Missed axe icon at pockets cell: {i}");
                }
            }
        }

        private IEnumerator DeleteAllAxesInInventory()
        {
            var inventory = Context.Inventory.GetContent(Screens.Inventory.Content.InventoryCount.Item).GetGO();

            for (int i = 0; i <= 2; i++)
            {
                Context.Inventory.GetCells(Screens.Inventory.Cell.Pockets.Item).GetCell(i);
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                Context.Inventory.GetCells(Screens.Inventory.Cell.Pockets.Item).ClickCell(i);
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Delete, new ResultData<SimpleCommandResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            }
            //var cellObj = Context.Inventory.GetCells(Screens.Inventory.Cell.Pockets.Item).GetCell(2);
            yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Close, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        }

        private void CheckIsUseButtonActive()
        {
            var useButtonIsActive = new UseButtonIsActiveChecker(Context);
            if (!useButtonIsActive.Check())
            {
                Fail("Use button is not active");
            }
        }

        private IEnumerator CheckItemCountAtInventory(int woodCount, int cellIndex)
        {
            yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());

            var cellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, cellIndex, woodCount);
            if (!cellCountChecker.Check())
            {
                Fail($"Wrong item count at pockets cell: {cellIndex}, expected count: {woodCount}");
            }

            yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Close, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        }
    }
}