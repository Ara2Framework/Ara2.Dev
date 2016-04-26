// Guids.cs
// MUST match guids.h
using System;

namespace Ara2_Dev_VS.Publish
{
    static class GuidList
    {
        public const string guidAra2_Dev_VS_PublishPkgString = "e78ab954-07ee-4893-b009-7650a1742079";
        public const string guidAra2_Dev_VS_PublishCmdSetString = "ff544673-6fda-4ec3-9399-8f6bea7d33b5";

        public static readonly Guid guidAra2_Dev_VS_PublishCmdSet = new Guid(guidAra2_Dev_VS_PublishCmdSetString);
    };
}