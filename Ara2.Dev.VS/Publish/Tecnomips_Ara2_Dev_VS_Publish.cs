namespace Ara2_Dev_VS.Publish
{
    using System;
    
    /// <summary>
    /// Helper class that exposes all GUIDs used across VS Package.
    /// </summary>
    internal sealed partial class PackageGuids
    {
        public const string guidAra2_Dev_VS.PublishPkgString = "e78ab954-07ee-4893-b009-7650a1742079";
        public const string guidAra2_Dev_VS.PublishCmdSetString = "ff544673-6fda-4ec3-9399-8f6bea7d33b5";
        public const string guidImagesString = "2c056548-6285-4f9f-b31d-0b211ba4b6ea";
        public static Guid guidAra2_Dev_VS.PublishPkg = new Guid(guidAra2_Dev_VS.PublishPkgString);
        public static Guid guidAra2_Dev_VS.PublishCmdSet = new Guid(guidAra2_Dev_VS.PublishCmdSetString);
        public static Guid guidImages = new Guid(guidImagesString);
    }
    /// <summary>
    /// Helper class that encapsulates all CommandIDs uses across VS Package.
    /// </summary>
    internal sealed partial class PackageIds
    {
        public const int MyMenuGroup = 0x1020;
        public const int cmdidMyCommand = 0x0100;
        public const int bmpPic1 = 0x0001;
        public const int bmpPic2 = 0x0002;
        public const int bmpPicSearch = 0x0003;
        public const int bmpPicX = 0x0004;
        public const int bmpPicArrows = 0x0005;
        public const int bmpPicStrikethrough = 0x0006;
    }
}
