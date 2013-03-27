namespace ZeroMQ.Interop
{
    using System;

    internal static class Retry
    {
#if PocketPC
        public static int zmq_buffer_recv(IntPtr socket, IntPtr buffer, int maxBufferSize, int flags) {
            int rc;

            do {
                rc = LibZmq.zmq_buffer_recv(socket, buffer, maxBufferSize, flags);
            }
            while (rc == -1 && LibZmq.zmq_errno() == ErrorCode.EINTR);

            return rc;
        }


        public static int zmq_msg_recv(IntPtr msg, IntPtr socket, int flags) {
            int rc;

            do {
                rc = LibZmq.zmq_msg_recv(msg, socket, flags);
            }
            while (rc == -1 && LibZmq.zmq_errno() == ErrorCode.EINTR);

            return rc;
        }


        public static int zmq_buffer_send(IntPtr socket, IntPtr buffer, int sizeToSend, int flags) {
            int rc;

            do {
                rc = LibZmq.zmq_buffer_send(socket, buffer, sizeToSend, flags);
            }
            while (rc == -1 && LibZmq.zmq_errno() == ErrorCode.EINTR);

            return rc;
        }

        public static int zmq_msg_send(IntPtr msg, IntPtr socket, int flags) {
            int rc;

            do {
                rc = LibZmq.zmq_msg_send(msg, socket, flags);
            }
            while (rc == -1 && LibZmq.zmq_errno() == ErrorCode.EINTR);

            return rc;
        }

        public static int zmq_getsockopt(IntPtr socket, int option, IntPtr optionValue, IntPtr optionLength) {
            int rc;

            do {
                rc = LibZmq.zmq_getsockopt(socket, option, optionValue, optionLength);
            }
            while (rc == -1 && LibZmq.zmq_errno() == ErrorCode.EINTR);

            return rc;
        }

        public static int zmq_setsockopt(IntPtr socket, int option, IntPtr optionValue, int optionLength) {
            int rc;

            do {
                rc = LibZmq.zmq_setsockopt(socket, option, optionValue, optionLength);
            }
            while (rc == -1 && LibZmq.zmq_errno() == ErrorCode.EINTR);

            return rc;
        }
#else
        public static int IfInterrupted<T1, T2, T3>(Func<T1, T2, T3, int> operation, T1 arg1, T2 arg2, T3 arg3)
        {
            int rc;

            do
            {
                rc = operation(arg1, arg2, arg3);
            }
            while (rc == -1 && LibZmq.zmq_errno() == ErrorCode.EINTR);

            return rc;
        }

        public static int IfInterrupted<T1, T2, T3, T4>(Func<T1, T2, T3, T4, int> operation, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            int rc;

            do
            {
                rc = operation(arg1, arg2, arg3, arg4);
            }
            while (rc == -1 && LibZmq.zmq_errno() == ErrorCode.EINTR);

            return rc;
        }
#endif
    }
}
