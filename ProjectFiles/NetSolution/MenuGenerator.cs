#region Using directives
using UAManagedCore;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.Core;
using System.Collections.Generic;
using System.Linq;
#endregion

public class MenuGenerator : BaseNetLogic
{
    public override void Start()
    {
        try
        {
            var screens = GetNodesIntoFolder<PanelType>("UI/Screens/Instances").ToList();
            Log.Info("Number of screens: " + screens.Count);
            RowLayout row = null;
            for (int i = 0; i < screens.Count; i++)
            {
                var screen = screens[i];
                if (i % 10 == 0)
                {
                    row = InformationModel.MakeObject<RowLayout>("row_" + i / 10);
                    row.HorizontalAlignment = HorizontalAlignment.Stretch;
                    row.BottomMargin = 5;
                    Owner.Add(row);
                }

                var menuButton = InformationModel.MakeObject<MenuButton>("menuButton_" + i);
                menuButton.Text = screen.BrowseName;
                menuButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                menuButton.RightMargin = 5;
                menuButton.GetVariable("Screen").Value = screen.NodeId;

                row.Add(menuButton);
            }
        }
        catch (System.Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    public static IEnumerable<T> GetNodesIntoFolder<T>(string rootFolder)
    {
        var objectsInFolder = new List<T>();
        foreach (var o in Project.Current.GetObject(rootFolder).Children)
        {
            switch (o)
            {
                case T _:
                    objectsInFolder.Add((T)o);
                    break;
                case Folder _:
                case UAObject _:
                    objectsInFolder.AddRange(GetNodesIntoFolder<T>(rootFolder + "/" + o.BrowseName));
                    break;
                default:
                    break;
            }
        }
        return objectsInFolder;
    }
}
