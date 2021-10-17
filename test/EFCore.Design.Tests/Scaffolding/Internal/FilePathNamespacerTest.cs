// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void GetNamespaceFromOutputPath_can_run_with_null_knownRootNamespace()
        {
            Assert.True(false);
        }

        [ConditionalFact]
        public void GetNamespaceFromOutputPath_returns_null_when_namespace_and_outputDir_are_null()
        {
            var rootDirectory = "rootDirectory";            
            var namespacer = new FilePathNamespacer();

            var result = namespacer.GetNamespaceFromOutputPath(rootDirectory, null, null);

            Assert.Null(result);
        }

        [ConditionalFact]
        public void GetNamespaceFromOutputPath_returns_rootNamespace_when_outputDir_not_in_knownProjectDir_path()
        {
            var rootDirectory = "rootDirectory";
            var mySpaceyDir = "anotherDirectory/anotherChildDirectory";
            var namespacer = new FilePathNamespacer();

            var result = namespacer.GetNamespaceFromOutputPath(rootDirectory, "MyNamespace", mySpaceyDir);

            Assert.Equal("MyNamespace", result);
        }
    }
}
