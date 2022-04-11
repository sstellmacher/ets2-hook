using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace Ets2Client.Telemetry.Data
{
    class SharedMemory
    {
        private const int MEMORY_MAP_SIZE = 16 * 1024;

        private MemoryMappedFile memoryMappedHandle;
        private MemoryMappedViewAccessor memoryMappedView;

        public bool Connected { get; private set; } = false;
        public Exception HookException { get; private set; } = null;

        public bool Connect(string mapName)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    memoryMappedHandle = MemoryMappedFile.OpenExisting(mapName, MemoryMappedFileRights.ReadWrite);
                    memoryMappedView = memoryMappedHandle.CreateViewAccessor(0, MEMORY_MAP_SIZE);
                    Connected = true;
                }
                else
                {
                    throw new InvalidOperationException("Only supported on windows.");
                }
            }
            catch (Exception e)
            {
                HookException = e;
                Connected = false;
            }

            return Connected;
        }

        public byte[] ReadRawData()
        {
            if (!Connected)
                throw new InvalidOperationException(nameof(SharedMemory) + " not connected.");

            var rawData = new byte[MEMORY_MAP_SIZE];

            memoryMappedView.ReadArray(0, rawData, 0, rawData.Length);

            return rawData;
        }

        public T ToObject<T>(byte[] structureDataBytes)
        {
            T createdObject = default(T);

            var memoryObjectSize = Marshal.SizeOf(typeof(T));

            if (memoryObjectSize > structureDataBytes.Length)
                return createdObject;

            var reservedMemPtr = Marshal.AllocHGlobal(memoryObjectSize);

            Marshal.Copy(structureDataBytes, 0, reservedMemPtr, memoryObjectSize);

            createdObject = (T)Marshal.PtrToStructure(reservedMemPtr, typeof(T));

            Marshal.FreeHGlobal(reservedMemPtr);

            return createdObject;
        }
    }
}
