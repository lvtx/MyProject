﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ProcessLauncher
{
	static class QuickLook
	{
        public static void ToggleQuickLook(string path)
        {
            string PipeName = "QuickLook.App.Pipe." + WindowsIdentity.GetCurrent().User?.Value;
            string Toggle = "QuickLook.App.PipeMessages.Toggle";

            using (var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out))
            {
                client.Connect();

                using (var writer = new StreamWriter(client))
                {
                    writer.WriteLine($"{Toggle}|{path}");
                    writer.Flush();
                }
            }
        }

        public static void CheckQuickLookAvailability(ApplicationDataContainer localSettings)
        {
            static int QuickLookServerAvailable()
            {
                string PipeName = "QuickLook.App.Pipe." + WindowsIdentity.GetCurrent().User?.Value;
                string Switch = "QuickLook.App.PipeMessages.Switch";

                using var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out);
                try
                {
                    client.Connect(500);
                    var serverInstances = client.NumberOfServerInstances;

                    using (var writer = new StreamWriter(client))
                    {
                        writer.WriteLine($"{Switch}|");
                        writer.Flush();
                    }

                    return serverInstances;

                }
                catch (TimeoutException e)
                {
                    client.Close();
                    return 0;
                }
            }

            localSettings.Values["quicklook_enabled"] = QuickLookServerAvailable() != 0;
        }  
    }
}
