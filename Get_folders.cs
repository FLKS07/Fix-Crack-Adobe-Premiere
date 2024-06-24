class Get_folders
{
    static string default_program_files_folder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
    static string[] app_names = { "photoshop", "illustrator", "premiere", "effects" };


    public static string? Get_adobe_root_folder()
    {
        string? adobe_folder = Get_adobe_folder(default_program_files_folder);
        Console.WriteLine($"Is your aplication contained on the default installation of Adobe Porgram files? \n Default Adobe Folder: {adobe_folder}\n");
        Console.WriteLine("[y/n]");
        string? input = Console.ReadLine();
        input.Trim().ToLower();
        if (input == "y")
        {
            return adobe_folder;
        }
        return null;
    }

    public static string get_app_folder(string adobe_root_folder)
    {
        string[] app_folders = Directory.GetDirectories(adobe_root_folder);
        int i = 0;
        Console.WriteLine("What is the correct folder?");
        foreach (string app_folder in app_folders)
        {
            Console.WriteLine($"{app_folder}, {i} ");
            i += 1;
        }

        int folder;

        while (true)
        {
            string? folder_choice = Console.ReadLine();
            try
            {
                folder = int.Parse(folder_choice);
                break;
            }
            catch
            {
                Console.WriteLine("That input ins not correct, try again:");
            }
        }
        return app_folders[folder];
    }

    public static string? get_app(string app_folder)
    {
        string[] file_names = Directory.GetFiles(app_folder);
        foreach (string app_name in app_names)
        {
            foreach (string file_name in file_names)
            {
                string lower_downcase_filename = Path.GetFileName(file_name);
                lower_downcase_filename = lower_downcase_filename.ToLower();

                if (lower_downcase_filename.Contains(app_name))
                {
                    Ask_file(file_name);
                    return Path.Combine(app_folder, file_name);
                }
            }
        }
        return null;

    }

    public static bool Ask_file(string file)
    {

        Console.WriteLine($"Is this the correct file? {file}");
        Console.Write("[y/n]");
        string input = Console.ReadLine();
        if (input == "y")
        {
            return true;
        }
        return false;
    }


    public static string? Get_adobe_folder(string program_files_folder)
    {
        string[] program_folders = Directory.GetDirectories(program_files_folder);
        foreach (string program_folder in program_folders)
        {
            if (program_folder.Contains("Adobe"))
            {
                return program_folder;
            }

        }
        return null;
    }

}