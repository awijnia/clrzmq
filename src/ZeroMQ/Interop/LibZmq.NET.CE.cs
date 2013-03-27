#if !MONO

namespace ZeroMQ.Interop
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Runtime.InteropServices;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Compatibility with native headers.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Compatibility with native headers.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Compatibility with native headers.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Compatibility with native headers.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Reviewed. Suppression is OK here.")]
    internal static class LibZmq
    {

        // From zmq.h (v3):
        // typedef struct {unsigned char _ [32];} zmq_msg_t;
        private static readonly int Zmq3MsgTSize = 32 * Marshal.SizeOf(typeof(byte));

        // From zmq.h (v2):
        // #define ZMQ_MAX_VSM_SIZE 30
        //
        // typedef struct
        // {
        //     void *content;
        //     unsigned char flags;
        //     unsigned char vsm_size;
        //     unsigned char vsm_data [ZMQ_MAX_VSM_SIZE];
        // } zmq_msg_t;
        private static readonly int ZmqMaxVsmSize = 30 * Marshal.SizeOf(typeof(byte));
        private static readonly int Zmq2MsgTSize = IntPtr.Size + (Marshal.SizeOf(typeof(byte)) * 2) + ZmqMaxVsmSize;

        public static readonly int ZmqMsgTSize;

        public static readonly int MajorVersion;
        public static readonly int MinorVersion;
        public static readonly int PatchVersion;

        public static readonly long PollTimeoutRatio;

        static LibZmq()
        {
            AssignCurrentVersion(out MajorVersion, out MinorVersion, out PatchVersion);

            PollTimeoutRatio = 1;
            ZmqMsgTSize = Zmq3MsgTSize;
        }


        private static void AssignCurrentVersion(out int majorVersion, out int minorVersion, out int patchVersion)
        {
            int sizeofInt32 = Marshal.SizeOf(typeof(int));

            IntPtr majorPointer = Marshal.AllocHGlobal(sizeofInt32);
            IntPtr minorPointer = Marshal.AllocHGlobal(sizeofInt32);
            IntPtr patchPointer = Marshal.AllocHGlobal(sizeofInt32);

            zmq_version(majorPointer, minorPointer, patchPointer);

            majorVersion = Marshal.ReadInt32(majorPointer);
            minorVersion = Marshal.ReadInt32(minorPointer);
            patchVersion = Marshal.ReadInt32(patchPointer);

            Marshal.FreeHGlobal(majorPointer);
            Marshal.FreeHGlobal(minorPointer);
            Marshal.FreeHGlobal(patchPointer);
        }

        [DllImport("libzmq", CharSet = CharSet.Unicode)]
        public static extern Int32 zmq_bind(IntPtr socket, Byte[] addr);
        public static Int32 zmq_bind(IntPtr socket, String addr) {
            return zmq_bind(socket, Encoding.ASCII.GetBytes(addr));
        }

        [DllImport("libzmq", CharSet = CharSet.Unicode)]
        public static extern Int32 zmq_unbind(IntPtr socket, Byte[] addr);
        public static Int32 zmq_unbind(IntPtr socket, String addr) {
            return zmq_unbind(socket, Encoding.ASCII.GetBytes(addr));
        }

        [DllImport("libzmq", CharSet = CharSet.Unicode)]
        public static extern Int32 zmq_connect(IntPtr socket, Byte[] addr);
        public static Int32 zmq_connect(IntPtr socket, String addr) {
            return zmq_connect(socket, Encoding.ASCII.GetBytes(addr));
        }

        [DllImport("libzmq", CharSet = CharSet.Unicode)]
        public static extern Int32 zmq_disconnect(IntPtr socket, Byte[] addr);
        public static Int32 zmq_disconnect(IntPtr socket, String addr) {
            return zmq_disconnect(socket, Encoding.ASCII.GetBytes(addr));
        }

        [DllImport("libzmq", CharSet = CharSet.Unicode)]
        unsafe public static extern Int32 zmq_socket_monitor(IntPtr socket, Byte[] addr, int events);
        public static Int32 zmq_socket_monitor(IntPtr socket, String addr, int events) {
            return zmq_socket_monitor(socket, Encoding.ASCII.GetBytes(addr), events);
        }

        [DllImport("libzmq")]
        public static extern Int32 zmq_close(IntPtr socket);

        [DllImport("libzmq")]
        public static extern Int32 zmq_buffer_recv(IntPtr socket, IntPtr buf, int size, int flags);

        [DllImport("libzmq")]
        public static extern Int32 zmq_errno();

        [DllImport("libzmq", CharSet = CharSet.Unicode)]
        unsafe public static extern Int32 zmq_msg_recv(IntPtr msg, IntPtr socket, int flags);

        [DllImport("libzmq", CharSet = CharSet.Unicode)]
        unsafe public static extern Int32 zmq_buffer_send(IntPtr socket, IntPtr buf, int size, int flags);

        [DllImport("libzmq", CharSet = CharSet.Unicode)]
        unsafe public static extern Int32 zmq_msg_send(IntPtr msg, IntPtr socket, int flags);

        [DllImport("libzmq", CharSet = CharSet.Unicode)]
        unsafe public static extern Int32 zmq_getsockopt(IntPtr socket, int option, IntPtr optval, IntPtr optvallen);

        [DllImport("libzmq", CharSet = CharSet.Unicode)]
        unsafe public static extern Int32 zmq_setsockopt(IntPtr socket, int option, IntPtr optval, int optvallen);

        [DllImport("libzmq")]
        public static extern Int32 zmq_msg_init(IntPtr msg);

        [DllImport("libzmq")]
        public static extern Int32 zmq_msg_init_size(IntPtr msg, int size);

        [DllImport("libzmq")]
        public static extern Int32 zmq_msg_close(IntPtr msg);

        [DllImport("libzmq")]
        public static extern Int32 zmq_msg_size(IntPtr msg);

        [DllImport("libzmq")]
        public static extern IntPtr zmq_msg_data(IntPtr msg);

        [DllImport("libzmq")]
        public static extern Int32 zmq_poll([In] [Out] PollItem[] items, int numItems, long timeoutMsec);

        [DllImport("libzmq")]
        public static extern IntPtr zmq_strerror(Int32 errorCode);

        [DllImport("libzmq")]
        public static extern IntPtr zmq_ctx_new();

        [DllImport("libzmq")]
        public static extern Int32 zmq_ctx_get(IntPtr context, Int32 option);

        [DllImport("libzmq")]
        public static extern Int32 zmq_ctx_set(IntPtr context, Int32 option, Int32 optval);

        [DllImport("libzmq")]
        public static extern IntPtr zmq_socket(IntPtr context, Int32 type);

        [DllImport("libzmq")]
        public static extern Int32 zmq_ctx_destroy(IntPtr context);

        [DllImport("libzmq")]
        public static extern Int32 zmq_version(IntPtr major, IntPtr minor, IntPtr patch);
    }
    // ReSharper restore InconsistentNaming
}

#endif