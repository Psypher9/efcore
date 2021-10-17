// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Design.Internal
{
    internal class FilePathNamespacer : IFilePathNamespacer
    {
        /// <summary>
        /// <para>
        /// if outputPath is a subfolder of knownProjectDir, then use each
        /// subfolder as a subnamespace
        /// --output-dir $(knownProjectDir)/A/B/C
        /// => "namespace $(knownRootNamespace).A.B.C"
        /// </para>
        ///
        /// <para>
        /// If the outputPath contains spaces, those spaces (" ") are replaced
        /// with underscores ("_").
        /// </para>
        /// </summary>
        /// <param name="knownProjectDir">The target project's root directory</param>
        /// <param name="knownRootNamespace">The root namespace of the target project</param>
        /// <param name="outputDir">The path which may be part of the generated namespace</param>
        /// <returns>The generated namespace</returns>
        public string GetNamespaceFromOutputPath(string knownProjectDir, string knownRootNamespace, string outputDir)
        {
            Check.NotNull(knownProjectDir, nameof(knownProjectDir));
            Check.NotNull(outputDir, nameof(outputDir));

            var subNamespace = SubnamespaceFromOutputPath(knownProjectDir, outputDir);

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
