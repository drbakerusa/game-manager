﻿using System.Diagnostics;
using System.Reflection;

namespace GameManager.Components;

public static class UIElements
{
    public static void Error(string message) => PrintMessageWithColor(ConsoleColor.Red, message);

    public static void Warning(string message) => YellowHighlight(message);
    public static void YellowHighlight(string message) => PrintMessageWithColor(ConsoleColor.Yellow, message);

    public static void Success(string message) => GreenHighlight(message);
    public static void GreenHighlight(string message) => PrintMessageWithColor(ConsoleColor.Green, message);

    public static void Blue(string message) => PrintMessageWithColor(ConsoleColor.Blue, message);

    public static void Normal(string message) => Console.WriteLine(message);

    public static void Blank() => Console.WriteLine();

    public static string ApplicationTitle => GenerateApplicationTitle();

    public static void Divider(bool fullwidth = true, bool includeSpace = false)
    {
        int width = 0;
        if ( fullwidth )
            width = Console.WindowWidth - 2;
        else
            width = 7;

        if ( includeSpace )
            Blank();

        Normal(("".PadLeft(width, '-')));

        if ( includeSpace )
            Blank();
    }

    public static int? IntegerInput(int? defaultValue, string prompt = "")
    {
        var inputIsInteger = false;

        while ( !inputIsInteger )
        {
            var defaultString = defaultValue is null ? string.Empty : $" ({defaultValue})";

            var message = string.Empty;

            if ( string.IsNullOrEmpty(prompt) )
                message = $"Enter value{defaultString}";
            else
                message = $"{prompt}{defaultString}";

            var input = TextInput(message);

            if ( string.IsNullOrEmpty(input) )
                break;
            else
            {
                if ( int.TryParse(input, out int value) )
                {
                    inputIsInteger = true;
                    return value;
                }
            }
        }

        return defaultValue;
    }

    public static void PageTitle(string title)
    {
        Blue(title);
        Underline(title);
        Blank();
    }

    public static int Menu(List<string> options, bool showDivider = false)
    {
        if ( showDivider )
            Divider(fullwidth: false, includeSpace: true);

        foreach ( var option in options )
        {
            Normal(MenuOption(index: (options.IndexOf(option) + 1), optionText: option));
        }

        var selection = SelectionPrompt("Make a selection", numberOfOptions: options.Count);

        return selection is null ? -1 : (int) selection - 1;
    }

