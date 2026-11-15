
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new JarvisWindow());
    }
}

public class JarvisWindow : Form
{
    TextBox input = new();
    TextBox chat = new();
    string memoryFile = Path.Combine(
        Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData),
        "JARVIS ZERO", "memory.txt");

    public JarvisWindow()
    {
        Text = "JARVIS ZERO";
        Size = new Size(850, 600);
        MinimumSize = new Size(650, 450);
        BackColor = Color.FromArgb(10, 8, 16);
        ForeColor = Color.White;
        StartPosition = FormStartPosition.CenterScreen;

        Label title = new()
        {
            Text = "J.A.R.V.I.S.",
            ForeColor = Color.FromArgb(190, 120, 255),
            Font = new Font("Segoe UI", 25, FontStyle.Bold),
            Dock = DockStyle.Top,
            Height = 65,
            TextAlign = ContentAlignment.MiddleCenter
        };

        chat.Multiline = true;
        chat.ReadOnly = true;
        chat.Dock = DockStyle.Fill;
        chat.BackColor = Color.FromArgb(19, 14, 27);
        chat.ForeColor = Color.FromArgb(225, 210, 245);
        chat.Font = new Font("Consolas", 11);
        chat.BorderStyle = BorderStyle.None;
        chat.ScrollBars = ScrollBars.Vertical;

        Panel bottom = new()
        {
            Dock = DockStyle.Bottom,
            Height = 65,
            BackColor = Color.FromArgb(15, 10, 22),
            Padding = new Padding(10)
        };

        input.Dock = DockStyle.Fill;
        input.BackColor = Color.FromArgb(35, 23, 48);
        input.ForeColor = Color.White;
        input.Font = new Font("Segoe UI", 12);
        input.BorderStyle = BorderStyle.FixedSingle;

        Button send = new()
        {
            Text = "SEND",
            Dock = DockStyle.Right,
            Width = 100,
            BackColor = Color.FromArgb(110, 45, 170),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };

        send.Click += (_, _) => Respond();
        input.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Respond();
            }
        };

        bottom.Controls.Add(input);
        bottom.Controls.Add(send);

        Controls.Add(chat);
        Controls.Add(bottom);
        Controls.Add(title);

        Say("JARVIS", "Good day, Evan. Systems initialized.");
        Say("JARVIS", "At your service, Boss. Type HELP to see commands.");
    }

    void Say(string speaker, string message)
    {
        chat.AppendText($"{speaker}: {message}\r\n\r\n");
    }

    void Respond()
    {
        string command = input.Text.Trim();
        if (command.Length == 0) return;

        input.Clear();
        Say("EVAN", command);

        if (command.Equals("help", StringComparison.OrdinalIgnoreCase))
        {
            Say("JARVIS", "Commands: HELP, TIME, REMEMBER followed by a note, MEMORY.");
        }
        else if (command.Equals("time", StringComparison.OrdinalIgnoreCase))
        {
            Say("JARVIS", DateTime.Now.ToString("F"));
        }
        else if (command.StartsWith("remember ", StringComparison.OrdinalIgnoreCase))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(memoryFile)!);
            File.AppendAllText(memoryFile,
                command.Substring(9) + Environment.NewLine);
            Say("JARVIS", "I have saved that note, Evan.");
        }
        else if (command.Equals("memory", StringComparison.OrdinalIgnoreCase))
        {
            string notes = File.Exists(memoryFile)
                ? File.ReadAllText(memoryFile)
                : "Your memory is currently empty.";

            Say("JARVIS", notes);
        }
        else
        {
            Say("JARVIS", "I'm listening, Evan. That feature isn't connected yet.");
        }
    }
}
