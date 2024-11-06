using System.Collections.Generic;

public class PaintData
{
    private Dictionary<string, int> _priorityData = new();

    private static PaintData _instance;
    public static PaintData Instance
    {
        get
        {
            if (_instance == null) _instance = new();
            return _instance;
        }
    }


    public PaintData()
    {
        #region Pillars

        _priorityData.Add("Pillars", 0);
        _priorityData.Add("Pillars (1)", 1);
        _priorityData.Add("Pillars (2)", 2);
        _priorityData.Add("Pillars (3)", 3);
        _priorityData.Add("Pillars (4)", 4);
        _priorityData.Add("Pillars (5)", 0);
        _priorityData.Add("Pillars (6)", 1);
        _priorityData.Add("Pillars (7)", 2);
        _priorityData.Add("Pillars (8)", 3);
        _priorityData.Add("Pillars (9)", 4);

        #endregion

        #region Roofs
        _priorityData.Add("Roof", 0);
        _priorityData.Add("Roof (1)", 1);
        _priorityData.Add("Roof (2)", 2);
        _priorityData.Add("Roof (3)", 3);
        _priorityData.Add("Roof (4)", 4);
        _priorityData.Add("Roof (5)", 5);
        _priorityData.Add("Roof (6)", 6);
        _priorityData.Add("Roof (7)", 7);
        _priorityData.Add("Roof (8)", 8);
        _priorityData.Add("Roof (9)", 9);
        _priorityData.Add("Roof (10)", 10);
        _priorityData.Add("Roof (11)", 11);
        _priorityData.Add("Roof (12)", 12);
        _priorityData.Add("Roof (13)", 13);
        _priorityData.Add("Roof (14)", 14);
        _priorityData.Add("Roof (15)", 15);
        _priorityData.Add("Roof (16)", 16);
        _priorityData.Add("Roof (17)", 17);
        _priorityData.Add("Roof (18)", 18);
        _priorityData.Add("Roof (19)", 19);
        _priorityData.Add("Roof (20)", 20);
        _priorityData.Add("Roof (21)", 21);
        _priorityData.Add("Roof (22)", 22);
        _priorityData.Add("Roof (23)", 23);
        _priorityData.Add("Roof (24)", 24);
        _priorityData.Add("Roof (25)", 25);
        _priorityData.Add("Roof (26)", 26);
        _priorityData.Add("Roof (27)", 27);
        _priorityData.Add("Roof (28)", 28);
        _priorityData.Add("Roof (29)", 29);
        #endregion

        #region Walls
        _priorityData.Add("Wall", 0);
        _priorityData.Add("Wall (1)", 1);
        _priorityData.Add("Wall (2)", 2);
        _priorityData.Add("Wall (3)", 3);
        _priorityData.Add("Wall (4)", 4);
        _priorityData.Add("Wall (5)", 5);
        _priorityData.Add("Wall (6)", 6);
        _priorityData.Add("Wall (7)", 7);
        _priorityData.Add("Wall (8)", 8);
        _priorityData.Add("Wall (9)", 9);
        _priorityData.Add("Wall (10)", 10);
        _priorityData.Add("Wall (11)", 11);
        _priorityData.Add("Wall (12)", 12);
        _priorityData.Add("Wall (13)", 13);
        _priorityData.Add("Wall (14)", 14);
        _priorityData.Add("Wall (15)", 15);
        _priorityData.Add("Wall (16)", 16);
        _priorityData.Add("Wall (17)", 17);
        _priorityData.Add("Wall (18)", 18);
        _priorityData.Add("Wall (19)", 19);
        _priorityData.Add("Wall (20)", 20);
        _priorityData.Add("Wall (21)", 21);
        _priorityData.Add("Wall (22)", 22);
        _priorityData.Add("Wall (23)", 23);
        _priorityData.Add("Wall (24)", 24);
        _priorityData.Add("Wall (25)", 25);
        _priorityData.Add("Wall (26)", 26);
        _priorityData.Add("Wall (27)", 27);
        _priorityData.Add("Wall (28)", 28);
        _priorityData.Add("Wall (29)", 29);
        _priorityData.Add("Wall2", 0);
        _priorityData.Add("Wall2 (1)", 1);
        _priorityData.Add("Wall2 (2)", 2);
        _priorityData.Add("Wall2 (3)", 3);
        _priorityData.Add("Wall2 (4)", 4);
        _priorityData.Add("Wall2 (5)", 5);
        _priorityData.Add("Wall2 (6)", 6);
        _priorityData.Add("Wall2 (7)", 7);
        _priorityData.Add("Wall2 (8)", 8);
        _priorityData.Add("Wall2 (9)", 9);
        _priorityData.Add("Wall2 (10)", 10);
        _priorityData.Add("Wall2 (11)", 11);
        _priorityData.Add("Wall2 (12)", 12);
        _priorityData.Add("Wall2 (13)", 13);
        _priorityData.Add("Wall2 (14)", 14);
        _priorityData.Add("Wall2 (15)", 15);
        _priorityData.Add("Wall2 (16)", 16);
        _priorityData.Add("Wall2 (17)", 17);
        _priorityData.Add("Wall2 (18)", 18);
        _priorityData.Add("Wall2 (19)", 19);
        _priorityData.Add("Wall2 (20)", 20);
        _priorityData.Add("Wall2 (21)", 21);
        _priorityData.Add("Wall2 (22)", 22);
        _priorityData.Add("Wall2 (23)", 23);
        _priorityData.Add("Wall2 (24)", 24);
        _priorityData.Add("Wall2 (25)", 25);
        _priorityData.Add("Wall2 (26)", 26);
        _priorityData.Add("Wall2 (27)", 27);
        _priorityData.Add("Wall2 (28)", 28);
        _priorityData.Add("Wall2 (29)", 29);


        #endregion

        #region Crates

        #endregion

        //CwHitScreenBase.OnHitUpdate += OnHitUpdate;
    }

    public int GetPriority(string key)
    {
        if (_priorityData.ContainsKey(key))
        {
            return _priorityData[key];
        }
        return 0;
    }
}