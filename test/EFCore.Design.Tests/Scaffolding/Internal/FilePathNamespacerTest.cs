// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Xunit;

namespace Microsoft.EntityFrameworkCore
{
    public class FilePathNamespacerTest
    {
        [ConditionalFact]
        public void GetNamespaceFromOutputPath_replaces_spaces_with_underscores()
        {
            var rootDirectory = "rootDirectory";
            var mySpaceyDir = "rootDirectory/My Spacey Directory";
            var namespacer = new FilePathNamespacer();

            var result = namespacer.GetNamespaceFromOutputPath(rootDirectory, "MyNamespace", mySpaceyDir);

            Assert.DoesNotContain(mySpaceyDir, result);
            Assert.Contains("My_Spacey_Directory", result);
            Assert.Equal("MyNamespace.My_Spacey_Directory", result);
        }


        [ConditionalFact]
        public void GetNamespaceFromOutputPath_appends_outputPath_to_knownRootNamespace()
        {
            var rootDirectory = "rootDirectory";
            var rootNamespace = "MyNamespace";
            var outputDir = "anotherChildDirectory";
            var namespacer = new FilePathNamespacer();

            var result = namespacer.GetNamespaceFromOutputPath(rootDirectory, rootNamespace, $"{rootDirectory}/{outputDir}");

            Assert.Equal($"{rootNamespace}.{outputDir}", result);
        }

        [ConditionalFact]
        public void GetNamespaceFromOutputPath_returns_only_outputPath_when_knownRootNamespace_is_empty()
        {
            var rootDirectory = "rootDirectory";
            var rootNamespace = "";
            var outputDir = "ChildDirectory";
            var namespacer = new FilePathNamespacer();

            var result = namespacer.GetNamespaceFromOutputPath(rootDirectory, rootNamespace, $"{rootDirectory}/{outputDir}");

            Assert.Equal(outputDir, result);
        }

        [ConditionalFact]
        public void GetNamespaceFromOutputPath_throws_if_knownProjectDir_is_null()
        {
            var namespacer = new FilePathNamespacer();

            var ex = Assert.Throws<ArgumentNullException>(() => namespacer.GetNamespaceFromOutputPath(null, "", ""));

            Assert.Equal("knownProjectDir", ex.ParamName);
        }

        [ConditionalFact]
        public void GetNamespaceFromOutputPath_throws_if_outputPath_is_null()
        {
            var namespacer = new FilePathNamespacer();

            var ex = Assert.Throws<ArgumentNullException>(() => namespacer.GetNamespaceFromOutputPath("", "", null));

            Assert.Equal("outputPath", ex.ParamName);
        }

        [ConditionalFact]
        public void GetNamespaceFromOutputPath_returns_rootNamespace_when_outputDir_not_in_knownProjectDir_path()
        {
            var rootDirectory = "rootDirectory";
            var childDirectory = "anotherDirectory/anotherChildDirectory";
            var namespacer = new FilePathNamespacer();

            var result = namespacer.GetNamespaceFromOutputPath(rootDirectory, "MyNamespace", childDirectory);

            Assert.Equal("MyNamespace", result);
        }

        [ConditionalFact]
        public void GetNamespaceFromOutputPath_returns_knownRootNamespace_when_knownProjectDir_and_outPutDir_are_empty()
        {
            var knownNamespace = "MyNamespace";            
            var namespacer = new FilePathNamespacer();

            var result = namespacer.GetNamespaceFromOutputPath("", knownNamespace, "");

            Assert.Equal(knownNamespace, result);
        }

        [ConditionalFact]
        public void GetNamespaceFromOutputPath_returns_empty_when_all_arguments_empty()
        {
            var namespacer = new FilePathNamespacer();

            var result = namespacer.GetNamespaceFromOutputPath("", "", "");

            Assert.Empty(result);
        }



    }
}
