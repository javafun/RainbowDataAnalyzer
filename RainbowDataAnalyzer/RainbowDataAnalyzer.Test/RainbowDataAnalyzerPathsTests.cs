﻿namespace RainbowDataAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using TestHelper;
    using RainbowDataAnalyzer;

    [TestClass]
    public class RainbowDataAnalyzerPathsTests : DiagnosticVerifier
    {
        [TestMethod]
        public void ShouldGetMessageAboutPath()
        {
            var test = "class TestClass { string scPath = \"/sitecore/nonexistent\"; }";
            var expected = new DiagnosticResult
            {
                Id = "RainbowDataAnalyzerPaths",
                Message = "An item with path '/sitecore/nonexistent' could not be found",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 35)
                        }
            };

            var yamlContents = new[] {
                "---\r\nID: \"0ec9e41a-0d47-47ec-a0ac-2819edb60311\"\r\nPath: /sitecore/existent"
            };

            this.VerifyCSharpDiagnostic(test, yamlContents, expected);
        }

        [TestMethod]
        public void ShouldNotGetMessageAboutPath()
        {
            var test = "class TestClass { string scPath = \"/sitecore/existent\"; }";

            var yamlContents = new string[] {
                "---\r\nID: \"0ec9e41a-0d47-47ec-a0ac-2819edb60311\"\r\nPath: /sitecore/existent"
            };
            
            var expected = new DiagnosticResult
                {
                    Id = "RainbowDataAnalyzerPathToId",
                    Message = "The path corresponds with ID '0ec9e41a-0d47-47ec-a0ac-2819edb60311'",
                    Severity = DiagnosticSeverity.Info,
                    Locations =
                            new[] {
                                    new DiagnosticResultLocation("Test0.cs", 11, 35)
                                }
                };

            this.VerifyCSharpDiagnostic(test, yamlContents, expected);
        }
        
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new RainbowDataAnalyzerAnalyzer();
        }
    }
}