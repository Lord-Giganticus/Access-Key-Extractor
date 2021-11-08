using Eto.Drawing;
using Eto.Forms;
using Access_Key_Extractor.Library;
using System;
using System.IO;
using System.Linq;

namespace Access_Key_Extractor.UI
{
    partial class MainForm : Form
    {
        void InitializeComponent()
        {
            Title = "Access Key Extractor UI";
            MinimumSize = new Size(400, 400);
            Padding = 10;
            Location = new Point((int)(Screen.WorkingArea.Width / 3), (int)(Screen.WorkingArea.Height / 4));

            Content = new TextBox { ReadOnly = true, Text = string.Empty, Size = Size };

            // create a few commands that can be used for the menu and toolbar
            var clickMe = new Command { MenuText = "Open a elf or bin file"};
            clickMe.Executed += (sender, e) =>
            {
                var ofd = new OpenFileDialog
                {
                    CheckFileExists = true,
                    CurrentFilterIndex = 0,
                    Filters = {
                        new FileFilter("Accepted files (*.elf, *.bin)", new string[]{ ".elf", ".bin" }),
                        new FileFilter("Elf file (*.elf)", new string[]{ ".elf" }),
                        new FileFilter("Bin file (*.bin)", new string[]{ ".bin"}),
                    },
                    Title = "Open a binary file.",
                    Directory = new Uri(Directory.GetCurrentDirectory()),
                    MultiSelect = false
                };
                if (ofd.ShowDialog(this) is DialogResult.Ok)
                {
                    RomFile file = new(ofd.FileName);
                    var keys = file.GetKeys();
                    string data = $"[ '{string.Join("', '", keys.Take(keys.Length - 2))}', '{keys.Last()}' ]";
                    ((TextBox)Content).Text = $"Possible access keys (the correct key is usually one of the first)\n{data}";
                }
            };

            var saveCommand = new Command { MenuText = "Save the keys", Shortcut = Application.Instance.CommonModifier | Keys.S };
            saveCommand.Executed += SaveCommand_Executed;

            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();

            var aboutCommand = new Command { MenuText = "About..." };
            aboutCommand.Executed += (sender, e) => new AboutDialog(GetType().Assembly).ShowDialog(this);

            // create menu
            Menu = new MenuBar
            {
                Items =
                {
					// File submenu
					new SubMenuItem { Text = "&File", Items = { clickMe, saveCommand } },
					// new SubMenuItem { Text = "&View", Items = { /* commands/items */ } },
				},
                QuitItem = quitCommand,
                AboutItem = aboutCommand
            };
        }

        private void SaveCommand_Executed(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((TextBox)Content).Text)) {
                MessageBox.Show(this, "The keys are empty! Try loading a file.");
                return;
            }
            var sfd = new SaveFileDialog
            {
                Title = "Save the keys.",
                Directory = new Uri(Directory.GetCurrentDirectory()),
                FileName = "Keys",
                CurrentFilterIndex = 0,
                Filters =
                {
                    new FileFilter("Text file (.txt)", new string[]{ ".txt" }),
                    new FileFilter("All files (*.*)", new string[]{ "*.*" })
                }
            };
            if (sfd.ShowDialog(this) is DialogResult.Ok)
            {
                var data = ((TextBox)Content).Text;
#pragma warning disable IDE0057 // Use range operator
                data = data.Substring(data.IndexOf("\n") + 1);
#pragma warning restore IDE0057 // Use range operator
                File.WriteAllText(sfd.FileName, data);
            }
        }
    }
}
