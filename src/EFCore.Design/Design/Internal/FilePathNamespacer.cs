// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Design.Internal
{
    internal class FilePathNamespacer : IFilePathNamespacer
    {
        public string GetNamespaceFromOutputPath(string knownProjectDir, string knownRootNamespace, string outputPath)
        {
            Check.NotNull(knownProjectDir, nameof(knownProjectDir));
            Check.NotNull(outputPath, nameof(outputPath));

            var subNamespace = SubnamespaceFromOutputPath(knownProjectDir, outputPath);

            return string.IsNullOrEmpty(subNamespace)
                ? knownRootNamespace
                : string.IsNullOrEmpty(knownRootNamespace)
                    ? subNamespace
                    : knownRootNamespace + "." + subNamespace;
        }

        private string? SubnamespaceFromOutputPath(string projectDir, string outputDir)
        {
            if (!outputDir.StartsWith(projectDir, StringComparison.Ordinal))
            {
                return null;
            }

            var subPath = outputDir.Substring(projectDir.Length);

            return !string.IsNullOrWhiteSpace(subPath)
                ? string.Join(
                    ".",
                    subPath.Split(
                        new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries))
                        .Replace(' ', '_')
                : null;
        }
    }
}
