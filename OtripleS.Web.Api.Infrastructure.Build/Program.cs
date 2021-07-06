// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using ADotNet.Clients;
using ADotNet.Models.Pipelines.AspNets;
using ADotNet.Models.Pipelines.AspNets.Tasks.DotNetExecutionTasks;
using ADotNet.Models.Pipelines.AspNets.Tasks.UseDotNetTasks;

namespace OtripleS.Web.Api.Infrastructure.Build
{
    class Program
    {
        static void Main(string[] args)
        {
            var adotNetClient = new ADotNetClient();

            var buildPipeline = new AspNetPipeline
            {
                TriggeringBranches = new List<string>
                {
                    "master"
                },

                VirtualMachinesPool = new VirtualMachinesPool
                {
                    VirutalMachineImage = VirtualMachineImages.UbuntuLatest
                },

                ConfigurationVariables = new ConfigurationVariables
                {
                    BuildConfiguration = BuildConfiguration.Release
                },

                Tasks = new List<BuildTask>
                {
                    new UseDotNetTask
                    {
                        DisplayName = "Use .NET 6.0 Preview",

                        Inputs = new UseDotNetTasksInputs
                        {
                            Version = "6.0.100-preview.5.21302.13",
                            IncludePreviewVersions = true
                        }
                    },

                    new DotNetExecutionTask
                    {
                        DisplayName = "Restore",

                        Inputs = new DotNetExecutionTasksInputs
                        {
                            Command = Command.restore,
                            FeedsToUse = Feeds.select
                        }
                    },

                    new DotNetExecutionTask
                    {
                        DisplayName = "Build",

                        Inputs = new DotNetExecutionTasksInputs
                        {
                            Command = Command.build
                        }
                    },

                    new DotNetExecutionTask
                    {
                        DisplayName = "Test",

                        Inputs = new DotNetExecutionTasksInputs
                        {
                            Command = Command.test,
                            Projects = "**/*Unit*.csproj"
                        }
                    },

                    new DotNetExecutionTask
                    {
                        DisplayName = "Publish",

                        Inputs = new DotNetExecutionTasksInputs
                        {
                            Command = Command.publish,
                            PublishWebProjects = true
                        }
                    }
                }
            };

            adotNetClient.SerializeAndWriteToFile(buildPipeline, "../../../../azure-pipelines.yml");
        }
    }
}
