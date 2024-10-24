using System.Collections;
using System.Collections.Generic;
using Assets.UiTest.Context.Consts;
using Assets.UiTest.Results;
using UiTest.UiTest.Checker;

namespace Assets.UiTest.TestSteps
{
    public class CheckAxeLifecycleBasedOnTreeHitsStep : UiTestStepBase
    {
        public override string Id => "check_axe_lifecycle_based_on_tree_hits";
        public override double TimeOut => 300;

        protected override Dictionary<string, string> GetArgs()
        {
            return new Dictionary<string, string>();
        }

        private const int WOOD_COUNT_FROM_TREE = 3;

        protected override IEnumerator OnRun()
        {
            yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return DeleteTwoAxesInInventory();
            var treesOnScene = Cheats.FindTree();
            for (int i = 0; i <= 2; i++) 
            {
                yield return Commands.PlayerMoveCommand(treesOnScene[i].transform.position, new ResultData<PlayerMoveResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                yield return HitTreeAndCheckAxeExistence(true);
                CheckCountInWoodStacks(WOOD_COUNT_FROM_TREE + i * 3, 1);
            }
            yield return Commands.PlayerMoveCommand(treesOnScene[3].transform.position, new ResultData<PlayerMoveResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            yield return HitTreeAndCheckAxeExistence(false);
        }

        private IEnumerator DeleteTwoAxesInInventory()
        {
            var inventory = Context.Inventory.GetContent(Screens.Inventory.Content.InventoryCount.Item).GetGO();

            for (int i = 1; i <= 2; i++)
            {
                Context.Inventory.GetCells(Screens.Inventory.Cell.Pockets.Item).GetCell(i);
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                Context.Inventory.GetCells(Screens.Inventory.Cell.Pockets.Item).ClickCell(i);
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Delete, new ResultData<SimpleCommandResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            }
            yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Close, new ResultData<SimpleCommandResult>());
            yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
        }

        private IEnumerator HitTreeAndCheckAxeExistence(bool existenceExpected)
        {
            if (existenceExpected)
            {
                for (int i = 1; i <= 3; i++)
                {
                    yield return Commands.UseButtonClickCommand(Screens.Main.Button.Use, new ResultData<SimpleCommandResult>());
                    yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                    yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
                    yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                    var cellIconChecker = new IconEmptyChecker(Context, Screens.Inventory.Cell.Pockets, 0);
                    if (cellIconChecker.Check())
                    {
                        Fail("Cell should have axe");
                    }
                    yield return Commands.UseButtonClickCommand(Screens.Inventory.Button.Close, new ResultData<SimpleCommandResult>());
                    yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                }
            }
            else 
            {
                yield return Commands.UseButtonClickCommand(Screens.Main.Button.Use, new ResultData<SimpleCommandResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                yield return Commands.UseButtonClickCommand(Screens.Main.Button.Inventory, new ResultData<SimpleCommandResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                var cellIconChecker = new IconEmptyChecker(Context, Screens.Inventory.Cell.Pockets, 0);
                if (!cellIconChecker.Check())
                {
                    Fail("Cell should not have axe");
                }
            }
        }

        private void CheckCountInWoodStacks(int expectedAmmount, int cellWithWoodStack)
        {
            var cellCountChecker = new CellCountChecker(Context, Screens.Inventory.Cell.Pockets, cellWithWoodStack, expectedAmmount);
            if (!cellCountChecker.Check())
            {
                Fail($"Wrong wood count at pockets cell: {cellWithWoodStack}");
            }
        }
    }
}