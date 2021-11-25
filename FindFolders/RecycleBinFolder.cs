using System;
using System.Runtime.InteropServices;

namespace FilesAndFolders
{
    public static class RecycleBinFolder
    {
        public static Message Info;

        enum RecycleFlags : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000002,
            SHERB_NOSOUND = 0x00000004
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

        public static void Delete()
        {
            try
            {
                uint result = SHEmptyRecycleBin(IntPtr.Zero, null, 0);
                Info?.Invoke("SUCCESS", "Корзина очищена.");                
            }
            catch (Exception)
            {
                Info?.Invoke("ERROR", "Не удалось очистить корзину.");
            }
        }
    }
}
