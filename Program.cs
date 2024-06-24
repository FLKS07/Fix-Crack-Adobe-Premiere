using WindowsFirewallHelper;


class Program
{

    static void Main(string[] arg)
    {
        if (arg.Length == 0)
        {
            string? adobe_folder = Get_folders.Get_adobe_root_folder();
            string? app_folder = Get_folders.get_app_folder(adobe_folder);
            if (app_folder != null)
            {
                string? app_file_path = Get_folders.get_app(app_folder);
                if (add_new_rule(app_file_path))
                {
                    Console.WriteLine("Sucessful");
                }
            }
            else
            {
                Console.WriteLine("Not found the aplication :(");
                return;
            }
        }




    }

    public static bool add_new_rule(string path_to_use_file)
    {
        string filename = Path.GetFileName(path_to_use_file);
        try
        {
            var rule = FirewallManager.Instance.CreateApplicationRule(FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public,
                                    $"Automatic Block Adobe {filename}",
                                    FirewallAction.Block, path_to_use_file,
                                    FirewallProtocol.Any);
            rule.Direction = FirewallDirection.Outbound; //Outbound
            FirewallManager.Instance.Rules.Add(rule);


            var _rule = FirewallManager.Instance.CreateApplicationRule(FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public,
                $"Automatic: Block Adobe {filename}",
                FirewallAction.Block, path_to_use_file,
                FirewallProtocol.Any);

            _rule.Direction = FirewallDirection.Inbound; //Inbound
            FirewallManager.Instance.Rules.Add(_rule);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create the rule: {ex.Message}");
            return false;
        }
    }
}