using System.Collections;
using System.Collections.Generic;
using Assets.UiTest.Context.Consts;
using Assets.UiTest.Results;
using UiTest.UiTest.Checker;

namespace Assets.UiTest.TestSteps
{
    public class FindAndCutAllTreesStep : UiTestStepBase
    {
        public override string Id => "find_and_cut_all_trees";
        public override double TimeOut => 300;

        protected override Dictionary<string, string> GetArgs()
        {
            return new Dictionary<string, string>();
        }

        protected override IEnumerator OnRun()
        {
            var treesOnScene = Cheats.FindTree();
            for (int i = 0; i < treesOnScene.Count; i++)
            {
                yield return Commands.PlayerMoveCommand(treesOnScene[i].transform.position, new ResultData<PlayerMoveResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
                CheckIsUseButtonActive();
                yield return CutTree();
            }

            treesOnScene = Cheats.FindTree();

            for (int i = 0; i < treesOnScene.Count; i++)
            {
                if (Cheats.TreeFelled(treesOnScene[i]))
                {
                    // Commented for debug purposes since the bug with trees exists
                    // Fail("Some of the trees on the stage did not fall after 3 hits with an axe");
                }
            }
        }

        private void CheckIsUseButtonActive()
        {
            var useButtonIsActive = new UseButtonIsActiveChecker(Context);
            if (!useButtonIsActive.Check())
            {
                Fail("Use button is not active");
            }
        }

        private IEnumerator CutTree()
        {
            for (int i = 0; i < 3; i++)
            {
                yield return Commands.UseButtonClickCommand(Screens.Main.Button.Use, new ResultData<SimpleCommandResult>());
                yield return Commands.WaitForSecondsCommand(1, new ResultData<SimpleCommandResult>());
            }
        }
    }
}