    public static int? PagedMenu(List<string> options, string listTitle, string promptCancelMessage, bool showDivider = false)
    {
        if ( showDivider )
            Divider(fullwidth: false, includeSpace: true);

        var pageSize = new SettingsService().DefaultPageSize;
        var selectionMade = false;
        var selectedItem = string.Empty;
        var currentPage = 1;
        var totalPages = CalculateTotalPages(options.Count) == 0 ? 1 : CalculateTotalPages(options.Count);

        while ( !selectionMade )
        {
            Console.Clear();

            var optionsPage = new List<string>();
            var message = $"Page {currentPage} of {totalPages}";
            PageTitle(listTitle);

            if ( options.Any() )
            {
                Normal(message);
                Underline(message);
                Blank();

                optionsPage = options.Skip((currentPage - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();
                if ( currentPage != 1 )
                    optionsPage.Add("Previous Page");
                if ( currentPage != totalPages )
                    optionsPage.Add("Next Page");

                foreach ( var option in optionsPage )
                {
                    Normal(MenuOption(index: (optionsPage.IndexOf(option) + 1), optionText: option));
                }

                var selection = SelectionPrompt($"Make a selection ({promptCancelMessage})", numberOfOptions: options.Count);

                if ( selection == null )
                    return null;

                selectedItem = optionsPage[(int) selection - 1];

                if ( selectedItem == "Previous Page" )
                    currentPage--;
                else if ( selectedItem == "Next Page" )
                    currentPage++;
                else
                    selectionMade = true;
            }
            else
            {
                Warning("None found");
                Divider(fullwidth: false, includeSpace: true);
                TextInput("Enter to continue");
                return null;
            }
        }

        return options.IndexOf(selectedItem);
    }

    public static string TextInput(string prompt)
    {
        Normal(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    public static bool Confirm(string message, bool defaultResponse = true)
    {
        Divider(fullwidth: false, includeSpace: true);

        var choices = defaultResponse ? "(Y/n)" : "(y/N)";
        Normal($"{message}? {choices}");
        var response = Console.ReadLine()?.ToLower();

        if ( defaultResponse == true )
        {
            if ( string.IsNullOrEmpty(response) )
                return true;
            else
                return response == "y";
        }
        else
        {
            if ( string.IsNullOrEmpty(response) )
                return false;
            else
                return response == "y";
        }
    }

    public static string ConvertBoolToYesNo(bool value) => value ? "yes" : "no";

    public static string MenuOption(int index, string optionText) => $"{index,4}.     {optionText}";

    public static (string Username, string Password) GetCredentials(string? existingUsername)
    {
        var usernamePrompt = string.IsNullOrEmpty(existingUsername) ? string.Empty : $" (blank for {existingUsername})";
        var input = TextInput($"Enter username{usernamePrompt}");
        var password = SecretInput("Enter password");

        Console.WriteLine();

        if ( string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(existingUsername) )
            return (existingUsername, password);
        else
            return (input, password);
    }

    public static string SecretInput(string promptMessage)
    {
        Normal(promptMessage);
        var secret = string.Empty;
        ConsoleKeyInfo keyInfo;

        do
        {
            keyInfo = Console.ReadKey(intercept: true);

            if ( keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter )
            {
                secret += keyInfo.KeyChar;
                Console.Write("*");
            }
            else
            {
                if ( keyInfo.Key == ConsoleKey.Backspace && secret.Length > 0 )
                {
                    secret = secret.Substring(0, secret.Length - 1);
                    Console.Write("b b");
                }
            }
        } while ( keyInfo.Key != ConsoleKey.Enter );

        return secret;
    }

    public static void ShowSuccessMessage(string message, int timeout = 2)
    {
        UIElements.Success(message);
        Pause(timeout);
    }

    public static void ShowErrorMessage(string message, int timeout = 2)
    {
        UIElements.Error(message);
        Pause(timeout);
    }

    public static void Pause(int sec = 2)
    {
        Console.WriteLine();
        var pauseProc = Process.Start(
            new ProcessStartInfo()
            {
                FileName = "cmd",
                Arguments = "/C TIMEOUT /t " + sec + " /nobreak > NUL",
                UseShellExecute = false
            });
        pauseProc?.WaitForExit();
    }

    private static int? SelectionPrompt(string message, int numberOfOptions)
    {
        var selectionMade = false;
        int selection = int.MinValue;

        while ( !selectionMade )
        {
            Divider(fullwidth: false, includeSpace: true);

            Console.WriteLine(message);
            var input = Console.ReadLine();

            if ( string.IsNullOrEmpty(input) )
                return null;

            if ( int.TryParse(input, out selection) )
            {
                if ( selection > 0 && selection <= numberOfOptions )
                    selectionMade = true;
            }
        }

        return selection;
    }

    private static void PrintMessageWithColor(ConsoleColor color, string message)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void PageHeader()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        var title = GenerateApplicationTitle();

        using ( var settings = new SettingsService() )
            if ( settings.NewerVersionExists )
                title += $" [{settings.LatestApplicationVersion} is available!]";

        Blue(title);
        Divider();
    }

    private static void Underline(string message) => Console.WriteLine("".PadRight(message.Length, '-'));

    private static int CalculateTotalPages(int numberOfItems)
        => (int) Math.Ceiling((decimal) numberOfItems / (decimal) new SettingsService().DefaultPageSize);

    private static string GenerateApplicationTitle()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        var versionString = string.Empty;

        if ( version != null )
            versionString = $"v{version.Major}.{version.Minor}.{version.Build}";

        return $"Game Manager {versionString}";
    }
}
