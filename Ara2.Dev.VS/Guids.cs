// Guids.cs
// MUST match guids.h
using System;

namespace Tecnomips.Ara2_Dev_VS
{
    static class GuidList
    {
        public const string guidAra2_Dev_VSPkgString = "83b01fcc-93ae-45ec-8ff9-8c060b8b3f5c";
        public const string guidAra2_Dev_VSCmdSetString = "115fbd16-b0be-406b-880d-9a506a17a704";
        public const string guidAra2_Dev_VSEditorFactoryString = "9d183b57-5141-4694-8e13-0ebc91f1ffc5";

        public static readonly Guid guidAra2_Dev_VSCmdSet = new Guid(guidAra2_Dev_VSCmdSetString);
        public static readonly Guid guidAra2_Dev_VSEditorFactory = new Guid(guidAra2_Dev_VSEditorFactoryString);
    };
}