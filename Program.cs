using System.Security.AccessControl;
using WindowsFirewallHelper;
using WindowsFirewallHelper.FirewallRules;
using WindowsFirewallHelper.Exceptions;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.IO.Enumeration;




class Program
{
    public static bool fix = false;

    static void Main()
    {
        string default_folder_location = @"C:\Program Files\Adobe";
        string path_to_use = "";
        string path_to_use_file;
        Console.WriteLine("Adobe INSTALATION Folder DEFAULT Location: \n" + default_folder_location);
        var dir_ = Directory.GetDirectories(default_folder_location);


        bool found_folder = false;
        foreach (var dir in dir_)
        {
            Console.WriteLine($"\n Is this the correct folder?: \n {dir}");
            Console.Write("[y/n]");
            string folder_input = Console.ReadLine();
            folder_input.Trim().ToLower();
            if (folder_input == "y")
            {
                path_to_use = dir;
                Console.WriteLine(path_to_use);
                found_folder = true;
                break;
            }
        }
        if (!found_folder)
        {
            Console.WriteLine("\n Press ENTER to use DEFAULT or put the path of aplication bellow:");
            path_to_use = Console.ReadLine().Trim();
        }

        bool found_file = false;
        try
        {
            string[] files = Directory.GetFiles(path_to_use);
            foreach (string file in files)
            {
                string file_name = Path.GetFileName(file);
                file_name = file_name.Trim().ToLower();

                if (file_name.Contains("photoshop") || file_name.Contains("premiere") || file_name.Contains("illustrator"))
                {
                    Console.WriteLine($"Is this the right program? \n {path_to_use}\\{file.Trim()}");
                    Console.Write("[y/n]");
                    string _input = Console.ReadLine().ToLower();

                    if (_input == "y")
                    {
                        path_to_use_file = Path.Combine(path_to_use, file);
                        add_new_rule(path_to_use_file);
                        found_file = true;

                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            if (!found_file)
            {

                Console.WriteLine("\n Program nout found, please put the Full path of the aplicaiton here:");
                path_to_use_file = Console.ReadLine().Trim();
                add_new_rule(path_to_use_file);
            }

        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("\n The aplication is not openned with adminstrator permissions. \n This is needed to acess program folders");
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("\n Path is not found, are you sure it's the right one?");
        }

        if (fix == true)
        {
            Console.WriteLine("Fixed :) ! Adobe mad :(");
        }
        else
        {
            Console.WriteLine("Not fixed");
        }

    }

    public static void add_new_rule(string path_to_use_file)
    {
        string filename = Path.GetFileName(path_to_use_file);
        try
        {
            var rule = FirewallManager.Instance.CreateApplicationRule(FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public,
                                    $"Block Adobe {filename}",
                                    FirewallAction.Block, path_to_use_file,
                                    FirewallProtocol.Any);
            rule.Direction = FirewallDirection.Outbound; //Outbound
            FirewallManager.Instance.Rules.Add(rule);


            var _rule = FirewallManager.Instance.CreateApplicationRule(FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public,
                $"Block Adobe {filename}",
                FirewallAction.Block, path_to_use_file,
                FirewallProtocol.Any);

            _rule.Direction = FirewallDirection.Inbound; //Inbound
            FirewallManager.Instance.Rules.Add(_rule);

            fix = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create the rule: {ex.Message}");
            fix = false;
            return;
        }
    }
}